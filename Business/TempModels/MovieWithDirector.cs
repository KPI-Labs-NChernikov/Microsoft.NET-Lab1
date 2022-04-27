using Data.Models;

namespace Business.TempModels
{
    public record MovieWithDirector
    {
        public Movie Movie { get; init; }

        public Person Director { get; init; }
    }
}
