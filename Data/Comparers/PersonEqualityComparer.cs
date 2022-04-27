using Data.Models;

namespace Data.Comparers
{
    public class PersonEqualityComparer : ModelEqualityComparer<Person>
    {
        public override bool Equals(Person x, Person y)
        {
            var result = base.Equals(x, y);
            if (!result || x is null)
                return result;
            return x.FirstName == y.FirstName
                && x.LastName == y.LastName
                && x.Patronymic == y.Patronymic
                && x.BirthYear == y.BirthYear;
        }
    }
}
