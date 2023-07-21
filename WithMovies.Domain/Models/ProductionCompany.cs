namespace WithMovies.Domain.Models
{
    public class ProductionCompany : BaseEntity
    {
        public required string Name { get; set; }
        public virtual ICollection<Movie> Movies { get; set; } = null!;
    }
}
