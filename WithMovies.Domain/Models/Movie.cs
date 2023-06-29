﻿using WithMovies.Domain.Enums;

namespace WithMovies.Domain.Models
{
    public class Movie : BaseEntity
    {
        public required int ImdbId { get; set; }
        public required virtual MovieCollection? BelongsToCollection { get; set; }
        public required string Title { get; set; }
        public required string OriginalTitle { get; set; }
        public required bool Adult { get; set; }
        public required string Overview { get; set; }
        public required int Budget { get; set; }
        public required virtual ICollection<Genre> Genres { get; set; }
        public required string HomePage { get; set; }
        public required string? PosterPath { get; set; }
        public required virtual ICollection<ProductionCompany> ProductionCompanys { get; set; }
        /// <summary>
        /// ISO-3166_1 country code
        /// </summary>
        public required virtual ICollection<string> ProductionCountrys { get; set; }
        public required DateTime ReleaseDate { get; set; }
        public required ulong Revenue { get; set; }
        public required TimeSpan RunTime { get; set; }
        /// <summary>
        ///  ISO-639_1 language code
        /// </summary>
        public required string SpokenLanguage { get; set; }
        public required MovieStatus Status { get; set; }
        public required decimal VoteAverage { get; set; }
        public required decimal VoteCount { get; set; }
    }

}
