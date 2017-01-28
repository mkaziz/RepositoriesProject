using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RepositoriesProject.InjectableObjects;

namespace RepositoriesProject.Controllers
{
    [Route("v1/api/[controller]")]
    public class LoadDataController : Controller
    {
        IQueuingService QueuingService { get ;}
        public LoadDataController(IQueuingService queuingService)
        {
            QueuingService = queuingService;
        }
        // GET api/values
        [HttpGet]
        public bool Get()
        {
            // do something to RabbitMQ
            return QueuingService.SendMessage();
        }

    }
}
