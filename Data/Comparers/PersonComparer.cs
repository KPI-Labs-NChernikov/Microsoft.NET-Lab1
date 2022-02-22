using Data.Interfaces;

namespace Data.Comparers
{
    public class PersonComparer<T> : ModelEqualityComparer<T> where T : IPerson
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
