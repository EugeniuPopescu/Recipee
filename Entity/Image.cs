namespace Recipee.Entity
{
    public class Image
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
    }
}
