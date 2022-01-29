﻿using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using RoverCore.Boilerplate.Infrastructure.Extensions;
using Serilog;
using Serilog.Events;

namespace RoverCore.Boilerplate.Web;

public class Program
{
    public static void Main(string[] args)
    {
        string logPath = "Logs" + Path.DirectorySeparatorChar;
        Directory.CreateDirectory(logPath);

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Debug()
            .WriteTo.File(logPath, rollingInterval: RollingInterval.Day,
                flushToDiskInterval: TimeSpan.FromSeconds(1),
                shared: true)
            .CreateLogger();


        try
        {
            Log.Information("Starting web host");
            BuildWebHost(args)
                .RunMigrations()  // Apply any new EF migrations (requires appsetting)
                .RunSeeders()     // Run any auto-registered seeders (classes that implement ISeeder) (requires appsetting)
                .Run();           // Start the web host
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }

    }

    public static IWebHost BuildWebHost(string[] args)
    {
        var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddEnvironmentVariables()
            .Build();

        return WebHost.CreateDefaultBuilder(args)
            .UseConfiguration(configuration)
            .UseSerilog()
            .UseStartup<Startup>()
            .Build();
    }

}
