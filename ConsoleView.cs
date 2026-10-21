using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;
using WolfGame;
using static Wolf_Island.Cell;

namespace Wolf_Island
{
    internal sealed class ConsoleView : IRenderer
    {
        public void Render(GameSnapshot snapshot)
        {
            char[,] field = new char[snapshot.Height, snapshot.Width];

            for (int y = 0; y < snapshot.Height; y++)
                for (int x = 0; x < snapshot.Width; x++)
                    field[y, x] = '.';

            foreach (var cell in snapshot.Cells)
            {
                field[cell.Y, cell.X] = cell.Type switch
                {
                    CellType.Wolf => 'W',
                    CellType.Rabbit => 'r',
                    CellType.Grass => '"',
                    _ => '.'
                };
            }

            Console.SetCursorPosition(0, 0);
            Console.WriteLine(new string('=', (int)(snapshot.Width + 2)));

            for (int y = 0; y < snapshot.Height; y++)
            {
                Console.Write('|');
                for (int x = 0; x < snapshot.Width; x++)
                {
                    char c = field[y, x];
                    Console.ForegroundColor = c switch
                    {
                        'W' => ConsoleColor.Red,
                        'r' => ConsoleColor.White,
                        '"' => ConsoleColor.Green,
                        _ => ConsoleColor.DarkGray,
                    };
                    Console.Write(c);
                }
                Console.ResetColor();
                Console.WriteLine('|');
            }

            Console.WriteLine(new string('=', (int)(snapshot.Width + 2)));
        }
    }
}