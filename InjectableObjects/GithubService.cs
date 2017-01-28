using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RepositoriesProject.Database;
using RepositoriesProject.POCOs;

namespace RepositoriesProject.InjectableObjects
{
    public class GithubService : IGithubService
    {
        IConfigurationRoot Configuration { get; }
        public GithubService(IConfigurationRoot configuration)
        {
            Configuration = configuration;
        }
        // public GithubService()
        // {
            
        // }
        public async Task<List<GithubRepository>> RetrieveRepositories(string organizationName)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("https://api.github.com");
                    client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "http://developer.github.com/v3/#user-agent-required");
                    var response = await client.GetAsync($"/orgs/{organizationName}/repos");
                    response.EnsureSuccessStatusCode(); // Throw in not success

                    var stringResponse = await response.Content.ReadAsStringAsync();
                    var results = JsonConvert.DeserializeObject<List<GithubRepository>>(stringResponse);

                    using (var context = new GithubRepositoryDataContext(Configuration)) 
                    {
                        context.Database.EnsureCreated();
                        context.AddRange(results);
                        context.SaveChanges();
                    }

                    return results;
                }
                catch (Exception e)
                {
                    // do some real error logging
                    Console.WriteLine($"Exception: {e.Message}");
                }
            }
            return new List<GithubRepository>();
        }
    }
}