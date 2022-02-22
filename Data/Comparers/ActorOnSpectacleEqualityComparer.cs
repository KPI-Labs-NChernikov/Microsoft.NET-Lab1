using Data.Interfaces;
using Data.Models;

namespace Data.Comparers
{
    public class ActorOnSpectacleEqualityComparer : ModelEqualityComparer<ActorOnSpectacle>
    {
        public override bool Equals(ActorOnSpectacle x, ActorOnSpectacle y)
        {
            var result = base.Equals(x, y);
            if (!result || x is null)
                return result;
            return x.Role == y.Role
                && x.SpectacleId == y.SpectacleId
                && x.ActorId == y.ActorId;
        }
    }
}
