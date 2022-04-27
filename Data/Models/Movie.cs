using Data.Interfaces;

namespace Data.Models
{
    public class Movie : IPerformance
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int GenreId { get; set; }

        public ushort Year { get; set; }

        public int DirectorId { get; set; }
    }
}
