namespace Recipee.Entity
{
    public class EntityImage
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
        public byte[]? Data { get; set; }
    }
}
