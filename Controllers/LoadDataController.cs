using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RepositoriesProject.Controllers
{
    [Route("v1/api/[controller]")]
    public class LoadDataController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            // do something to RabbitMQ
            return new string[] { "value1", "value2" };
        }

    }
}
