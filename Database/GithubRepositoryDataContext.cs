using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySQL.Data.EntityFrameworkCore.Extensions;
using RepositoriesProject.POCOs;

namespace RepositoriesProject.Database
{
    public class GithubRepositoryDataContext : DbContext 
    {
        IConfiguration Configuration { get; }
        
        public GithubRepositoryDataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(Configuration.GetConnectionString("MySqlConnection"));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // builder.Entity<OwnerData>()
            //     .HasDiscriminator<int>("Type")
            //     .HasValue<Owner>(0)
            //     .HasValue<OwnerData>(1);
        }
        public DbSet<GithubRepository> GithubRepositories { get; set; }
        public DbSet<Owner> Owners { get; set; }
    }


    public class GithubRepositoryData : GithubRepository 
    {

    }

    public class OwnerData : Owner 
    {
        
    }
}