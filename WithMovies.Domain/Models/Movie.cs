namespace WithMovies.Domain.Models
{
    public class Movie : BaseEntity
    {
        public required int ImdbId { get; set; }
        public required string BelongsToCollection { get; set; }
        public required string Title { get; set; }
        public required string OriginalTitle { get; set; }
        public required bool Adult { get; set; }
        public required string Overview { get; set; }
        public required int Budget { get; set; }
        public required string Genres { get; set; }
        public required string HomePage { get; set; }
        public required string PosterPath { get; set; }
        public required string ProductionCompanys { get; set; }
        public required string ProductionCountrys { get; set; }
        public required string ReleaseDate { get; set; }
        public required int Revenue { get; set; }
        public required int RunTime { get; set; }
        public required string SpokenLanguage { get; set; }
        public required string Status { get; set; }
        public required decimal VoteAverage { get; set; }
        public required decimal VoteCount { get; set; }
    }

}
