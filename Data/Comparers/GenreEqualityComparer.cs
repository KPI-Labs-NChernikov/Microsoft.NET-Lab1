using Data.Models;

namespace Data.Comparers
{
    public class GenreEqualityComparer : ModelEqualityComparer<Genre>
    {
        public override bool Equals(Genre x, Genre y)
        {
            var result = base.Equals(x, y);
            if (!result || x is null)
                return result;
            return x.Name == y.Name;
        }
    }
}
