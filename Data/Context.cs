using Data.Interfaces;
using Data.Models;
using System.Collections.Generic;

namespace Data
{
    public class Context
    {
        public ICollection<Person> People { get; set; } = new List<Person>();

        public ICollection<Genre> Genres { get; set; } = new List<Genre>();

        public ICollection<Movie> Movies { get; set; } = new List<Movie>();

        public ICollection<Spectacle> Spectacles { get; set; } = new List<Spectacle>();

        public ICollection<ActorOnMovie> ActorsOnMovies { get; set; } = new List<ActorOnMovie>();

        public ICollection<ActorOnSpectacle> ActorsOnSpectacles { get; set; } = new List<ActorOnSpectacle>();
    }
}
