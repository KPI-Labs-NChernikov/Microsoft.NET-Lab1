using Data.Models;

namespace Business.TempModels
{
    public record ActorStats
    {
        public Actor Actor { get; init; }

        public int MainRolesQuantity { get; init; }
    }
}
