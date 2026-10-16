using Wolf_Island;
using static WolfGame.Game;
using Timer = System.Timers.Timer;

namespace WolfGame;

public enum Direction { dNorth, dSouth, dEast, dWest, dNorthEast, dNorthWest, dSouthEast, dSouthWest }

internal class Game
{
    public struct Data
    {
        private readonly uint _fieldWidth;
        private readonly uint _fieldHeight;
        private readonly List<Wolf> _wolfs;
        private readonly List<Rabbit> _rabbits;
        private readonly List<Grass> _grass;

        public uint FieldWidth => _fieldWidth;
        public uint FieldHeight => _fieldHeight;
        public List<Wolf> Wolfs => _wolfs;
        public List<Rabbit> Rabbits => _rabbits;
        public List<Grass> Grass => _grass;

        public Data(uint fieldWidth, uint fieldHeight, List<Wolf> wolfs, List<Rabbit> rabbits, List<Grass> grass)
        {
            _fieldWidth = fieldWidth;
            _fieldHeight = fieldHeight;
            _wolfs = wolfs;
            _rabbits = rabbits;
            _grass = grass;
        }
    }

    protected Data _data;

    public Game(uint fieldWidth = 20, uint fieldHeight = 20, uint countWolfs = 5,  uint countRabbit = 5, uint countGrass = 10, uint frames = 2000)
    {
        List<(uint x, uint y)>  allPositions = CreatePositions(fieldWidth, fieldHeight);

        _data = new Data(fieldWidth, fieldHeight, new List<Wolf>(), new List<Rabbit>(), new List<Grass>());

        Stack<(uint x, uint y)> positions = new(allPositions?.GetShuffledCollections(out var _) ?? Array.Empty<(uint x, uint y)>());

        _data.Wolfs.AddRange(CreateAllWolfs(countWolfs, positions));
        _data.Grass.AddRange(CreateAllGrass(countRabbit, positions));
        _data.Rabbits.AddRange(CreateAllRabit(countGrass, positions));

        StartPlay(frames);
    }

    public virtual void StartPlay(uint frames)
    {
        while (true)
        {
            Update();

            Thread.Sleep((int)frames);
        }
    }

    public virtual void DrawGraphics()
    {
        char[,] field = new char[_data.FieldHeight, _data.FieldWidth];

        for (int y = 0; y < _data.FieldHeight; y++)
            for (int x = 0; x < _data.FieldWidth; x++)
                field[y, x] = '.';

        foreach (Grass g in _data.Grass)
            field[(int)g.Coord.y, (int)g.Coord.x] = '"';

        foreach (Rabbit r in _data.Rabbits)
            if (r.IsAlive)
                field[(int)r.Coord.y, (int)r.Coord.x] = 'r';

        foreach (Wolf w in _data.Wolfs)
            if (w.IsAlive)
                field[(int)w.Coord.y, (int)w.Coord.x] = 'W';

        Console.SetCursorPosition(0, 0);
        Console.WriteLine(new string('=', (int)_data.FieldWidth + 2));

        for (int y = 0; y < _data.FieldHeight; y++)
        {
            Console.Write('|');
            for (int x = 0; x < _data.FieldWidth; x++)
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

        Console.WriteLine(new string('=', (int)_data.FieldWidth + 2));
    }

    protected List<(uint x, uint y)> CreatePositions(uint fieldWidth, uint fieldHeight)
    {
        var positions = new List<(uint x, uint y)>((int)(fieldWidth * fieldHeight));

        for (uint x = 0; x < fieldWidth; x++)
            for (uint y = 0; y < fieldHeight; y++)
                positions.Add((x, y));

        return positions;
    }

    protected List<Wolf> CreateAllWolfs(uint countWolfs, Stack<(uint x, uint y)> positions)
    {
        List<Wolf> res = new List<Wolf>();

        for (int i = 0; i < countWolfs; i++)
        {
            if (!positions.TryPop(out var pos))
                throw new InvalidOperationException("The free cages for wolves are empty.!");

            //TODO: волку передавать pos
            res.Add(new Wolf());
        }

        return res;
    }

    protected List<Rabbit> CreateAllRabit(uint countRabbit, Stack<(uint x, uint y)> positions)
    {
        List<Rabbit> res = new List<Rabbit>();

        for (int i = 0; i < countRabbit; i++)
        {
            if (!positions.TryPop(out var pos))
                throw new InvalidOperationException("The free cages for wolves are empty.!");

            //TODO: кролю передавать pos
            res.Add(new Rabbit());
        }

        return res;
    }

    protected List<Grass> CreateAllGrass(uint countGrass, Stack<(uint x, uint y)> positions)
    {
        List<Grass> res = new List<Grass>();

        for (int i = 0; i < countGrass; i++)
        {
            if (!positions.TryPop(out var pos))
                throw new InvalidOperationException("The free cages for wolves are empty.!");

            //TODO: траве передавать pos
            res.Add(new Grass());
        }

        return res;
    }

    protected void Update()
    {
        foreach (var item in _data.Rabbits)
        {
            //TODO: вызвать Update логики кролей
        }

        foreach (var item in _data.Wolfs)
        {
            //TODO: вызвать Update логики волков
        }

        foreach (var item in _data.Grass)
        {
            //TODO: вызвать Update логики травы
        }

        //TODO: после всех циклов ОБЯЗАТЕЛЬНО сделать удаления всех мертвых и съединых животных и травы, и добавления новых, это делать ни в коем случае не в нутри самих циклов!!!

        DrawGraphics();
    }
}