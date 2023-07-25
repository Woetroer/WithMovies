using Microsoft.EntityFrameworkCore;
using WithMovies.Domain.Interfaces;
using WithMovies.Domain.Models;

namespace WithMovies.Business.Services
{
    public class ProductionCompanyService : IProductionCompanyService
    {
        private DataContext _dataContext;

        public ProductionCompanyService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<ProductionCompany> ProductionCompanyCreateAsync(string name)
        {
            var company = new ProductionCompany { Name = name };

            await _dataContext.AddAsync(company);

            return company;
        }

        public async Task<ProductionCompany> ProductionCompanyCreateAsync(int id, string name)
        {
            var company = new ProductionCompany { Id = id, Name = name };

            await _dataContext.AddAsync(company);

            return company;
        }

        public async Task<ProductionCompany> ProductionCompanyCreateAsync(ProductionCompany company)
        {
            await _dataContext.AddAsync(company);

            return company;
        }

        public Task<bool> ProductionCompanyExistsAsync(int id) =>
            Task.FromResult(_dataContext.ProductionCompanies.Any(c => c.Id == id));

        public Task<bool> ProductionCompanyExistsAsync(string name) =>
            Task.FromResult(_dataContext.ProductionCompanies.Any(c => c.Name == name));

        public Task<ProductionCompany?> ProductionCompanyGetAsync(int id) =>
            _dataContext.ProductionCompanies.FindAsync(id).AsTask();

        public Task<ProductionCompany?> ProductionCompanyGetAsync(string name) =>
            _dataContext.ProductionCompanies.FirstOrDefaultAsync(c => c.Name == name);
    }
}
