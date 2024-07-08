using Recipee.DTO;
using Recipee.Interfaces.Services;
using Ricettario.DTO;
using Newtonsoft.Json;

namespace Recipee.Services
{
    public class JsonService : IJsonService
    {
        private readonly IConfiguration _configuration;
        private readonly string? _importJsonFile;
        private readonly string? _exportJsonFile;

        public JsonService(IConfiguration configuration)
        {
            _configuration = configuration;
            _importJsonFile = configuration.GetValue<string>("RecipeeJson");
            _exportJsonFile = configuration.GetValue<string>("ExportedRecipee");
        }

        public RecipeeDTO? GetListOfRecipeeDeserialized()
        {
            if (string.IsNullOrEmpty(_importJsonFile) || !File.Exists(_importJsonFile))
            {
                return null;
            }
            // leggo dentro il file
            string jsonFile = File.ReadAllText(_importJsonFile);

            if (String.IsNullOrWhiteSpace(jsonFile))
            {
                return null;
            }

            // istanzio la lisa di ricette 
            //RecipeeDTO? recipeeListDeserialized = new();

            // deserializzo
            RecipeeDTO?  recipeeListDeserialized = JsonConvert.DeserializeObject<RecipeeDTO>(jsonFile);

            return recipeeListDeserialized;
        }

        public bool DownloadJson(List<RecipeDTO> listRecipee)
        {
            // controllo se esite il file se no lo creo
            if (File.Exists(_exportJsonFile))
            {
                File.Delete(_exportJsonFile);
            }

            // serializzo la lista
            string jsonList = JsonConvert.SerializeObject(listRecipee);

            if (string.IsNullOrWhiteSpace(jsonList) || string.IsNullOrEmpty(_exportJsonFile))
            {
                return false;
            }

            // scrivo nel file 
            File.WriteAllText(_exportJsonFile, jsonList);

            return true;
        }
    }
}
