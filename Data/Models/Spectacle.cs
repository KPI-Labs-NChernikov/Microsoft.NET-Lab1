using Data.Interfaces;

namespace Data.Models
{
    public class Spectacle : IPerformance
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int GenreId { get; set;}
    }
}
