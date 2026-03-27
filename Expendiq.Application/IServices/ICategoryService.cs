using Expendiq.Application.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expendiq.Application.IServices
{
    public interface ICategoryService
    {
        /// <summary>
        /// Get category by ID
        /// </summary>
        Task<CategoryResponseDto> GetByIdAsync(int id);

        /// <summary>
        /// Get all categories for a specific user
        /// </summary>
        Task<IEnumerable<CategoryResponseDto>> GetAllAsync(string userId);

        /// <summary>
        /// Create a new category
        /// </summary>
        Task<CategoryResponseDto> CreateAsync(CreateCategoryDto dto);

        /// <summary>
        /// Update an existing category
        /// </summary>
        Task<CategoryResponseDto> UpdateAsync(int id, UpdateCategoryDto dto);

        /// <summary>
        /// Delete a category by ID
        /// </summary>
        Task DeleteAsync(int id);

        /// <summary>
        /// Check if category exists
        /// </summary>
        Task<bool> ExistsAsync(int id);

        /// <summary>
        /// Get categories by search term
        /// </summary>
        Task<IEnumerable<CategoryResponseDto>> SearchAsync(string userId, string searchTerm);
    }
}
