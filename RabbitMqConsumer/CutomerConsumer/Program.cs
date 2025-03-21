﻿using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using GraphQlDemoPostgresQl.Common.Core.RabbitMq;
using GraphQlDemoPostgresQl.Common.Abstractions;
using Microsoft.Extensions.Configuration;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        var basePath = AppContext.BaseDirectory;  // Ensure correct path
        config.SetBasePath(basePath);
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        services.Configure<RabbitMqOptions>(context.Configuration.GetSection(RabbitMqOptions.CONFIG_PATH));
        services.AddSingleton<IMessageQueueService, DefaultRabbitMqService>(); // Register RabbitMQ service as singleton
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    })
    .Build();

using var scope = host.Services.CreateScope();
var serviceProvider = scope.ServiceProvider;
var rabbitMqService = serviceProvider.GetRequiredService<IMessageQueueService>();

// Register Consumer in Console App
rabbitMqService.RegisterConsumer(async (model, ea) =>
{
    string message = Encoding.UTF8.GetString(ea.Body.ToArray());
    Console.WriteLine($"Received Message: {message}");

    try
    {
        // Simulate message processing
        await Task.Delay(500);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error processing message: {ex.Message}");
    }
});

// Keep the application running
Console.WriteLine("Consumer is listening...");
await Task.Delay(-1);
