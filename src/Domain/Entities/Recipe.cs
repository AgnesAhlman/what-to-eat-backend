namespace Domain.Entities
{
    public class Recipe
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public ICollection<Category>? Categories { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
