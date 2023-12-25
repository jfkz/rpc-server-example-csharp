using GenericHostBoilerplate.App.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatsonWebsocket;

namespace GenericHostBoilerplate.App.HostedServices
{
    internal class RPCClientService : BackgroundService
    {
        private readonly IHostApplicationLifetime _hostLifetime;
        private readonly IHostEnvironment _hostingEnv;
        private readonly IConfiguration _configuration;
        private readonly IExampleService _exampleService;
        private readonly ILogger<HostedService> _logger;

        private WatsonWsClient _client;

        public RPCClientService(
            IHostApplicationLifetime hostLifetime,
            IHostEnvironment hostingEnv,
            IConfiguration configuration,
            IExampleService exampleService,
            ILogger<HostedService> logger)
        {
            _hostLifetime = hostLifetime;
            _hostingEnv = hostingEnv;
            _configuration = configuration;
            _exampleService = exampleService;
            _logger = logger;

            // RPC Client
            int port = 9000;
            string ip = "127.0.0.1";

            _client = new WatsonWsClient(ip, port, true | false);
            _client.ServerConnected += ServerConnected;
            _client.ServerDisconnected += ServerDisconnected;
            _client.MessageReceived += MessageReceived;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("RPCClientService is executing");
            _client.Start(); 

            await _client.SendAsync(Encoding.UTF8.GetBytes("Hello world!"));
            
            // var result = await _exampleService.DoWork(stoppingToken);
            // _logger.LogInformation($"Example service returned {result}");

            // _logger.LogInformation("Finished executing. Exiting.");
            // _hostLifetime.StopApplication();
        }

        static void MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            Console.WriteLine("Message from server: " + Encoding.UTF8.GetString(args.Data));
        }

        static void ServerConnected(object sender, EventArgs args)
        {
            Console.WriteLine("Server connected");
        }

        static void ServerDisconnected(object sender, EventArgs args)
        {
            Console.WriteLine("Server disconnected");
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting up");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping");
            return base.StopAsync(cancellationToken);
        }
    }
}
