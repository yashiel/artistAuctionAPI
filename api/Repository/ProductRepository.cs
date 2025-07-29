using api.Data;
using api.Interfaces;
using api.Models;
using api.Services;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class ProductRepository : IProductService
{
    private readonly AuctionDbContext _context;
    public ProductRepository(AuctionDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _context.Products.ToListAsync();
    }
    public async Task<Product> GetProductByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }
    public async Task AddProductAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateProductAsync(int id, Product product)
    {
        _context.Entry(product).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
    public async Task DeleteProductAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category)
    {
        return await _context.Products
            .Where(p => p.Category != null && p.Category.Name == category)
            .ToListAsync();
    }
    public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
    {
        return await _context.Products
            .Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
            .ToListAsync();
    }
    public async Task<IEnumerable<Product>> GetProductsByArtistAsync(int artistId)
    {
        return await _context.Products
            .Where(p => p.ArtistId == artistId)
            .ToListAsync();
    }
    public async Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
    {
        return await _context.Products
            .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
            .ToListAsync();
    }

}