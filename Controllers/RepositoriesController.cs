using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RepositoriesProject.InjectableObjects;
using RepositoriesProject.POCOs;

namespace RepositoriesProject.Controllers
{
    [Route("v1/api/[controller]")]
    public class RepositoriesController : Controller
    {
        protected IGithubService GithubService { get; }
        public RepositoriesController(IGithubService githubService)
        {
            GithubService = githubService;
        }
        // GET api/values
        [HttpGet]
        public IEnumerable<GithubRepository> Get()
        {
            // pull from SQL?
            return GithubService.RetrieveRepositories("gopangea").Result;
        }
    }
}
