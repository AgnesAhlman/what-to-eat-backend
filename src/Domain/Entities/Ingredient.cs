namespace Domain.Entities
{
    public class Ingredient
    {
        public int Id { get; set; }
        public required string Text { get; set; }
        public string? Unit { get; set; }
        public decimal? Amount { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; } = null!;
    }
}
