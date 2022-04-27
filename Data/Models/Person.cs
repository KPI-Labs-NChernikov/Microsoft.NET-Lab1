using Data.Interfaces;

namespace Data.Models
{
    public class Person : IModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Patronymic { get; set; }

        public ushort BirthYear { get; set; }

        public string FullName => $"{LastName} {FirstName} {Patronymic}".TrimEnd();
    }
}
