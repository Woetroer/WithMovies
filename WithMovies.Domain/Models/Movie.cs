using WithMovies.Domain.Enums;

namespace WithMovies.Domain.Models
{
    public class Movie : BaseEntity
    {
        public required string ImdbId { get; set; }
        public virtual MovieCollection? BelongsToCollection { get; set; } = null!;
        public required string Title { get; set; }
        public required string Tagline { get; set; }
        public required string? OriginalLanguage { get; set; }
        public required string OriginalTitle { get; set; }
        public required bool Adult { get; set; }
        public required string Overview { get; set; }
        public required int Budget { get; set; }
        public virtual ICollection<Genre> Genres { get; set; } = null!;
        public required string? HomePage { get; set; }
        public required string? PosterPath { get; set; }
        public virtual ICollection<ProductionCompany> ProductionCompanies { get; set; } = null!;

        /// <summary>
        /// ISO-3166_1 country code
        /// </summary>
        public virtual ICollection<string?> ProductionCountries { get; set; } = null!;
        public required DateTime? ReleaseDate { get; set; }
        public required ulong Revenue { get; set; }
        public required TimeSpan? Runtime { get; set; }

        /// <summary>
        ///  ISO-639_1 language code
        /// </summary>
        public virtual ICollection<string?> SpokenLanguages { get; set; } = null!;
        public required MovieStatus Status { get; set; }
        public required double VoteAverage { get; set; }
        public required int VoteCount { get; set; }
        public required double Popularity { get; set; }
        public virtual ICollection<CastMember> Cast { get; set; } = null!;
        public virtual ICollection<CrewMember> Crew { get; set; } = null!;
        public virtual ICollection<Review> Reviews { get; set; } = null!;
        public virtual ICollection<Keyword> Keywords { get; set; } = null!;
    }
}
