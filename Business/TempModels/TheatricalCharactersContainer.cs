using Data.Models;
using System.Collections.Generic;

namespace Business.TempModels
{
    public record TheatricalCharactersContainer
    {
        public IEnumerable<TheatricalCharacter> Value { get; init; }

        public override string ToString()
        {
            var character = string.Join("; ", Value);
            if (string.IsNullOrEmpty(character))
                character = "No particular theatre character";
            return character;
        }
    }
}
