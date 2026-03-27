using Expendiq.Domain.Entities;
using Expendiq.Infrastructure.Data;
using Expendiq.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expendiq.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Category>> GetAllAsync(string userId)
        {
            return await _context.Categories
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> SearchAsync(string userId, string searchTerm)
        {
            return await _context.Categories
                .Where(c => c.UserId == userId &&
                           c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task AddAsync(Category category)
        {
            category.CreatedAt = DateTime.UtcNow;
            await _context.Categories.AddAsync(category);
        }

        public async Task UpdateAsync(Category category)
        {
            var existingCategory = await _context.Categories.FindAsync(category.Id);
            if (existingCategory != null)
            {
                _context.Entry(existingCategory).CurrentValues.SetValues(category);
            }
            else
            {
                _context.Categories.Update(category);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Categories.AnyAsync(c => c.Id == id);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetCountAsync(string userId)
        {
            return await _context.Categories
                .Where(c => c.UserId == userId)
                .CountAsync();
        }
    }
}