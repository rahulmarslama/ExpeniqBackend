using Expendiq.Domain.Entities;

namespace Expendiq.Infrastructure.IRepositories
{
    public interface ICategoryRepository  // ✅ Change from 'internal' to 'public'
    {
        /// <summary>
        /// Get a category by ID
        /// </summary>
        Task<Category> GetByIdAsync(int id);

        /// <summary>
        /// Get all categories for a specific user
        /// </summary>
        Task<IEnumerable<Category>> GetAllAsync(int userId);

        /// <summary>
        /// Get categories by search term
        /// </summary>
        Task<IEnumerable<Category>> SearchAsync(int userId, string searchTerm);

        /// <summary>
        /// Add a new category
        /// </summary>
        Task AddAsync(Category category);

        /// <summary>
        /// Update an existing category
        /// </summary>
        Task UpdateAsync(Category category);

        /// <summary>
        /// Delete a category by ID
        /// </summary>
        Task DeleteAsync(int id);

        /// <summary>
        /// Check if category exists
        /// </summary>
        Task<bool> ExistsAsync(int id);

        /// <summary>
        /// Save changes to database
        /// </summary>
        Task SaveAsync();

        /// <summary>
        /// Get category count for a user
        /// </summary>
        Task<int> GetCountAsync(int userId);
    }
}