namespace WithMovies.Domain.Models
{
    public class RecommendationProfile : BaseEntity
    {
        // Inputs

        /// <summary>
        /// The inputs for the algorithm, things like what movies the user
        /// watched and what rating the user gave them.
        /// </summary>
        public virtual ICollection<RecommendationProfileInput> Inputs { get; set; } = null!;

        /// <summary>
        /// Which genres the user explicitly selected as one they liked on the
        /// account settings page. This array can be indexed by a genre.
        ///
        /// <example>
        /// For example:
        ///
        /// <code>
        /// bool userLikesCrime = ExplicitlyLikedGenres[(int)Genre.Crime];
        /// </code>
        /// </example>
        /// </summary>
        public virtual bool[] ExplicitlyLikedGenres { get; set; } = new bool[20];

        // Outputs

        /// <summary>
        /// A list of recommended movies and how much they are recommended
        /// </summary>
        public virtual ICollection<WeightedMovie> MovieWeights { get; set; } = null!;

        /// <summary>
        /// How much the user likes specific genres. Every value is a float
        /// ranging from <c>0.0</c> to <c>1.0</c>, with <c>0.0</c> indicating
        /// that the user dislikes this genre, while <c>1.0</c> means the user
        /// loves it. This array can be indexed by a genre.
        ///
        /// <example>
        /// For example:
        ///
        /// <code>
        /// float crimeWeight = GenreWeights[(int)Genre.Crime];
        /// </code>
        /// </example>
        /// </summary>
        public required float[] GenreWeights = new float[20];
    }
}
