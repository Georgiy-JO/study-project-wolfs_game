using Microsoft.VisualBasic;
using Wolf_Island.Entities.Animals;
using Wolf_Island.Entities.Animals.Service;
using Wolf_Island.Entities.Objects;
using Wolf_Island.Entities.Objects.FoodKinds;
using Wolf_Island.Extensions;
using static Wolf_Island.Cell;



namespace Wolf_Island;

public class Game
{
    public enum Direction
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
        private readonly List<Animal> _animals;
        private readonly List<Food> _food;

        public readonly uint Width => _fieldWidth;
        public readonly uint Height => _fieldHeight;
        public readonly List<Animal> Animals => _animals;
        public readonly List<Food> Food => _food;

        public Data(uint width, uint height, List<Animal> animals, List<Food> food)
        {
            _fieldWidth = width;
            _fieldHeight = height;
            _animals = animals;
            _food = food;
        }
    }

    protected Data _data;
    private readonly IFactory<Animal, AnimalData> _factory;

    public Game(IFactory<Animal, AnimalData> factory, uint fieldWidth = 20, uint fieldHeight = 20, uint countWolfs = 5, uint countRabbit = 5, uint countGrass = 10)
    {
        _factory = factory;
        _data = new Data(fieldWidth, fieldHeight, new List<Animal>(), new List<Food>());

        InitEntities(countWolfs, countRabbit, countGrass);
    }

    public void Tick()
    {
        foreach (var anim in _data.Animals)
        {
            if (anim.IsAlive) anim.Update(_data);
        }

        //foreach (var corn in _data.Carnivores)
        //{
        //    if (corn.IsAlive) corn.Update(_data);
        //}

        List<(uint x, uint y)> newGrass = new List<(uint x, uint y)>();

        foreach (var grass in _data.Food)
        {
            grass.Update(_data, newGrass);
        }

        foreach (var ng in newGrass)
        {
            _data.Food.Add(new Grass(ng));
        }


        _data.Animals.RemoveAll(a =>
        {
            if (a.IsAlive) return false;
            if (a.MeatValue != 0) _data.Food.Add(new Remains(a.Coord, a.MeatValue));
            return true;
        });
        //_data.Carnivores.RemoveAll(r =>
        //{
        //    if (r.IsAlive) return false;
        //    _data.Food.Add(new Remains(r.Coord, r.MeatValue));
        //    return true;
        //});

        //TODO: добавить рождение новых животных (if you feel lucky)
    }

    public GameSnapshot GetSnapshot()
    {
        var animals = new List<Animal>(_data.Animals.Count);
        foreach (var a in _data.Animals)
            if (a.IsAlive)
                animals.Add(a);

        var food = new List<Food>(_data.Food.Count);
        foreach (var f in _data.Food)
            food.Add(f);

        return new GameSnapshot(_data.Width, _data.Height, animals, food);
    }

    protected void InitEntities(uint countWolfs, uint countRabbit, uint countGrass)
    {
        Stack<(uint x, uint y)> positions = new(CreatePositions(_data.Width, _data.Height)?.GetShuffledCollections(out var _) ?? Array.Empty<(uint x, uint y)>());

        _data.Animals.AddRange(SpawnEnimals<Wolf>(countWolfs, positions));
        _data.Animals.AddRange(SpawnEnimals<Rabbit>(countRabbit, positions));
        _data.Food.AddRange(SpawnGrass(countGrass, positions));
    }

    protected static List<(uint x, uint y)> CreatePositions(uint fieldWidth, uint fieldHeight)
    {
        var positions = new List<(uint x, uint y)>((int)(fieldWidth * fieldHeight));

        for (uint x = 0; x < fieldWidth; x++)
            for (uint y = 0; y < fieldHeight; y++)
                positions.Add((x, y));

        return positions;
    }

    protected IEnumerable<Animal> SpawnEnimals<TAnimal>(uint count, Stack<(uint x, uint y)> positions) where TAnimal : Animal
    {
        for (uint i = 0; i < count; i++)
        {
            if (!positions.TryPop(out var pos))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No more free cells!!!");

                yield break;
            }

            yield return _factory.Create<TAnimal>(pos) ?? throw new ArgumentNullException("Animal is null", nameof(TAnimal));
        }
    }

    protected static List<Grass> SpawnGrass(uint countGrass, Stack<(uint x, uint y)> positions)
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