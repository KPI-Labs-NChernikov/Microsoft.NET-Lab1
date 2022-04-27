using Data.Interfaces;

namespace Data.Models
{
    public class ActorOnMovie : IActorOnPerformance
    {
        public int Id { get; set; }

        public int MovieId { get; set; }

        public int ActorId { get; set; }

        public string Role { get; set; }

        public bool IsMainRole { get; set; }

        public int PerformanceId { get => MovieId; set { MovieId = value; } }
    }
}
