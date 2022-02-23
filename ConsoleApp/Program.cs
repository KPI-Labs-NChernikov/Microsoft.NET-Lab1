using Business;
using ConsoleApp.Data;
using Data;
using Data.Models;
using System;
using System.Linq;

namespace ConsoleApp
{
    internal class Program
    {
        private static Context _context;

        private static FilmographyService _service;

        static void Main(string[] args)
        {
            _context = new Context();
            var seeder = new DataSeeder(_context);
            seeder.SeedData();
            _service = new FilmographyService(_context);
            var result1 = _service.GetActors();
            var result2 = _service.GetSpectacleCast(2);
            var result3 = _service.GetActorsWithFilmography();
            var result4 = _service.GetActorPerformances(6);
            var result5 = _service.GetMoviesGroupedByGenres();
            var result6 = _service.GetMoviesFromYear(2000);
            var result7 = _service.GetTopMainRolesPopularActors(55);
            var result8 = _service.FindActorByName("o");
            var result9 = _service.GetUniversalGenres();
            var result10 = _service.GetActorsDirectors();
            var result11 = _service.GetActorsByGenre(4);
            var result12 = _service.FindMoviesByDirectorName("sco");
            var result13 = _service.FindPerformancesByName("an");
            var result14 = _service.GetGenresStats();
            var result15 = _service.FindSpectaclesByGenreNameStart("d");
            var mov = new Movie();
            var str = mov.GetType().Name;
            Console.WriteLine("Hello World!");
        }
    }
}
