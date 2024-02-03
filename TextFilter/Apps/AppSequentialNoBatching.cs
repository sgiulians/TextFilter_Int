using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class AppSequentialNoBatching : IAppTextFilter
{
    private readonly ILogger<AppSequentialNoBatching> _logger;
    private readonly ApplicationConfiguration _appConfig;
    private readonly IFilterService _filterService;
    private readonly IFileReaderService _fileReaderService;

    public AppSequentialNoBatching(ILogger<AppSequentialNoBatching> logger,
        IOptions<ApplicationConfiguration> appConfig,
        IFilterService filterService,
        IFileReaderService fileReaderService)
    {
        _logger = logger;
        _appConfig = appConfig.Value;
        _filterService = filterService;
        _fileReaderService = fileReaderService;
    }

    public string[] Run()
    {
        try
        {
            // The sync version of ReadAllText has been used here since it's a
            // sequential version with no chunking hence free'ing the CLR IO-Bound
            // threadpool doesn't make much sense here...
            _logger.LogInformation($"Reading from: {_appConfig.InputFilePath}");
            var content = _fileReaderService.ReadAllText(_appConfig.InputFilePath);

            _logger.LogInformation($"Separating the words based on the word separators provided");
            var words = content.Split(_appConfig.WordSeparator, StringSplitOptions.RemoveEmptyEntries);

            // When defining the filter I prefer to define them for inclusion AND excluding them using
            // ... => !filter(...) as I find it more intuitive
            _logger.LogInformation($"Applying the filters provided");
            var filteredWords = words.Where(word => _filterService.Filters.All(filter => !filter(word)));

            return filteredWords.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return null;
        }
    }
}

