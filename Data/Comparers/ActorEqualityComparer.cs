using Data.Models;

namespace Data.Comparers
{
    public class ActorEqualityComparer : PersonComparer<Actor>
    {
        public override bool Equals(Actor x, Actor y)
        {
            var result = base.Equals(x, y);
            if (!result || x is null)
                return result;
            return x.TheatricalCharacter == y.TheatricalCharacter;
        }
    }
}
