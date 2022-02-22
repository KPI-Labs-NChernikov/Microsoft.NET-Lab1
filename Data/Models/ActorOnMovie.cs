using Data.Interfaces;

namespace Data.Models
{
    public class ActorOnMovie : IModel
    {
        public int Id { get; set; }

        public int MovieId { get; set; }

        public int ActorId { get; set; }

        public string Role { get; set; }

        public bool IsMainRole { get; set; }
    }
}
