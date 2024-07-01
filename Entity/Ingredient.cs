namespace Recipee.Entity
{
    public class Ingredient
    {
        public int Id { get; set; }
        public int RecipeId{ get; set; }
        public string? IngredientName { get; set; }
        public int Quantity { get; set; }
        public string? MeasurementUnit { get; set; }
    }
}
