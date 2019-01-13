using System.Collections.Generic;
using System.Threading.Tasks;
using DalSoft.RestClient.DependencyInjection;
using DalSoft.RestClient.MvcAndIoC.Example.Models;
using Microsoft.AspNetCore.Mvc;

namespace DalSoft.RestClient.MvcAndIoC.Example
{
    public class GitExampleController : Controller
    {
        private readonly IRestClientFactory _restClientFactory;
        
        public GitExampleController(IRestClientFactory restClientFactory)
        {
            _restClientFactory = restClientFactory;
        }

        [Route("examples/createclient"), HttpGet]
        public async Task<List<Repository>> CreateClient()
        {
            dynamic restClient = _restClientFactory.CreateClient();
            
            var repositories = await restClient.users.codemazeblog.repos.Get();
            
            return repositories;
        }
        [Route("examples/createclientnamed"), HttpGet]
        public async Task<List<Repository>> CreateClientNamed()
        {
            dynamic restClient = _restClientFactory.CreateClient("NamedGitHubClient");
            
            var repositories = await restClient.dotnet.repos.Get();
            
            return repositories;
        }
    }
}
