using Business;
using ConsoleApp.Data;
using Data;
using Data.Models;
using System;
using System.Collections.Generic;

namespace ConsoleApp
{
    internal class Program
    {
        private static Context _context;

        internal static FilmographyService Service { get; private set; }

        internal static Menu MainMenu { get; private set; }

        static void Main(string[] args)
        {
            _context = new Context();
            var seeder = new DataSeeder(_context);
            seeder.SeedData();
            Service = new FilmographyService(_context);
            IEnumerable<(string, Action)> mainMenuItems = new List<(string, Action)>()
            {
                ("xcv ", null)
            };
            MainMenu = new Menu
            {
                Header = "22. Actors and their filmographies\n" +
                "Nikita Chernikov, IS-02",
                Name = "query",
                Items = mainMenuItems
            };
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            MainMenu.Print();
            var result1 = Service.GetActors();
            var result2 = Service.GetSpectacleCast(2);
            var result3 = Service.GetActorsWithFilmography();
            var result4 = Service.GetActorPerformances(6);
            var result5 = Service.GetMoviesGroupedByGenres();
            var result6 = Service.GetMoviesFromYear(2000);
            var result7 = Service.GetTopMainRolesPopularActors(55);
            var result8 = Service.FindActorByName("o");
            var result9 = Service.GetUniversalGenres();
            var result10 = Service.GetActorsDirectors();
            var result11 = Service.GetActorsByGenre(4);
            var result12 = Service.FindMoviesByDirectorName("sco");
            var result13 = Service.FindPerformancesByName("an");
            var result14 = Service.GetGenresStats();
            var result15 = Service.FindSpectaclesByGenreNameStart("d");
            var mov = new Movie();
            var str = mov.GetType().Name;
            Console.WriteLine("Hello World!");
            Console.ResetColor();
        }
    }
}
