using api.Data;
using api.Models;
using api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace api.Services.Implementation;

public class ProductRepository : IProductService
{
    private readonly AuctionDbContext _context;

    public ProductRepository(AuctionDbContext context)
    {
        _context = context;
    }

    
    public async Task<IEnumerable<Product>> GetAllProductsAsync(int? page = null, int? pageSize = null)
    {
        var query = _context.Products
            .Include(p => p.Reviews)
            .AsQueryable();

        // Apply pagination only if both page and pageSize are passed
        if (page.HasValue && pageSize.HasValue)
        {
            query = query.Skip((page.Value - 1) * pageSize.Value)
                .Take(pageSize.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Reviews)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddProductAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateProductAsync(int id, Product product)
    {
        if (!await _context.Products.AnyAsync(p => p.Id == id))
            return;

        _context.Entry(product).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return;
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await _context.Products.FindAsync(new object[] { id });
        if (product == null) return;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category, int? page = null, int? pageSize = null)
    {
        var query = _context.Products
            .Include(p => p.Reviews)
            .Where(p => p.Category != null &&
                        p.Category.Name.ToLower() == category.ToLower())
            .AsQueryable();

        // Apply pagination if both page and pageSize are provided
        if (page.HasValue && pageSize.HasValue)
        {
            query = query.Skip((page.Value - 1) * pageSize.Value)
                .Take(pageSize.Value);
        }

        // Execute query
        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm, int? page = null, int? pageSize = null)
    {
        
        var query = _context.Products
            .Include(p => p.Reviews)
            .Where(p => EF.Functions.Like(p.Name, $"%{searchTerm}%")
                        || EF.Functions.Like(p.Description, $"%{searchTerm}%"))
            .AsQueryable();

        // Apply pagination if both page and pageSize are provided
        if (page.HasValue && pageSize.HasValue)
        {
            query = query.Skip((page.Value - 1) * pageSize.Value)
                .Take(pageSize.Value);
        }

        // Execute the query
        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByArtistAsync(int artistId, int? page = null, int? pageSize = null)
    {
        var query = _context.Products
            .Where(p => p.ArtistId == artistId)
            .Include(p => p.Reviews)
            .OrderBy(p => p.Id); // Always order before skip/take

        if (page.HasValue && pageSize.HasValue)
        {
            var skip = (page.Value - 1) * pageSize.Value;
            query = (IOrderedQueryable<Product>)query.Skip(skip).Take(pageSize.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice, int? page = null, int? pageSize = null)
    {
        // Start building query
        var query = _context.Products
            .Include(p => p.Reviews)
            .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
            .AsQueryable();

        // Apply pagination only if both page and pageSize are provided
        if (page.HasValue && pageSize.HasValue)
        {
            query = query.Skip((page.Value - 1) * pageSize.Value)
                .Take(pageSize.Value);
        }

        // Execute the query
        return await query.ToListAsync();
    }
}
