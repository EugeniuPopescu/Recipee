using Newtonsoft.Json;

namespace Ricettario.DTO
{
    public class IngredientDTO
    {
        [JsonProperty("nome_ingrediente")]
        public string? IngredientName { get; set; }

        [JsonProperty("quantita")]
        public int  Quantity{ get; set; }

        [JsonProperty("unita_misura")]
        public string? MeasurementUnit { get; set; }
    }
}
