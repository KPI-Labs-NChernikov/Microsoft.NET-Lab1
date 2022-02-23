using Data.Models;

namespace Business.TempModels
{
    public record SpectacleExtended
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public Genre Genre { get; init; }
    }
}
