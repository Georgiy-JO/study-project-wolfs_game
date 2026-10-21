using Microsoft.VisualBasic;
using Wolf_Island;
using static Wolf_Island.Cell;

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

        public uint Width => _fieldWidth;
        public uint Height => _fieldHeight;
        public List<Wolf> Wolfs => _wolfs;
        public List<Rabbit> Rabbits => _rabbits;
        public List<Grass> Grass => _grass;

        public Data(uint width, uint height, List<Wolf> wolfs, List<Rabbit> rabbits, List<Grass> grass)
        {
            _fieldWidth = width;
            _fieldHeight = height;
            _wolfs = wolfs;
            _rabbits = rabbits;
            _grass = grass;
        }
    }

    protected Data _data;

    public Game(uint fieldWidth = 20, uint fieldHeight = 20, uint countWolfs = 5, uint countRabbit = 5, uint countGrass = 10)
    {
        _data = new Data(fieldWidth, fieldHeight, new List<Wolf>(), new List<Rabbit>(), new List<Grass>());

        InitEntities(countWolfs, countRabbit, countGrass);
    }

    public void Tick()
    {
        foreach (var rabbit in _data.Rabbits)
        {
            rabbit.Update();
        }

        foreach (var wolf in _data.Wolfs)
        {
            wolf.Update();
        }

        foreach (var grass in _data.Grass)
        {
            grass.Update();
        }

        _data.Rabbits.RemoveAll((r) => !r.IsAlive);
        _data.Wolfs.RemoveAll((r) => !r.IsAlive);

        //TODO: добавить удаление съеденой травы, и добавления новых животных и травы, это делать ни в коем случае не в нутри самих циклов!!!
    }

    public GameSnapshot GetSnapshot()
    {
        var cells = new List<Cell>((int)(_data.Width * _data.Height));

        for (uint y = 0; y < _data.Height; y++)
            for (uint x = 0; x < _data.Width; x++)
                cells.Add(new Cell(x, y, CellType.Empty));

        foreach (var g in _data.Grass)
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
        _data.Rabbits.AddRange(CreateAllRabit(countRabbit, positions));
        _data.Grass.AddRange(CreateAllGrass(countGrass, positions));
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

            res.Add(new Wolf(pos));
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

            res.Add(new Rabbit(pos));
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

            res.Add(new Grass(coord: pos));
        }

        return res;
    }
}