using Newtonsoft.Json;
using Recipee.DTO;

namespace Ricettario.DTO
{
    public class RecipeDTO
    {
        public int Id { get; set; }

        [JsonProperty("nome")]
        public string? Name { get; set; }

        [JsonProperty("categoria")]
        public string? Category { get; set; }

        [JsonProperty("tempo_preparazione")]
        public int? CookingTime { get; set; }

        [JsonProperty("difficolta")]
        public string? Difficulty { get; set; }

        [JsonProperty("porzioni")]
        public int? Portions { get; set; }

        [JsonProperty("ingredienti")]
        public List<IngredientDTO>? Ingredients { get; set; }
    }
}
