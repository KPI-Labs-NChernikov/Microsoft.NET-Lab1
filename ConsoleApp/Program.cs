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
        internal static Context Context { get; private set; }

        internal static FilmographyService Service { get; private set; }

        internal static Menu MainMenu { get; private set; }

        static void Main(string[] args)
        {
            Context = new Context();
            var seeder = new DataSeeder(Context);
            seeder.SeedData();
            Service = new FilmographyService(Context);
            IEnumerable<(string, Action)> mainMenuItems = new List<(string, Action)>()
            {
                ("Get all actors. Sort by full name, then - year of birth", QueriesPrinter.GetActors),
                ("Get all films starting from any year. Sort by year descending, then by name ascending", QueriesPrinter.GetMoviesFromYear),
                ("Get all films and spectacles where actor starred in one IEnumerable. Sort by: name", QueriesPrinter.GetActorPerformances),
                ("Get all actors joined with their roles, then with films/spectacles", QueriesPrinter.GetActorsWithFilmography),
                ("Get the cast of the spectacle. sort by: type of the role", QueriesPrinter.GetSpectacleCast),
                ("Get movies grouped by genres. Sort by name of the genre", QueriesPrinter.GetMoviesGroupedByGenres),
                ("Get top-N actors. Sort by quantity of main roles both in movies and speactacles.", QueriesPrinter.GetTopMainRolesPopularActors),
                ("Find actors by fullname", QueriesPrinter.FindActorByName),
                ("Get genres that were used both in movies and spectacles", null),
                ("Get all actors that were directors at least in one movie. Sort by year of birth", null),
                ("Get all actors that starred in at least one movie or spectacle with given genre. Sort by fullname, then - year of birth", null),
                ("Find films by director's full name. Sort by film year descending", null),
                ("Find all films and spectacles by name. Group by type - spectacle or movie", null),
                ("Get genres with quantity of movies and spectacles of them. " +
                "Sort by quantity of movies desc., then - spectacles desc.", null),
                ("Find spectacles of genre by start of the name", null)
            };
            MainMenu = new Menu
            {
                Header = "22. Actors and their filmographies\n" +
                "Nikita Chernikov, IS-02",
                Name = "query",
                Items = mainMenuItems
            };
            MainMenu.Print();

            //
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
            //

            Console.ResetColor();
        }
    }
}
