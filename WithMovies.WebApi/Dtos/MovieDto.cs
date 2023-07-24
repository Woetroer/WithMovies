using WithMovies.Domain.Enums;
using WithMovies.Domain.Models;


namespace WithMovies.WebApi.Dtos
{
    public class MovieDto
    {
        public string? ImdbId { get; set; }
        public virtual MovieCollectionDto? BelongsToCollection { get; set; }
        public string? Title { get; set; }
        public string? Tagline { get; set; }
        public string? OriginalLanguage { get; set; }
        public string? OriginalTitle { get; set; }
        public bool Adult { get; set; }
        public string? Overview { get; set; }
        public int Budget { get; set; }
        public ICollection<int>? Genres { get; set; }
        public string? HomePage { get; set; }
        public string? PosterPath { get; set; }
        public ICollection<NamedId>? ProductionCompanies { get; set; }
        /// <summary>
        /// ISO-3166_1 country code
        /// </summary>
        public ICollection<string?>? ProductionCountries { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public ulong Revenue { get; set; }
        public TimeSpan? Runtime { get; set; }
        /// <summary>
        ///  ISO-639_1 language code
        /// </summary>
        public ICollection<string?>? SpokenLanguages { get; set; }
        public MovieStatus Status { get; set; }
        public double VoteAverage { get; set; }
        public int VoteCount { get; set; }
        public double Popularity { get; set; }
        public ICollection<string> Keywords { get; set; }
    }
}
