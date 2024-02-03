using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public class Program
{
    static void Main(string[] args)
    {
        // ENVIRONMENT
        var environment = Environment.GetEnvironmentVariable("DOTNET_ENV") ??
            "Development";

        var serviceCollection = new ServiceCollection();

        // CONFIG
        var configuration = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.{environment}.json", optional: false, reloadOnChange: true)
            .Build();


        var appSection = configuration.GetSection("Application");
        var sysSection = configuration.GetSection("System");
        var systemConfig = sysSection.Get<SystemConfiguration>();
        var appConfig = appSection.Get<ApplicationConfiguration>();

        // Configure IOPTION
        serviceCollection.Configure<ApplicationConfiguration>(appSection);
        serviceCollection.Configure<SystemConfiguration>(sysSection);

        // LOGGING
        serviceCollection.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.AddConfiguration(configuration.GetSection("Logging"));
        });

        // ADD CUSTOM FILTERS
        serviceCollection.AddFilters(appConfig.Filters);

        // ADD FILE READER (for the sake of testing...)
        serviceCollection.AddSingleton<IFileReaderService, FileReaderService>();


        // INJECT THE APP DRIVEN BY THE SYSTEM CONFIG
        // NB. AppSqeuentialNoBatching is the only version provided
        if (!systemConfig.Parallel.Enabled && !systemConfig.Batching.Enabled)
            serviceCollection.AddSingleton<IAppTextFilter, AppSequentialNoBatching>();

        // IF WE HAD OTHER IMPLEMENTATIONS AVAILABLE...
        // if (!systemConfig.Parallel.Enabled && systemConfig.Batching.Enabled)
        //    serviceCollection.AddSingleton<IAppTextFilter, AppSequentialWithBatching>();
        // if (systemConfig.Parallel.Enabled && !systemConfig.Batching.Enabled)
        //    serviceCollection.AddSingleton<IAppTextFilter, AppParallelNoBatching>();
        // if (systemConfig.Parallel.Enabled && systemConfig.Batching.Enabled)
        //     serviceCollection.AddSingleton<IAppTextFilter, AppParallelWithBatching>();


        var serviceProvider = serviceCollection.BuildServiceProvider();
        var application = serviceProvider.GetService<IAppTextFilter>();
        string [] results = application.Run();

        // PRINT OUT THE RESULTS
        foreach(var res in results)
            Console.WriteLine(res);
        
        Console.ReadKey();
    }
}
