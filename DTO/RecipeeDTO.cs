using Newtonsoft.Json;
using Ricettario.DTO;

namespace Recipee.DTO
{
    public class RecipeeDTO
    {
        [JsonProperty("ricette")]
        public List<RecipeDTO> Recipee { get; set; }
    }
}
