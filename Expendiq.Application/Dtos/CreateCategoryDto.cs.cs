namespace Expendiq.Application.DTOs.Category
{
    public class CreateCategoryDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; } = "#6366F1";
        public string Icon { get; set; } = "tag";
        public string UserId { get; set; }
    }
}