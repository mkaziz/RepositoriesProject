using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RepositoriesProject.Database;
using RepositoriesProject.POCOs;

namespace RepositoriesProject.InjectableObjects
{
    public class QueuingService : IQueuingService
    {
        IConfigurationRoot Configuration { get; }
        IGithubService GithubService { get; }
        public QueuingService(IConfigurationRoot configuration, IGithubService githubService)
        {
            Configuration = configuration;
            GithubService = githubService;
        }

        public bool SendMessage()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string message = "Get Repositories!";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "hello",
                                     basicProperties: null,
                                     body: body);

                return true;
            }
        }

        public void SetupReceiver()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare(queue: "hello",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                GithubService.RetrieveRepositoriesFromGithub("gopangea");
            };
            channel.BasicConsume(queue: "hello",
                                    noAck: true,
                                    consumer: consumer);
        }
    }
}