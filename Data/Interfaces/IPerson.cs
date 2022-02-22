namespace Data.Interfaces
{
    public interface IPerson : IModel
    {
        string FirstName { get; set; }

        string LastName { get; set; }

        string Patronymic { get; set; }

        ushort BirthYear { get; set; }

        public string GetFullName()
        {
            var fullName = $"{LastName} {FirstName}";
            if (Patronymic is null)
                return fullName;
            return fullName + " " + Patronymic;
        }
    }
}
