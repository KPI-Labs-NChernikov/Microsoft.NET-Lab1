using Data.Interfaces;
using System;

namespace Data.Models
{
    public class Genre : IModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
