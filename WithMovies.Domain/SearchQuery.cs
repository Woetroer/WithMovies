namespace WithMovies.Domain;

public enum SortMethod
{
    Rating,
    Relevance,
    Popularity,
    ReleaseDate,
}

public enum FilterState
{
    Exclude,
    Include,
    None,
}

public enum SortDirection
{
    Ascending,
    Descending,
}

public class SearchQuery
{
    public required string Text { get; set; }
    public required FilterState Adult { get; set; }
    public required Dictionary<string, FilterState> Filters { get; set; }

    public required SortMethod SortMethod { get; set; }
    public required SortDirection SortDirection { get; set; }

    public string[] GetIncludeFilters()
    {
        return Filters
            .Where(pair => pair.Value == FilterState.Include)
            .Select(pair => pair.Key)
            .ToArray();
    }

    public string[] GetExcludeFilters()
    {
        return Filters
            .Where(pair => pair.Value == FilterState.Exclude)
            .Select(pair => pair.Key)
            .ToArray();
    }
}
