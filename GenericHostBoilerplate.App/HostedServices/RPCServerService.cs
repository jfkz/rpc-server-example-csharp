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
    internal class RPCServerService : BackgroundService
    {
        private readonly IHostApplicationLifetime _hostLifetime;
        private readonly IHostEnvironment _hostingEnv;
        private readonly IConfiguration _configuration;
        private readonly IExampleService _exampleService;
        private readonly ILogger<HostedService> _logger;

        private WatsonWsServer _server;

        public RPCServerService(
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

            /** RPC Server **/
            int port = 9000;
            string ip = "127.0.0.1";

            _server = new WatsonWsServer(ip, port, true | false);
            _server.ClientConnected += ClientConnected;
            _server.ClientDisconnected += ClientDisconnected;
            _server.MessageReceived += MessageReceived;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _server.Start();
            _logger.LogInformation("RPC Server started");

            // var result = await _exampleService.DoWork(stoppingToken);
            // _logger.LogInformation($"Example service returned {result}");

            // _logger.LogInformation("Finished executing. Exiting.");
            // _hostLifetime.StopApplication();
        }

        static void ClientConnected(object sender, ConnectionEventArgs args)
        {
            Console.WriteLine("Client connected: " + args.Client.ToString());
        }

        static void ClientDisconnected(object sender, DisconnectionEventArgs args)
        {
            Console.WriteLine("Client disconnected: " + args.Client.ToString());
        }

        static void MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            Console.WriteLine("Message received from " + args.Client.ToString() + ": " + Encoding.UTF8.GetString(args.Data));
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting up RPC Server");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping RPC Server");
            return base.StopAsync(cancellationToken);
        }        
    }
}
