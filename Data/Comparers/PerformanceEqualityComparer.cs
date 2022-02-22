using Data.Interfaces;
using Data.Models;

namespace Data.Comparers
{
    public class PerformanceEqualityComparer : ModelEqualityComparer<IPerformance>
    {
        public override bool Equals(IPerformance x, IPerformance y)
        {
            var result = base.Equals(x, y);
            if (!result || x is null)
                return result;
            result = x.Name == y.Name
                && x.GenreId == y.GenreId
                && x.GetType() == y.GetType();
            if (result && x is Movie mx && y is Movie my)
                result = mx.Year == my.Year
                    && mx.DirectorId == my.DirectorId;
            return result;
        }
    }
}
