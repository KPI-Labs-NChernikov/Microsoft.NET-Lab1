using Data.Interfaces;

namespace Data.Models
{
    public class ActorOnSpectacle : IActorOnPerformance
    {
        public int Id { get; set; }

        public int SpectacleId { get; set; }

        public int ActorId { get; set; }

        public string Role { get; set; }

        public bool IsMainRole { get; set; }

        public int PerformanceId { get => SpectacleId; set { SpectacleId = value; } }
    }
}
