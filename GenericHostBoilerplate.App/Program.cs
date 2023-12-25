﻿// See https://aka.ms/new-console-template for more information
using GenericHostBoilerplate.App.HostedServices;
using GenericHostBoilerplate.App.Services;
using GenericHostBoilerplate.App.Services.Impl;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

CreateHostBuilder(args).Build().Run();

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            services.AddLogging();
            services.AddTransient<IExampleService, RPCService>();
            services.AddHostedService<RPCServerService>();
            services.AddHostedService<RPCClientService>();
        })
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            var env = hostingContext.HostingEnvironment;

            config.AddEnvironmentVariables();
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true);
        });