using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public async Task<List<GithubRepository>> RetrieveRepositoriesFromGithub(string organizationName)
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
                        foreach (var item in results) 
                        {
                            if (!context.GithubRepositories.Any(i => i.Id == item.Id)) 
                            {
                                context.GithubRepositories.Add(item);
                                var owner = context.Owners.FirstOrDefault(i => i.Id == item.Owner.Id);
                                if (owner != null)
                                    context.Entry(owner).State = EntityState.Unchanged;
                            }
                            else if (!context.Owners.Any(i => i.Id == item.Owner.Id))
                                context.Owners.Add(item.Owner);
                        }
                        context.SaveChanges();
                    }

                    return results;
                }
                catch (Exception e)
                {
                    // TODO: do some real error logging here
                    Console.WriteLine($"Exception: {e.Message}");
                }
            }
            return new List<GithubRepository>();
        }

        public List<GithubRepository> RetrieveRepositoriesFromStore()
        {
            using (var context = new GithubRepositoryDataContext(Configuration)) 
            {
                context.Database.EnsureCreated();
                var results = context.GithubRepositories;
                return results.ToList();
            }
        }
    }
}