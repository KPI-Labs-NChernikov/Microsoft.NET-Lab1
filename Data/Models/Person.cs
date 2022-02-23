﻿using Data.Interfaces;

namespace Data.Models
{
    public abstract class Person : IModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Patronymic { get; set; }

        public ushort BirthYear { get; set; }

        public string GetFullName()
        {
            var fullName = $"{LastName} {FirstName}";
            if (Patronymic is null)
                return fullName;
            return fullName + " " + Patronymic;
        }
    }
}