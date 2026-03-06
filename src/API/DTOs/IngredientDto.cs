namespace API.DTOs
{
    public class IngredientDto
    {
        public required string Text { get; set; }
        public string? Unit { get; set; }
        public decimal? Amount { get; set; }
    }
}
