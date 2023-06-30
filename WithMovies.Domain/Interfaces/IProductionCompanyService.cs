using WithMovies.Domain.Models;

namespace WithMovies.Domain.Interfaces
{
	public interface IProductionCompanyService
	{
		Task<bool> ProductionCompanyExistsAsync(int id);
		Task<bool> ProductionCompanyExistsAsync(string name);
		Task<bool> ProductionCompanyExistsAsync(ProductionCompany company) =>
			ProductionCompanyExistsAsync(company.Id);

		Task<ProductionCompany?> ProductionCompanyGetAsync(int id);
		Task<ProductionCompany?> ProductionCompanyGetAsync(string name);

		Task<ProductionCompany> ProductionCompanyCreateAsync(string name);
		Task<ProductionCompany> ProductionCompanyCreateAsync(int id, string name);
		Task<ProductionCompany> ProductionCompanyCreateAsync(ProductionCompany company);
	}
}

