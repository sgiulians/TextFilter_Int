using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class AppSequentialWithBatching : IAppTextFilter
{
    private readonly ILogger<AppSequentialNoBatching> _logger;
    private readonly ApplicationConfiguration _appConfig;
    private readonly SystemConfiguration _sysConfig;
    private readonly IFilterService _filterService;

    public AppSequentialWithBatching(ILogger<AppSequentialNoBatching> logger,
                                     IOptions<ApplicationConfiguration> appConfig,
                                     IOptions<SystemConfiguration> sysConfig,
                                     IFilterService filterService)
    {
        _logger = logger;
        _appConfig = appConfig.Value;
        _sysConfig = sysConfig.Value;
        _filterService = filterService;
    }

    public string[] Run() => throw new NotImplementedException();
}
