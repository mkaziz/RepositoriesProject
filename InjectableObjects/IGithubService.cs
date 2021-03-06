using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RepositoriesProject.POCOs;

namespace RepositoriesProject.InjectableObjects
{
    public interface IGithubService 
    {
        Task<List<GithubRepository>> RetrieveRepositoriesFromGithub(string organizationName);
        List<GithubRepository> RetrieveRepositoriesFromStore();
    }
}