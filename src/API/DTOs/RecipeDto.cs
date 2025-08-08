namespace API.DTOs
{
    public class RecipeDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public List<string>? Categories { get; set; }
    }
};
