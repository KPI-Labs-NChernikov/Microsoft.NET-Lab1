using System;
using System.Diagnostics;

namespace ConsoleApp
{
    public static class HelperMethods
    {
        public static void Quit()
        {
            Console.WriteLine("Press enter co continue");
            Console.ReadLine();
        }

        public static void PrintHeader(string header)
        {
            Console.WriteLine($"{header}\n");
        }

        public static string Search(string toFind)
        {
            Console.WriteLine($"Please, enter the {toFind}: ");
            return Console.ReadLine();
        }
    }
}
