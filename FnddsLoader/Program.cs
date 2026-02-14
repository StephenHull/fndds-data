using FnddsData.FnddsLoader;
using FnddsData.FnddsLoader.Contexts;
using FnddsData.FnddsLoader.Extensions;
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

                services.AddScoped<FnddsLoader>();
            });

    using var host = builder.Build();

    using var scope = host.Services.CreateScope();

    var fnddsLoader = scope.ServiceProvider.GetRequiredService<FnddsLoader>();

    await fnddsLoader.ImportDataAsync(arguments.FnddsVersion, arguments.ConnectionString);

    Environment.Exit(0);
}
catch (Exception e)
{
    Console.WriteLine("Failed to import the data.");
    Console.WriteLine(e);

    Environment.Exit(-1);
}
