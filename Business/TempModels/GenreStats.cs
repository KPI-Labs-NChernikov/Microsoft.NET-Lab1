using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.TempModels
{
    public record GenreStats
    {
        public Genre Genre { get; init; }

        public int MoviesQuantity { get; init; }

        public int SpectaclesQuantity { get; init; }

        public int TotalQuantity
        {
            get => MoviesQuantity + SpectaclesQuantity;
        }
    }
}
