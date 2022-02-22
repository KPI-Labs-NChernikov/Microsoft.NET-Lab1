using Data.Models;
using System;

namespace Data.Interfaces
{
    public interface IPerformance : IModel
    {
        string Name { get; set; }

        int GenreId { get; set; }
    }
}
