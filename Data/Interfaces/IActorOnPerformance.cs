namespace Data.Interfaces
{
    public interface IActorOnPerformance : IModel
    {
        public int PerformanceId { get; set; }

        public int ActorId { get; set; }

        public string Role { get; set; }

        public bool IsMainRole { get; set; }
    }
}
