public interface IFilterService
{
    List<Func<string, bool>> Filters { get; }
}
