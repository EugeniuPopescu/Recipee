using Newtonsoft.Json;
using Ricettario.DTO;

namespace Recipee.Entity
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int CookingTime { get; set; }
        public string Difficulty { get; set; }
        public int Portions { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public string ImagePath { get; set; }
    }
}
