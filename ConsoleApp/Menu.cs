using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp
{
    public class Menu
    {
        public string Header { get; set; }

        public string Name { get; set; }

        public IEnumerable<(string Text, Action Action)> Items { get; set; }

        public void Print()
        {
            if (Items is not null && Items.Any())
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                if (Header is not null)
                {
                    Console.WriteLine(Header);
                    Console.WriteLine();
                }
                var name = !string.IsNullOrEmpty(Name) ? Name : "item";
                Console.WriteLine($"Please, select the {name}:");
                var texts = Items.Select(i => i.Text);
                for (int i = 0; i < texts.Count(); i++)
                    Console.WriteLine($"{i+1}. {texts.ElementAt(i)}");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("0. Quit\n");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"Select the number from 0 to {Items.Count()}: ");
                bool parsed;
                parsed = int.TryParse(Console.ReadLine(), out int selected);
                while (!parsed || selected < 0 || selected > Items.Count())
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Error: wrong number");
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine($"Please, select the number from 0 to {Items.Count()} once more");
                    parsed = int.TryParse(Console.ReadLine(), out selected);
                }
                if (selected != 0)
                {
                    selected--;
                    Console.Clear();
                    Items.ElementAt(selected).Action?.Invoke();
                    Print();
                }
            }
            Console.ResetColor();
        }
    }
}
