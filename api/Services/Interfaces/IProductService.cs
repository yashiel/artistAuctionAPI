using api.Models;

namespace api.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProductsAsync(int? page = null, int? pageSize = null);
    Task<Product> GetProductByIdAsync(int id);
    Task AddProductAsync(Product product);
    Task UpdateProductAsync(int id, Product product);
    Task DeleteProductAsync(int id);
    Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category, int? page = null, int? pageSize = null);
    Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm, int? page = null, int? pageSize = null);
    Task<IEnumerable<Product>> GetProductsByArtistAsync(int artistId, int? page = null, int? pageSize = null);
    Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice, int? page = null, int? pageSize = null);

}