using System.Diagnostics;
using System.Text.Json;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using WithMovies.Data.Sqlite;
using WithMovies.Domain;
using WithMovies.Domain.Enums;
using WithMovies.Domain.Interfaces;
using WithMovies.Domain.Models;

namespace WithMovies.Business.Services
{
    class MovieCollectionImport
    {
        public required string Name { get; set; }
        public required string? PosterPath { get; set; }
        public required string? BackdropPath { get; set; }
    }

    class MovieImport
    {
        public required int Id { get; set; }
        public required string ImdbId { get; set; }
        public required bool Adult { get; set; }
        public MovieCollectionImport? BelongsToCollection { get; set; }
        public required int Budget { get; set; }
        public required List<Genre> Genres { get; set; }
        public string? Homepage { get; set; }
        public string? OriginalLanguage { get; set; }
        public required string OriginalTitle { get; set; }
        public required string Overview { get; set; }
        public required double Popularity { get; set; }
        public string? PosterPath { get; set; }
        public required List<ProductionCompany> ProductionCompanies { get; set; }
        public required List<string?> ProductionCountries { get; set; }
        public int[]? ReleaseDate { get; set; }
        public required ulong Revenue { get; set; }
        public int[]? Runtime { get; set; }
        public required List<string?> SpokenLanguages { get; set; }
        public MovieStatus? MovieStatus { get; set; }
        public required string Tagline { get; set; }
        public required string Title { get; set; }
        public string? Video { get; set; }
        public required double VoteAverage { get; set; }
        public required int VoteCount { get; set; }
    }

    public class SqliteMovieService : IMovieService
    {
        private readonly DataContext _dataContext;
        private readonly IKeywordService _keywordService;
        private readonly ILogger<SqliteMovieService> _logger;
        private readonly IMovieCollectionService _movieCollectionService;

        public SqliteMovieService(
            DataContext dataContext,
            IMovieCollectionService movieCollectionService,
            ILogger<SqliteMovieService> logger,
            IKeywordService keywordService
        )
        {
            _logger = logger;
            _dataContext = dataContext;
            _movieCollectionService = movieCollectionService;
            _keywordService = keywordService;
        }

        public async Task ImportJsonAsync(Stream json)
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new GenericEnumJsonConverter<Genre>());
            options.Converters.Add(new GenericEnumJsonConverter<MovieStatus>());

            var movies = new List<Movie>();
            var movieImports = (
                await JsonSerializer.DeserializeAsync<List<MovieImport>>(json, options)
            )!;

            // for progress bar
            double progress = 0.0;
            double step = 1.0 / (movieImports.Count - 1);
            int iteration = 0;

            foreach (MovieImport import in movieImports)
            {
                progress += step;
                iteration++;

                //if (import is null)
                //    throw new ArgumentException("Incorrect import");

                // There are a few duplicate rows in the dataset
                if (movies.Any(m => m.Id == import.Id))
                    continue;

                MovieCollection? collection = null;

                if (import.BelongsToCollection is MovieCollectionImport collectionImport)
                {
                    if (
                        !await _movieCollectionService.MovieCollectionExistsAsync(
                            collectionImport.Name
                        )
                    )
                    {
                        collection = await _movieCollectionService.MovieCollectionCreateAsync(
                            collectionImport.Name,
                            collectionImport.PosterPath,
                            collectionImport.BackdropPath
                        );
                    }
                    else
                    {
                        collection = await _movieCollectionService.MovieCollectionGetByNameAsync(
                            collectionImport.Name
                        );
                    }
                }

                if (iteration % 500 == 0)
                {
                    string progressBar = $"|{new string('=', (int)(progress * 10.0)) + ">", -11}|";
                    _logger.LogInformation($"{progressBar} Adding movies");
                }

                movies.Add(
                    new Movie
                    {
                        Id = import.Id,
                        ImdbId = import.ImdbId,
                        Adult = import.Adult,
                        BelongsToCollection = collection,
                        Budget = import.Budget,
                        Genres = import.Genres,
                        HomePage = import.Homepage,
                        OriginalLanguage = import.OriginalLanguage,
                        OriginalTitle = import.OriginalTitle,
                        Overview = import.Overview,
                        Popularity = import.Popularity,
                        PosterPath = import.PosterPath,
                        ProductionCompanies = import.ProductionCompanies
                            .Select(c => new ProductionCompany { Name = c.Name })
                            .ToList(),
                        ProductionCountries = import.ProductionCountries,
                        ReleaseDate =
                            import.ReleaseDate != null
                                ? new DateTime(import.ReleaseDate[0], 1, 1).AddDays(
                                    import.ReleaseDate[1]
                                )
                                : null,
                        Revenue = import.Revenue,
                        Runtime =
                            import.Runtime != null
                                ? new TimeSpan(0, 0, import.Runtime[0], import.Runtime[1])
                                : null,
                        SpokenLanguages = import.SpokenLanguages,
                        Status = import.MovieStatus ?? MovieStatus.None,
                        Tagline = import.Tagline,
                        Title = import.Title,
                        VoteAverage = import.VoteAverage,
                        VoteCount = import.VoteCount,
                    }
                );
            }

