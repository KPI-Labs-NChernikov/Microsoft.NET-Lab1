using Data.Interfaces;

namespace Business.TempModels
{
    public record FilmographyItem
    {
        public string Role { get; init; }
            
        public bool IsMainRole { get; init; }
        
        public IPerformance Performance { get; init; }
    }
}
