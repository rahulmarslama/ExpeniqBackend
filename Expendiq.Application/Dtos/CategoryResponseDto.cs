namespace Expendiq.Application.DTOs.Category
{
    public class CategoryResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string Icon { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}