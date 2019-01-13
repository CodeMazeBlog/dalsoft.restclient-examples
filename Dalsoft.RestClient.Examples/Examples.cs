using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DalSoft.RestClient.Examples.Models;
using Xunit;

namespace DalSoft.RestClient.Examples
{
    public class LiveExamples
    {
        /*
         * Below are examples of how to use DalSoft.restClient.
         * These are live examples that actually consume the test REST API's JSONPlaceholder and JSONTest
         * All examples have a verifying test using Xunit. You can run or debug the tests using VS (Test -> Run -> All Tests or Test -> Debug -> All Tests).
         */
         
        /* How to Write Get, Post, Put/Patch, and Delete Requests */

        [Fact]
        public async Task RestClient_Get_ReturnsDynamicResponse()
        {
            // Perform a HTTP GET on https://jsonplaceholder.typicode.com/users/1  
            dynamic restClient = new RestClient("https://jsonplaceholder.typicode.com");  // This set ups the base url for DalSoft.Client
  
                               //baseurl/Users/1 GET       
            var response = await restClient.Users(1).Get();  
            
            // Verify the dynamic response object - this is the deserialized JSON body returned from https://jsonplaceholder.typicode.com/users/1 
            Assert.Equal(1, response.id);
            Assert.Equal("Leanne Graham", response.name);
            Assert.Equal("Romaguera-Crona", response.company.name);
        }

        [Fact]
        public async Task RestClient_PatchAnonymousObject_ReturnsDynamicResponse()
        {
            // Perform a HTTP PATCH on https://jsonplaceholder.typicode.com/users/1  
            dynamic restClient = new RestClient("https://jsonplaceholder.typicode.com");  // This set ups the base url for DalSoft.Client
  
            //                   baseurl/Users/1 Patch anonymous object representing JSON body required by the REST API
            var response = await restClient.Users(1).Patch(new
            {
                name = "Al Pacino",
                company = new
                {
                    name = "Universal Pictures"
                }
            });  
            
            // Verify the dynamic response object - this is the deserialized JSON body returned from https://jsonplaceholder.typicode.com/users/1 
            Assert.Equal("Al Pacino", response.name);
            Assert.Equal("Universal Pictures", response.company.name);
        }

        /* HttpResponseMessage and body ToString() */

        [Fact]
        public async Task RestClient_CastingHttpResponseMessage_ReturnsHttpResponseMessage()
        {
            // Perform a HTTP GET on https://jsonplaceholder.typicode.com/users/1  
            dynamic restClient = new RestClient("https://jsonplaceholder.typicode.com");  // This set ups the base url for DalSoft.Client
            
            //                                  baseurl/Users/1 GET       
            HttpResponseMessage httpResponseMessage = await restClient.Users(1).Get();  
            
            // Verify the cast to HttpResponseMessage
            Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
            Assert.True(httpResponseMessage.IsSuccessStatusCode);
        }

        [Fact]
        public async Task RestClient_AccessingHttpResponseMessageFromDynamicResponse_ReturnsHttpResponseMessage()
        {
            // Perform a HTTP GET on https://jsonplaceholder.typicode.com/users/1  
            dynamic restClient = new RestClient("https://jsonplaceholder.typicode.com");  // This set ups the base url for DalSoft.Client
            
            //                                  baseurl/Users/1 GET       
            var response = await restClient.Users(1).Get();  
            
            // Verify the access to HttpResponseMessage using the dynamic Response
            Assert.Equal(HttpStatusCode.OK, response.HttpResponseMessage.StatusCode); // HttpResponseMessage attached to dynamic Response
            Assert.Equal("Leanne Graham", response.name); // Also can still access the deserialized JSON body returned from  https://jsonplaceholder.typicode.com/users/1  
        }

        [Fact]
        public async Task RestClient_CallingToString_ReturnsTheBodyOfTheResponseAsAString()
        {
            // Perform a HTTP GET on https://jsonplaceholder.typicode.com/users/1  
            dynamic restClient = new RestClient("https://jsonplaceholder.typicode.com");  // This set ups the base url for DalSoft.Client
  
            //                   baseurl/Users/1 GET       
            var response = await restClient.Users(1).Get();  
            
            // Verify the string  - is the JSON string returned from https://jsonplaceholder.typicode.com/users/1 
            Assert.Matches(new Regex("\"name\": \"Leanne Graham\"", RegexOptions.Multiline), response.ToString());
        }

        /* Headers */

        [Fact]
        public async Task RestClient_CallingHeaders_AddsTheHeadersToTheRequest()
        {
            // http://headers.jsontest.com/ Echos the request headers back as JSON
            dynamic restClient = new RestClient("http://headers.jsontest.com/");  // This set ups the base url for DalSoft.Client
   
            var response = await restClient
                .Headers(new { Accept = "text/html" })  
                .Get();  
            
            // Verify the the headers were added to the request
            Assert.Equal("text/html", response.Accept);
        }

