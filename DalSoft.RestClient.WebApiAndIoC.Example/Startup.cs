using System;
using DalSoft.RestClient.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace DalSoft.RestClient.MvcAndIoC.Example
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // How to use HttpClientFactory with one RestClient
            services.AddRestClient("https://api.github.com", new Headers(new { UserAgent = "DalSoft.RestClient" }))
                .HttpClientBuilder.ConfigureHttpClient(client =>
                {
                    client.Timeout = TimeSpan.FromMinutes(1);
                });

            // How to use HttpClientFactory NamedClient feature to support multiple RestClients in the same project.
            services.AddRestClient("NamedGitHubClient", "https://api.github.com/orgs/", new Headers(new { UserAgent = "DalSoft.RestClient" }));
            
            services.AddMvc();
        }

        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
