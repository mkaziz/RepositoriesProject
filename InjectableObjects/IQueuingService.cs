using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RepositoriesProject.POCOs;

namespace RepositoriesProject.InjectableObjects
{
    public interface IQueuingService 
    {
        bool SendMessage();
        void SetupReceiver();
    }
}