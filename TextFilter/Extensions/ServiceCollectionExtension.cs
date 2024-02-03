using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TextFilter.Tests")]
public static class ServiceCollectionExtension
{


    internal static void AddFilters(this ServiceCollection serviceCollection, string[] filterNames)
        => serviceCollection.AddSingleton<IFilterService>(new FilterService(LookUpFilters(filterNames)));

    public static List<Func<string, bool>> LookUpFilters(string[] filterNames)
    {
        var filters = new List<Func<string, bool>>();
        foreach (var filterName in filterNames)
        {
            var method = typeof(FilterMethods).GetMethod(filterName,
                BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);

            if (method != null)
            {
                Func<string, bool> filterDelegate =
                    (Func<string, bool>)Delegate.CreateDelegate(typeof(Func<string, bool>), method);
                filters.Add(filterDelegate);
            }
        }
        return filters;
    }
}
