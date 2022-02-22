using Data.Interfaces;

namespace Data.Models
{
    public class Director : IPerson
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Patronymic { get; set; }

        public ushort BirthYear { get; set; }
    }
}
