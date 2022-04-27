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
                Console.WriteLine($"Name: {actor.FullName}");
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
                Name = "actor's number",
            };
            var menuItems = new List<(string, Action)>();
            var actors = Program.Service.GetActors();
            int chosenId = -1;
            for (int i = 0; i < actors.Count(); i++)
            {
                var actor = actors.ElementAt(i);
                menuItems.Add((actor.FullName, () => { chosenId = actor.Id; }));
            }
            menu.Items = menuItems;
            menu.Print(true);
            Console.ForegroundColor= ConsoleColor.DarkGreen;
            if (chosenId != -1)
            {
                var chosenActor = actors.SingleOrDefault(a => a.Id == chosenId);
                var filmography = Program.Service.GetActorPerformances(chosenActor.Id);
                HelperMethods.PrintHeader($"Actor's filmography: {chosenActor.FullName}");
                foreach (var performance in filmography)
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
        }

        public static void GetActorsWithFilmography()
        {
            var result = Program.Service.GetActorsWithFilmography();
            HelperMethods.PrintHeader("Actors with filmography:");
            foreach (var actor in result)
            {
                Console.WriteLine(actor.Actor.FullName);
                foreach (var item in actor.Filmography)
                {
                    Console.Write($"{item.Role}, ");
                    Console.Write(item.Performance.Name);
                    Console.Write($" ({item.Performance.GetType().Name}");
                    if (item.Performance is Movie movie)
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
                Name = "spectacle's number",
            };
            var menuItems = new List<(string, Action)>();
            var spectacles = Program.Context.Spectacles;
            int chosenId = -1;
            for (int i = 0; i < spectacles.Count; i++)
            {
                var spectacle = spectacles.ElementAt(i);
                menuItems.Add((spectacle.Name, () => { chosenId = spectacle.Id; }));
            }
            menu.Items = menuItems;
            menu.Print(true);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            if (chosenId != -1)
            {
                var chosenSpectacle = spectacles.SingleOrDefault(a => a.Id == chosenId);
                var cast = Program.Service.GetSpectacleCast(chosenSpectacle.Id);
                HelperMethods.PrintHeader($"Spectacle's cast: {chosenSpectacle.Name}");
                foreach (var performanceRole in cast)
                {
                    Console.Write($"{performanceRole.Actor.FullName} -");
                    Console.Write($" {performanceRole.Role}");
                    if (performanceRole.IsMainRole)
                        Console.Write(" (main)");
                    Console.WriteLine("\n");
                }
            HelperMethods.Quit();
            }
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
                Console.WriteLine($"{i + 1}. {actor.Actor.FullName}");
                Console.WriteLine($"Year of birth: {actor.Actor.BirthYear}");
                Console.WriteLine($"Theatrical character: {actor.Actor.TheatricalCharacter}");
                Console.WriteLine($"Main roles played: {actor.MainRolesQuantity}");
                Console.WriteLine();
            }
            HelperMethods.Quit();
        }

        public static void FindActorByName()
        {
            HelperMethods.PrintHeader("Find actor");
            var name = HelperMethods.Search("actor's full name");
            var result = Program.Service.FindActorByName(name);
            Console.Clear();
            HelperMethods.PrintHeader("Find actor");
            HelperMethods.PrintHeader($"Results for \"{name}\":");
            foreach (var actor in result)
            {
                Console.WriteLine($"Name: {actor.FullName}");
                Console.WriteLine($"Year of birth: {actor.BirthYear}");
                Console.WriteLine($"Theatrical character: {actor.TheatricalCharacter}");
                Console.WriteLine();
            }
            HelperMethods.Quit();
        }

        public static void GetUniversalGenres()
        {
            var result = Program.Service.GetUniversalGenres();
            HelperMethods.PrintHeader("Genres for both movies and spectacles:");
            foreach (var genre in result)
                Console.WriteLine($"{genre.Name}\n");
            HelperMethods.Quit();
        }

        public static void GetActorsDirectors()
        {
            var result = Program.Service.GetActorsDirectors();
            HelperMethods.PrintHeader("Actors that were directors too:");
            foreach (var actor in result)
            {
                Console.WriteLine($"Name: {actor.FullName}");
                Console.WriteLine($"Year of birth: {actor.BirthYear}");
                Console.WriteLine($"Theatrical character (as actor): {actor.TheatricalCharacter}");
                Console.WriteLine();
            }
            HelperMethods.Quit();
        }

        public static void GetActorsByGenre()
        {
            HelperMethods.PrintHeader("Actors that stared in films/spectacles of specific genre:");
            var menu = new Menu()
            {
                Name = "genre's number",
            };
            var menuItems = new List<(string, Action)>();
            var genres = Program.Context.Genres;
            int chosenId = -1;
            for (int i = 0; i < genres.Count; i++)
            {
                var genre = genres.ElementAt(i);
                menuItems.Add((genre.Name, () => { chosenId = genre.Id; }));
            }
            menu.Items = menuItems;
            menu.Print(true);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            if (chosenId != -1)
            {
                var chosenGenre = genres.SingleOrDefault(g => g.Id == chosenId);
                var actors = Program.Service.GetActorsByGenre(chosenGenre.Id);
                HelperMethods.PrintHeader($"Actors that stared in {chosenGenre.Name} films/spectacles:");
                foreach (var actor in actors)
                {
                    Console.WriteLine($"Name: {actor.FullName}");
                    Console.WriteLine($"Year of birth: {actor.BirthYear}");
                    Console.WriteLine($"Theatrical character: {actor.TheatricalCharacter}");
                    Console.WriteLine();
                }
                HelperMethods.Quit();
            }
        }

        public static void FindMoviesByDirectorName()
        {
            HelperMethods.PrintHeader("Find movies by director's name");
            var name = HelperMethods.Search("director's full name");
            var result = Program.Service.FindMoviesByDirectorName(name);
            Console.Clear();
            HelperMethods.PrintHeader("Find movies by director's name");
            HelperMethods.PrintHeader($"Results for \"{name}\":");
            foreach (var item in result)
            {
                Console.WriteLine($"{item.Movie.Name} ({item.Movie.Year}) - Director {item.Director.FullName}");
                Console.WriteLine();
            }
            HelperMethods.Quit();
        }

        public static void FindPerformancesByName()
        {
            HelperMethods.PrintHeader("Find performances by name");
            var name = HelperMethods.Search("performance's name");
            var result = Program.Service.FindPerformancesByName(name);
            Console.Clear();
            HelperMethods.PrintHeader("Find performances by name");
            HelperMethods.PrintHeader($"Results for \"{name}\":");
            foreach (var typeWithPerformances in result)
            {
                Console.WriteLine($"{typeWithPerformances.Key.Name}s:");
                var performances = typeWithPerformances.ToList();
                foreach (var performance in performances)
                {
                    Console.Write(performance.Name);
                    if (performance is Movie movie)
                        Console.Write($" ({movie.Year})");
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
            HelperMethods.Quit();
        }

        public static void GetGenresStats()
        {
            var result = Program.Service.GetGenresStats();
            HelperMethods.PrintHeader("Genres' stats:");
            for (int i = 0; i < result.Count(); i++)
            {
                var genre = result.ElementAt(i);
                Console.WriteLine(genre.Genre);
                Console.WriteLine($"Movies: {genre.MoviesQuantity}");
                Console.WriteLine($"Spectacles: {genre.SpectaclesQuantity}");
                Console.WriteLine($"Totally: {genre.TotalQuantity}");
                Console.WriteLine();
            }
            HelperMethods.Quit();
        }

        public static void FindSpectaclesByGenreNameStart()
        {
            HelperMethods.PrintHeader("Find spectacles by genre name start");
            var name = HelperMethods.Search("genre's name");
            var result = Program.Service.FindSpectaclesByGenreNameStart(name);
            Console.Clear();
            HelperMethods.PrintHeader("Find spectacles by genre name start");
            HelperMethods.PrintHeader($"Results for \"{name}\":");
            foreach (var spectacle in result)
            {
                Console.WriteLine($"{spectacle.Name} ({spectacle.Genre})");
                Console.WriteLine();
            }
            HelperMethods.Quit();
        }
    }
}
