public class FilterService : IFilterService
{
    public List<Func<string, bool>> Filters { get; private set; }

    public FilterService(List<Func<string, bool>> filters)
    {
        Filters = filters;
    }
}
