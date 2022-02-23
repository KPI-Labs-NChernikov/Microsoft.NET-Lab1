using System;

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
    }
}
