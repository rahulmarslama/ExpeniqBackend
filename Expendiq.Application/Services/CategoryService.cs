using Expendiq.Application.DTOs.Category;
using Expendiq.Application.IServices;
using Expendiq.Domain.Entities;
using Expendiq.Infrastructure.IRepositories;

namespace Expendiq.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<CategoryResponseDto> GetByIdAsync(int id)
        {

            var category = await _repository.GetByIdAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID {id} not found");
            }

            return MapToDto(category);
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetAllAsync(int userId)
        {

            if (userId<=0)
                throw new ArgumentException("UserId is required");

            var categories = await _repository.GetAllAsync(userId);
            return categories.Select(MapToDto);
        }

        public async Task<CategoryResponseDto> CreateAsync(CreateCategoryDto dto)
        {

            ValidateCreateDto(dto);

            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description,
                Color = dto.Color ?? "#6366F1",
                Icon = dto.Icon ?? "tag",
                UserId = dto.UserId,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(category);
            await _repository.SaveAsync();

            return MapToDto(category);
        }

        public async Task<CategoryResponseDto> UpdateAsync(int id, UpdateCategoryDto dto)
        {

            var category = await _repository.GetByIdAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID {id} not found");
            }

            category.Name = dto.Name ?? category.Name;
            category.Description = dto.Description ?? category.Description;
            category.Color = dto.Color ?? category.Color;
            category.Icon = dto.Icon ?? category.Icon;

            await _repository.UpdateAsync(category);
            await _repository.SaveAsync();

            return MapToDto(category);
        }

        public async Task DeleteAsync(int id)
        {

            var category = await _repository.GetByIdAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID {id} not found");
            }

            await _repository.DeleteAsync(id);
            await _repository.SaveAsync();

        }

        public async Task<bool> ExistsAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            return category != null;
        }

        public async Task<IEnumerable<CategoryResponseDto>> SearchAsync(int userId, string searchTerm)
        {

            var categories = await _repository.GetAllAsync(userId);
            return categories
                .Where(c => c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .Select(MapToDto);
        }

        private void ValidateCreateDto(CreateCategoryDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Category name is required");

            if (dto.Name.Length > 100)
                throw new ArgumentException("Category name must be less than 100 characters");

            if (dto.UserId<=0)
                throw new ArgumentException("UserId is required");
        }

        private CategoryResponseDto MapToDto(Category category)
        {
            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Color = category.Color,
                Icon = category.Icon,
                UserId = category.UserId,
                CreatedAt = category.CreatedAt
            };
        }
    }
}