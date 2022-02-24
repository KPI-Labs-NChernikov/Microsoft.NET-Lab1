using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp
{
    public class QueriesPrinter
    {
        public static void GetActors()
        {
            var result = Program.Service.GetActors();
            HelperMethods.PrintHeader("Actors");
            foreach (var actor in result)
            {
                Console.WriteLine($"Name: {actor.GetFullName()}");
                Console.WriteLine($"Year of birth: {actor.BirthYear}");
                Console.WriteLine($"Theatrical character: {actor.TheatricalCharacter}");
                Console.WriteLine();
            }
            HelperMethods.Quit();
        }

        public static void GetMoviesFromYear()
        {
            HelperMethods.PrintHeader("Search movies from year:");
            var form = new NumberForm<ushort>()
            {
                Min = 1895,
                Max = (ushort)DateTime.Now.Year,
                Handler = ushort.TryParse
            };
            var minYear = form.GetNumber();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            var result = Program.Service.GetMoviesFromYear(minYear);
            HelperMethods.PrintHeader($"Results (movies from {minYear}):");
            foreach (var movie in result)
            {
                Console.WriteLine($"{movie.Name} ({movie.Year})");
                Console.WriteLine();
            }
            HelperMethods.Quit();
        }

        public static void GetActorPerformances()
        {
            HelperMethods.PrintHeader("Actor's filmography:");
            var menu = new Menu()
            {
                Name = "actor's id",
            };
            var menuItems = new List<(string, Action)>();
            var actors = Program.Service.GetActors();
            int chosedId = 0;
            for (int i = 0; i < actors.Count(); i++)
            {
                var actor = actors.ElementAt(i);
                menuItems.Add((actor.GetFullName(), () => { chosedId = actor.Id; }));
            }
            menu.Items = menuItems;
            menu.Print(true);
            Console.ForegroundColor= ConsoleColor.DarkGreen;
            var chosenActor = actors.SingleOrDefault(a => a.Id == chosedId);
            var filmography = Program.Service.GetActorPerformances(chosenActor.Id);
            HelperMethods.PrintHeader($"Actor's filmography: {chosenActor.GetFullName()}");
            foreach(var performance in filmography)
            {
                Console.Write(performance.Name);
                Console.Write($" ({performance.GetType().Name}");
                if (performance is Movie movie)
                {
                    Console.Write($", {movie.Year}");
                }
                Console.WriteLine(")\n");
            }
            HelperMethods.Quit();
        }

        public static void GetActorsWithFilmography()
        {
            var result = Program.Service.GetActorsWithFilmography();
            HelperMethods.PrintHeader("Actors with filmography:");
            foreach (var actor in result)
            {
                Console.WriteLine(actor.Actor.GetFullName());
                foreach (var (Role, _, Performance) in actor.Filmography)
                {
                    Console.Write($"{Role}, ");
                    Console.Write(Performance.Name);
                    Console.Write($" ({Performance.GetType().Name}");
                    if (Performance is Movie movie)
                    {
                        Console.Write($", {movie.Year}");
                    }
                    Console.WriteLine(")");
                }
                Console.WriteLine();
            }
            HelperMethods.Quit();
        }

        public static void GetSpectacleCast()
        {
            HelperMethods.PrintHeader("Spectacle's cast:");
            var menu = new Menu()
            {
                Name = "spectacle's id",
            };
            var menuItems = new List<(string, Action)>();
            var spectacles = Program.Context.Spectacles;
            int chosedId = 0;
            for (int i = 0; i < spectacles.Count; i++)
            {
                var spectacle = spectacles.ElementAt(i);
                menuItems.Add((spectacle.Name, () => { chosedId = spectacle.Id; }));
            }
            menu.Items = menuItems;
            menu.Print(true);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            var chosenSpectacle = spectacles.SingleOrDefault(a => a.Id == chosedId);
            var cast = Program.Service.GetSpectacleCast(chosenSpectacle.Id);
            HelperMethods.PrintHeader($"Spectacle's cast: {chosenSpectacle.Name}");
            foreach (var performanceRole in cast)
            {
                Console.Write($"{performanceRole.Actor.GetFullName()} -");
                    Console.Write($" {performanceRole.Role}");
                if (performanceRole.IsMainRole)
                    Console.Write(" (main)");
                    Console.WriteLine("\n");
                }
            HelperMethods.Quit();
        }

        public static void GetMoviesGroupedByGenres()
        {
            var result = Program.Service.GetMoviesGroupedByGenres();
            HelperMethods.PrintHeader("Movies by genres");
            foreach (var item in result)
            {
                Console.WriteLine($"{item.Key.Name}:");
                IEnumerable<Movie> movies = item.ToList();
                foreach(var movie in movies)
                {
                    Console.WriteLine($"{movie.Name} ({movie.Year})");
                }
                Console.WriteLine();
            }
            HelperMethods.Quit();
        }

        public static void GetTopMainRolesPopularActors()
        {
            HelperMethods.PrintHeader("Top actors (by main roles):");
            var form = new NumberForm<int>()
            {
                Min = 1,
                Handler = int.TryParse
            };
            var quantity = form.GetNumber();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            var result = Program.Service.GetTopMainRolesPopularActors(quantity);
            HelperMethods.PrintHeader($"Top-{quantity} actors (by main roles):");
            for (int i = 0; i < result.Count(); i++)
            {
                var actor = result.ElementAt(i);
                Console.WriteLine($"{i + 1}. {actor.Actor.GetFullName()}");
                Console.WriteLine($"Year of birth: {actor.Actor.BirthYear}");
                Console.WriteLine($"Theatrical character: {actor.Actor.TheatricalCharacter}");
                Console.WriteLine($"Main roles played: {actor.MainRolesQuantity}");
                Console.WriteLine();
            }
            HelperMethods.Quit();
        }

        public static void FindActorByName()
        {
            HelperMethods.PrintHeader("Find actor:");
            var name = HelperMethods.Search("actor's full name");
            var result = Program.Service.FindActorByName(name);
            Console.Clear();
            HelperMethods.PrintHeader($"Results for \"{name}\":");
            foreach (var actor in result)
            {
                Console.WriteLine($"Name: {actor.GetFullName()}");
                Console.WriteLine($"Year of birth: {actor.BirthYear}");
                Console.WriteLine($"Theatrical character: {actor.TheatricalCharacter}");
                Console.WriteLine();
            }
            HelperMethods.Quit();
        }
    }
}
