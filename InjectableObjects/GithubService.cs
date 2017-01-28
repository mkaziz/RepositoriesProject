using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RepositoriesProject.POCOs;

namespace RepositoriesProject.InjectableObjects
{
    public class GithubService : IGithubService
    {
        public GithubService()
        {
            
        }

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
                    return JsonConvert.DeserializeObject<List<GithubRepository>>(stringResponse);
                }
                catch (HttpRequestException e)
                {
                    // do some real error logging
                    Console.WriteLine($"Request exception: {e.Message}");
                }
            }
            return new List<GithubRepository>();
        }
    }
}