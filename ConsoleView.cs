using System;
using Wolf_Island.Entities.Animals;
using Wolf_Island.Entities.Objects.FoodKinds;
using static Wolf_Island.Cell;

namespace Wolf_Island
{
    internal sealed class ConsoleView : IRenderer
    {
        public void Render(GameSnapshot snapshot)
        {
            int width = (int)snapshot.Width;
            int height = (int)snapshot.Height;

            char[,] field = new char[height, width];

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    field[y, x] = '.';

            foreach (var f in snapshot.Food)
            {
                int x = (int)f.Coord.x;
                int y = (int)f.Coord.y;
                if (x < 0 || y < 0 || x >= width || y >= height) continue;

                field[y, x] = f switch
                {
                    Grass => '"',
                    Remains => 'x',
                    _ => '?'
                };
            }

            foreach (var a in snapshot.Animals)
            {
                int x = (int)a.Coord.x;
                int y = (int)a.Coord.y;
                if (x < 0 || y < 0 || x >= width || y >= height) continue;

                field[y, x] = a switch
                {
                    Wolf => 'W',
                    Rabbit => 'r',
                    _ => '?'
                };
            }

            Console.SetCursorPosition(0, 0);
            Console.WriteLine(new string('=', width + 2));

            for (int y = 0; y < height; y++)
            {
                Console.Write('|');
                for (int x = 0; x < width; x++)
                {
                    char c = field[y, x];

                    Console.ForegroundColor = c switch
                    {
                        'W' => ConsoleColor.Red,
                        'r' => ConsoleColor.White,
                        '"' => ConsoleColor.Green,
                        'x' => ConsoleColor.DarkYellow,
                        _ => ConsoleColor.DarkGray,
                    };

                    Console.Write(c);
                }

                Console.ResetColor();
                Console.WriteLine('|');
            }

            Console.WriteLine(new string('=', width + 2));
        }
    }
}