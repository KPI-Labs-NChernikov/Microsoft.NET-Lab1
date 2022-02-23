using Data.Models;

namespace Data.Comparers
{
    public class PersonComparer<T> : ModelEqualityComparer<T> where T : Person
    {
        public override bool Equals(T x, T y)
        {
            var result = base.Equals(x, y);
            if (!result || x is null)
                return result;
            return x.FirstName == y.FirstName
                && x.LastName == y.LastName
                && x.Patronymic == y.Patronymic
                && x.BirthYear == y.BirthYear
                && x.GetType() == y.GetType();
        }
    }
}
