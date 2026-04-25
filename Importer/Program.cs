using FoodAndNutrientData.Importer;
using FoodAndNutrientData.Importer.Contexts;
using FoodAndNutrientData.Importer.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

try
{
    var arguments = args.GetArguments();

    var builder =
        Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            })
            .ConfigureServices((hostingContext, services) =>
            {
                var configuration = hostingContext.Configuration;

                var connectionString = configuration.GetConnectionString("Default");

                services.AddDbContext<FnddsDbContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                });

                services.AddScoped<Importer>();
            });

    using var host = builder.Build();

    using var scope = host.Services.CreateScope();

    var importer = scope.ServiceProvider.GetRequiredService<Importer>();

    await importer.ImportDataAsync(arguments.FnddsVersion, arguments.FnddsConnectionString,
        arguments.FpedConnectionString, arguments.FpidConnectionString);

    Console.WriteLine("Successfully imported the data.");

    Environment.Exit(0);
}
catch (Exception e)
{
    Console.WriteLine("Failed to import the data.");
    Console.WriteLine(e);

    Environment.Exit(-1);
}
