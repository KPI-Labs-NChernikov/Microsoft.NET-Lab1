using Data.Models;

namespace Business.TempModels
{
    public class Actor : Person
    {
        public TheatricalCharactersContainer TheatricalCharacters { get; set; }
    }
}
