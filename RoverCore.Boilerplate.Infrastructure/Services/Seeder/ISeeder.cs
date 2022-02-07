﻿using Serviced;

namespace RoverCore.Boilerplate.Infrastructure.Services.Seeder;

public interface ISeeder : IScoped
{
    Task SeedAsync();
}

public interface ISeeder<T> : ISeeder
{
}
/*
public interface ISeeder<T> : IScoped<T>, ISeeder
{
}
*/