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

    public required SortMethod SortMethod { get; set; }
    public required SortDirection SortDirection { get; set; }
}
