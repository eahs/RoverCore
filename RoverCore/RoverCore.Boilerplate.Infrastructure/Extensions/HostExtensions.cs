﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RoverCore.Boilerplate.Domain.Entities;
using RoverCore.Boilerplate.Infrastructure.Persistence.DbContexts;
using RoverCore.Boilerplate.Infrastructure.Services.Seeder;

namespace RoverCore.Boilerplate.Infrastructure.Extensions;

public static class HostExtensions
{
    public static IWebHost RunSeeders(this IWebHost host)
    {
        using (var serviceScope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var seeder = serviceScope.ServiceProvider.GetService<ApplicationSeederService>();

            var settings = serviceScope.ServiceProvider.GetService<ApplicationSettings>();

            if (settings is { SeedDataOnStartup: true }) seeder?.SeedAsync().GetAwaiter().GetResult();
        }

        return host;
    }

    public static IWebHost RunMigrations(this IWebHost host)
    {
        using (var serviceScope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
            var settings = serviceScope.ServiceProvider.GetService<ApplicationSettings>();

            if (settings is { ApplyMigrationsOnStartup: true }) context?.Database.Migrate();
        }

        return host;
    }
}