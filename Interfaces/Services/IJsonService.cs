using Recipee.DTO;
using Ricettario.DTO;

namespace Recipee.Interfaces.Services
{
    public interface IJsonService
    {
        bool DownloadJson(List<RecipeDTO> listRecipee);
        RecipeeDTO? GetListOfRecipeeDeserialized();
    }
}