            await _dataContext.AddRangeAsync(movies);
        }

        public Task<Movie?> GetByIdAsync(int movieId) =>
            _dataContext.Movies.Include(m => m.Keywords).FirstOrDefaultAsync(m => m.Id == movieId);

        public async Task<IQueryable<Movie>> GetTrending(int start, int limit)
        {
            return _dataContext
                .LoadExtension("math")
                .Movies.FromSqlRaw(
                    """
                    SELECT * FROM Movies
                    ORDER BY pow(VoteCount, VoteAverage) DESC
                    LIMIT :limit
                    OFFSET :start
                    """,
                    new SqliteParameter(":start", start),
                    new SqliteParameter(":limit", limit)
                );
        }

        public async Task<IQueryable<Movie>> GetTrending(User user, int start, int limit)
        {
            return _dataContext
                .LoadExtension("math")
                .Movies.FromSqlRaw(
                    """
                    SELECT *, AVG(KwWeight) AS Weight
                    FROM Movies,
                         (
                             SELECT r.KeywordId AS KeywordId,
                                    r.Weight AS KwWeight,
                                    l.MoviesId AS MovieId
                             FROM KeywordMovie l
                             INNER JOIN WeightedKeywords r
                             ON l.KeywordsId = r.KeywordId
                             WHERE r.ParentId = :rProfileId
                         )
                    WHERE Movies.Id = MovieId
                    GROUP BY MovieId
                    ORDER BY Weight * Weight DESC
                    LIMIT :limit
                    OFFSET :start
                    """,
                    new SqliteParameter(":rProfileId", user.RecommendationProfile.Id),
                    new SqliteParameter(":start", start),
                    new SqliteParameter(":limit", limit)
                );
        }

        public async Task<IQueryable<Movie>> GetFriendMovies(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryable<Movie>> GetWatchList(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<SearchResults> Query(SearchQuery query, int start, int limit)
        {
            DateTime startTime = DateTime.Now;

            var movies = _dataContext.LoadExtension("math").Movies.AsQueryable();

            var excludeFilters = query.GetExcludeFilters();
            if (excludeFilters.Length > 0)
            {
                var exclude = await _keywordService.FindKeywords(excludeFilters);
                movies = movies.Where(m => !m.Keywords.Any(k => exclude.Contains(k)));
            }

            var includeFilters = query.GetIncludeFilters();
            if (includeFilters.Length > 0)
            {
                var include = await _keywordService.FindKeywords(includeFilters);
                movies = movies.Where(m => m.Keywords.Any(k => include.Contains(k)));
            }

            if (!string.IsNullOrWhiteSpace(query.Text))
            {
                movies = movies.Where(
                    m =>
                        m.Title.ToLower().Contains(query.Text.ToLower())
                        || m.Overview.ToLower().Contains(query.Text.ToLower())
                        || m.Tagline.ToLower().Contains(query.Text.ToLower())
                        || (
                            m.BelongsToCollection != null
                            && m.BelongsToCollection.Name.ToLower().Contains(query.Text.ToLower())
                        )
                );
            }

            movies = query.SortMethod switch
            {
                SortMethod.Relevance => movies.OrderByDescending(m => m.Popularity),
                SortMethod.Popularity => movies.OrderByDescending(m => m.Popularity),
                SortMethod.Rating => movies.OrderByDescending(m => m.VoteCount * m.VoteAverage),
                SortMethod.ReleaseDate
                    => movies.OrderByDescending(m => m.ReleaseDate ?? DateTime.MaxValue),
                _ => throw new UnreachableException(),
            };

            if (query.SortDirection == SortDirection.Ascending)
                movies = movies.Reverse();

            movies = movies.Skip(start).Take(limit);

            double time = (DateTime.Now - startTime).TotalSeconds;

            return new SearchResults
            {
                Time = time,
                Movies = movies
                    .Select(
                        m =>
                            new PreviewDto
                            {
                                Id = m.Id,
                                Title = m.Title,
                                PosterPath = m.PosterPath,
                                Tagline = m.Tagline,
                            }
                    )
                    .ToArray(),
            };
        }
    }
}
