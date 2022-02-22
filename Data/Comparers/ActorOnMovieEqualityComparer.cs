using Data.Models;

namespace Data.Comparers
{
    public class ActorOnMovieEqualityComparer : ModelEqualityComparer<ActorOnMovie>
    {
        public override bool Equals(ActorOnMovie x, ActorOnMovie y)
        {
            var result = base.Equals(x, y);
            if (!result || x is null)
                return result;
            return x.Role == y.Role
                && x.MovieId == y.MovieId
                && x.ActorId == y.ActorId;
        }
    }
}
