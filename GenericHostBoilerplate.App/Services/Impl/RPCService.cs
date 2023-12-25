using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatsonWebsocket;

namespace GenericHostBoilerplate.App.Services.Impl
{
    internal class RPCService : IExampleService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ExampleService> _logger;

        public RPCService(IConfiguration configuration, ILogger<ExampleService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation("RPCServices is doing work");

            await Task.Delay(5000, cancellationToken);

            return _configuration["AppSettings:ExampleSetting"];
        }
    }
}
