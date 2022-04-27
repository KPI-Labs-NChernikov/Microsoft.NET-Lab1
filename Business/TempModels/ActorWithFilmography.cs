using Data.Interfaces;
using Data.Models;
using System.Collections.Generic;

namespace Business.TempModels
{
    public record ActorWithFilmography
    {
        public Actor Actor { get; init; }

        public IEnumerable<FilmographyItem> Filmography { get; init; }
    }
}
