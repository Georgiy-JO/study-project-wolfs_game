using Microsoft.VisualBasic;
using Wolf_Island;
using Wolf_Island.Entities;
using static Wolf_Island.Cell;
using Wolf_Island.Entities.Animals.Mobs;

namespace WolfGame;

public enum Direction { dNorth, dSouth, dEast, dWest, dNorthEast, dNorthWest, dSouthEast, dSouthWest }

internal class Game
{
    public enum Direction   // 8
    {
        dNorth = 0, dSouth, dEast, dWest, dNorthEast, dNorthWest, dSouthEast, dSouthWest
    }
    public static (long x, long y) GetCoords(Direction direction, (uint x, uint y) curCoords)
    {
        long x = curCoords.x;
        long y = curCoords.y;
        switch (direction)
        {
            case Direction.dNorth:
                return (x, y - 1);
            case Direction.dSouth:
                return (x, y + 1);
            case Direction.dEast:
                return (x + 1, y);
            case Direction.dWest:
                return (x - 1, y);
            case Direction.dNorthEast:
                return (x + 1, y - 1);
            case Direction.dNorthWest:
                return (x - 1, y - 1);
            case Direction.dSouthEast:
                return (x + 1, y + 1);
            case Direction.dSouthWest:
                return (x - 1, y + 1);
            default:
                return (x, y);
        }
    }
    public struct Data
    {
        private readonly uint _fieldWidth;
        private readonly uint _fieldHeight;
        private readonly List<Wolf> _wolfs;
        private readonly List<Rabbit> _rabbits;
        private readonly List<Food> _food;

        public readonly uint Width => _fieldWidth;
        public readonly uint Height => _fieldHeight;
        public readonly List<Wolf> Wolfs => _wolfs;
        public readonly List<Rabbit> Rabbits => _rabbits;
        public readonly List<Food> Food => _food;

        public Data(uint width, uint height, List<Wolf> wolfs, List<Rabbit> rabbits, List<Food> food)
        {
            _fieldWidth = width;
            _fieldHeight = height;
            _wolfs = wolfs;
            _rabbits = rabbits;
            _food = food;
        }
    }

    protected Data _data;

    public Game(uint fieldWidth = 20, uint fieldHeight = 20, uint countWolfs = 5, uint countRabbit = 5, uint countGrass = 10)
    {
        _data = new Data(fieldWidth, fieldHeight, new List<Wolf>(), new List<Rabbit>(), new List<Food>());

        InitEntities(countWolfs, countRabbit, countGrass);
    }

    public void Tick()
    {
        foreach (var rabbit in _data.Rabbits)
        {
            rabbit.Update(_data);
        }

        foreach (var wolf in _data.Wolfs)
        {
            wolf.Update(_data);
        }

        
        List<(uint x, uint y)> newGrass = new List<(uint x, uint y)>();
        foreach (var grass in _data.Food.OfType<Grass>())       // the check of type is not really needed here, because Remains has update too
        {
            grass.Update(_data, newGrass);
        }
        foreach (var ng in newGrass)
        {
            _data.Food.Add(new Grass(ng));
        }
        

        _data.Rabbits.RemoveAll(r =>
        {
            if (r.IsAlive) return false;
            _data.Food.Add(new Remains(r.Coord, r.MeatValue));
            return true;
        });
        _data.Wolfs.RemoveAll(r =>
        {
            if (r.IsAlive) return false;
            _data.Food.Add(new Remains(r.Coord, r.MeatValue));
            return true;
        });

        //TODO: добавить рождение новых животных (if you feel lucky)
    }

    public GameSnapshot GetSnapshot()
    {
        var cells = new List<Cell>((int)(_data.Width * _data.Height));

        for (uint y = 0; y < _data.Height; y++)
            for (uint x = 0; x < _data.Width; x++)
                cells.Add(new Cell(x, y, CellType.Empty));

        foreach (var g in _data.Food)
            cells[(int)(g.Coord.y * _data.Width + g.Coord.x)] = new Cell((uint)g.Coord.x, (uint)g.Coord.y, CellType.Grass);

        foreach (var r in _data.Rabbits)
            if (r.IsAlive)
                cells[(int)(r.Coord.y * _data.Width + r.Coord.x)] = new Cell((uint)r.Coord.x, (uint)r.Coord.y, CellType.Rabbit);

        foreach (var w in _data.Wolfs)
            if (w.IsAlive)
                cells[(int)(w.Coord.y * _data.Width + w.Coord.x)] = new Cell((uint)w.Coord.x, (uint)w.Coord.y, CellType.Wolf);

        return new GameSnapshot(_data.Width, _data.Height, cells);
    }

    public void InitEntities(uint countWolfs, uint countRabbit, uint countGrass)
    {
        List<(uint x, uint y)> allPositions = CreatePositions(_data.Width, _data.Height);
        Stack<(uint x, uint y)> positions = new(allPositions?.GetShuffledCollections(out var _) ?? Array.Empty<(uint x, uint y)>());

        _data.Wolfs.AddRange(CreateAllWolfs(countWolfs, positions));
        _data.Rabbits.AddRange(CreateAllRabbits(countRabbit, positions));
        _data.Food.AddRange(CreateAllGrass(countGrass, positions));
    }

    protected static List<(uint x, uint y)> CreatePositions(uint fieldWidth, uint fieldHeight)
    {
        var positions = new List<(uint x, uint y)>((int)(fieldWidth * fieldHeight));

        for (uint x = 0; x < fieldWidth; x++)
            for (uint y = 0; y < fieldHeight; y++)
                positions.Add((x, y));

        return positions;
    }

    protected static List<Wolf> CreateAllWolfs(uint countWolfs, Stack<(uint x, uint y)> positions)
    {
        List<Wolf> res = new List<Wolf>();

        for (int i = 0; i < countWolfs; i++)
        {
            if (!positions.TryPop(out var pos))
                throw new InvalidOperationException("The free cages for wolves are empty.!");

            res.Add(new Wolf(pos));
        }

        return res;
    }

    protected static List<Rabbit> CreateAllRabbits(uint countRabbit, Stack<(uint x, uint y)> positions)
    {
        List<Rabbit> res = new List<Rabbit>();

        for (int i = 0; i < countRabbit; i++)
        {
            if (!positions.TryPop(out var pos))
                throw new InvalidOperationException("The free cages for wolves are empty.!");

            res.Add(new Rabbit(pos));
        }

        return res;
    }

    protected static List<Grass> CreateAllGrass(uint countGrass, Stack<(uint x, uint y)> positions)
    {
        List<Grass> res = new List<Grass>();

        for (int i = 0; i < countGrass; i++)
        {
            if (!positions.TryPop(out var pos))
                throw new InvalidOperationException("The free cages for wolves are empty.!");

            res.Add(new Grass(coords: pos));
        }

        return res;
    }
}