        /* Casting, Collections and Serialization */

        [Fact]
        public async Task RestClient_CastingToAStrongTypedObject_CastTheResponses()
        {
            // Perform a HTTP GET on https://jsonplaceholder.typicode.com/users 
            dynamic restClient = new RestClient("https://jsonplaceholder.typicode.com");  //This set ups the base url for DalSoft.Client
  
            //                   baseurl/Users/1 GET       
            List<User> users = await restClient.Users.Get();  
            
            // Above we cast the response to a List of User objects, the User class is in the /Models folder take note of the use of the JsonProperty attribute

            // Verify the response object has been cast to List<User> - this is the deserialized JSON body returned from https://jsonplaceholder.typicode.com/users
            var user = users.First(); // Just get the First user from the List of Users https://jsonplaceholder.typicode.com/users
            Assert.Equal(1, user.Id);
            Assert.Equal("Bret", user.UserName); // Notice the JsonProperty attribute allows us to use our own Model properties
            Assert.Equal("Romaguera-Crona", user.Company.Name); // If you use strongly-typed objects you or course get type safety and intellisense
        }
        
        [Fact]
        public async Task RestClient_UsingLinqToAccessTheDynamicResponse_AccessesValuesUsingLinq()
        {
            // Perform a HTTP GET on https://jsonplaceholder.typicode.com/users 
            dynamic restClient = new RestClient("https://jsonplaceholder.typicode.com");  //This set ups the base url for DalSoft.Client
   
            //  If the response can be deserialized to collection, you don't have to cast to a strongly-typed object the dynamic response fully supports collections

            //  You can even use LINQ by casting to a dynamic List
            List<dynamic> users = await restClient.Users.Get();
            var user = users.Single(x => x.name == "Leanne Graham");  // Let's query the response returned for "Leanne Graham"
            string name = user.name;
            
            // Remember when using a dynamic response you need to access the properties as they are returned in the JSON response.
            Assert.Equal("Leanne Graham", name);
        }

        /* Extensibility and DelegatingHandlers */

        [Fact]
        public async Task RestClient_AddCookieUsingHttpClientHandler_AddsCookieToTheRequest()
        {
            var cookieContainer = new CookieContainer(); 
            var httpClientHandler = new HttpClientHandler // Notice this is the 'standard' DelegatingHandler HttpClientHandler from System.Net.Http
            {
                CookieContainer = cookieContainer
            };  
            
            // httpbin is a test site that will allow us to to check the test cookie we set
            dynamic restClient = new RestClient("https://httpbin.org/cookies/set?testcookie=Hello_From_RestClient", new Config(httpClientHandler));  
  
            await restClient.Get();  
  
            Assert.Equal("Hello_From_RestClient", cookieContainer.GetCookies(new Uri("https://httpbin.org"))["testcookie"]?.Value);  
        }

        [Fact]
        public async Task RestClient_ExtendUsingAFunc_CallsTheFuncAsPartOfTheRequest()
        {
            // Here we use http://headers.jsontest.com/ which echos request headers as JSON
            
            dynamic restClient = new RestClient("http://headers.jsontest.com/", new Config()  
                .UseHandler(async (request, token, next) =>  
                {   // You can do anything to the HttpRequestMessage or HttpResponseMessage here  
                    
                    request.Headers.Add("Authorization", "Bearer your_bearer_token");   
                    
                    return await next(request, token); //Call next handler in pipeline  
                }));

            var response = await restClient.Get();

            // Verify that the delegate set the Authorization header
            Assert.Equal("Bearer your_bearer_token", response.Authorization);
        }

        /* Exception Handling */

        [Fact]
        public async Task RestClient_EnsureSuccessStatusCode_ThrowsHttpRequestException()
        {
            // Here we use https://httpstat.us/500 which allows us to force a 500 status
            dynamic client = new RestClient("https://httpstat.us/500");  
  
            var response = await client.Get();   
            HttpResponseMessage httpResponseMessage = response;
            
            // Verify that we can call EnsureSuccessStatusCode using DalSoft.RestClient
            Assert.Throws<HttpRequestException>(() => httpResponseMessage.EnsureSuccessStatusCode());
        }

        /* Unit Testing with DalSoft.RestClient */

        [Fact]
        public async Task RestClient_UnitHandlerMockResponse_ReturnsOurMockResponse()
        {
            // Here we configure the UnitTestHandler tp Mock the Http response to always return "{'foo':'bar'}
            
            var config = new Config()    
                .UseUnitTestHandler(request => new HttpResponseMessage    
                {    
                    Content = new StringContent("{'foo':'bar'}")    
                });    
    
            dynamic client = new RestClient("http://test.com", config);  
  
            var result = await client.whatever.Get();  
  
            Assert.Equal("{'foo':'bar'}", result.ToString());  
        }

    }


}
