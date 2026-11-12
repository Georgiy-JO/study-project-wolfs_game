using Wolf_Island.App.View;
using Wolf_Island.Core.GameSnapshots;

namespace Wolf_Island.App.View.TUI;

    internal sealed class TUIGameRender : IRender
    {
        private readonly struct Cell
        {
            private static readonly Dictionary<string, char> Sybols = new(StringComparer.OrdinalIgnoreCase)
            {
                ["Wolf"] = 'W',
                ["Rabbit"] = 'r',
                ["Grass"] = '"',
                ["Remains"] = 'X',
                ["None"] = '.',
                ["Unknown"] = '?'
            };
            private static readonly Dictionary<char, ConsoleColor> Colors = new()
            {
                ['W'] = ConsoleColor.Red,
                ['r'] = ConsoleColor.White,
                ['"'] = ConsoleColor.Green,
                ['X'] = ConsoleColor.DarkYellow,
                ['.'] = ConsoleColor.Gray,
                ['?'] = ConsoleColor.Magenta
            };
            public static char GetSybol(string id) { return Sybols.TryGetValue(id,out var val)?val: Sybols["Unknows"]; }
            public static ConsoleColor GetColor(string id) { return GetColor(id); }
            public static ConsoleColor GetColor(char symbol) { return Colors.TryGetValue(symbol, out var val)?val: Colors['_']; }
        }

        private readonly char[,] _field;
        private readonly uint _width;
        private readonly uint _height;

        public TUIGameRender(uint width, uint height)
        {
            _width=width;
            _height=height;
            _field = new char[width, height];
            CleanField();
        }

        public void Render(ObjectStyleGameSnapshot snapshot)
        {
             SetField(snapshot);
            PrintGame();

        }
        private void CleanField()
        {
            for (int y = 0; y < _height; y++)
                for (int x = 0; x < _width; x++)
                    _field[x, y] = Cell.GetSybol("None");
        }
        public void PrintGame()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            int fullWidth = (_width + 2 < int.MaxValue) ? (int)_width + 2 : int.MaxValue;
            Console.WriteLine(new string('=', fullWidth));

            for (int y = 0; y < _height; y++)
            {
                Console.Write('|');
                for (int x = 0; x < _width; x++)
                {
                    Console.ForegroundColor = Cell.GetColor(_field[x, y]);
                    Console.Write(_field[x, y]);
                }

                Console.ResetColor();
                Console.WriteLine('|');
            }

            Console.WriteLine(new string('=', fullWidth));
        }
        public void SetField(ObjectStyleGameSnapshot snapshot)
        {
            CleanField();
            foreach(var en in snapshot.Entities)
            {
                if (en.Coord.x < 0 || en.Coord.y < 0 || en.Coord.x >= _width || en.Coord.y >= _height) continue;
                _field[en.Coord.x, en.Coord.y] = Cell.GetSybol(en.Id);
            }
        }
    }
