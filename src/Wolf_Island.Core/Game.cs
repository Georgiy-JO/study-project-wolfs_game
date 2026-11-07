using Microsoft.VisualBasic;
using Microsoft.Win32;
using System.Reflection;
using Wolf_Island.Core.DataLoader;
using Wolf_Island.Core.Entities;
using Wolf_Island.Core.Entities.Animals;
using Wolf_Island.Core.Entities.Objects;
using Wolf_Island.Core.Entities.Service;
using Wolf_Island.Core.Extensions;
using Wolf_Island.Core.GameSnapshots;
using static Wolf_Island.Game;



namespace Wolf_Island;

internal class Game
{
    public enum Direction
    {
        dNorth = 0, dSouth, dEast, dWest, dNorthEast, dNorthWest, dSouthEast, dSouthWest
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
    private readonly EntityFactory _factory;


    public Game(uint fieldWidth = 20, uint fieldHeight = 20, string settingJsonlocation ="Configs/")
    {
        _data = new Data(fieldWidth, fieldHeight, new List<Animal>(), new List<Food>());

        var animalJson = settingJsonlocation + "animals.json";
        var foodJson = settingJsonlocation + "food.json";
        if (!File.Exists(animalJson))
            throw new FileNotFoundException($"File not found: {animalJson}");
        if (!File.Exists(foodJson))
            throw new FileNotFoundException($"File not found: {foodJson}");
        _factory = new EntityFactory(Loader.GetFullRegistry(animalJson, foodJson));
    }
    public Dictionary<string, uint> GetEntitiesRegistry()
    {
        Dictionary<string, uint> registry = new();
        var animalList = _factory.GetPossibleIDs();
        foreach (var id in animalList)
            registry[id] = 0;
        return registry;
    }
    public void StartGame(Dictionary<string, uint> idAmoutRegistry)
    {
        InitEntities(idAmoutRegistry);
    }
    public void StopGame()
    {
        _data.Animals.Clear();
        _data.Food.Clear();
    }
    public void RestartGame(Dictionary<string, uint> idAmoutRegistry)
    {
        StopGame();
        StartGame(idAmoutRegistry);
    }

    public ObjectStyleGameSnapshot GetObjectStyleGameSnapshot()
    {
        var entitiesList = _data.Animals
            .Select(a => new ObjectStyleGameSnapshot.EntitySnapshot(a.GetType().Name, a.Coord))
            .Concat(_data.Food.Select(f => new ObjectStyleGameSnapshot.EntitySnapshot(f.GetType().Name, f.Coord)))
            .ToList();

        return new ObjectStyleGameSnapshot(_data.Width,_data.Height, entitiesList);
    }
    public ListStyleGameSnapshot GetListStyleGameSnapshot()
    {
        var registry = new Dictionary<string, List<(uint X, uint Y)>>(StringComparer.OrdinalIgnoreCase);
        void Add(Entity entity)
        {
            string key = entity.GetType().Name;
            if (!registry.TryGetValue(key, out var list))
            {
                list = new List<(uint, uint)>();
                registry[key] = list;
            }
            list.Add(entity.Coord);
        }

        foreach (var animal in _data.Animals) Add(animal);
        foreach (var food in _data.Food) Add(food);

        return new ListStyleGameSnapshot(_data.Width, _data.Height, registry);          //issue?
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
    public void Tick()
    {
        foreach (var anim in _data.Animals)
        {
            if (anim.IsAlive) anim.Update(_data);
        }

        List<(uint x, uint y)> newGrass = new List<(uint x, uint y)>();

        foreach (var grass in _data.Food)
        {
            grass.Update(_data, newGrass);
        }

        foreach (var ng in newGrass)
        {
            _data.Food.Add(_factory.CreateFood("Grass",ng,null,null,true));          // works?
        }


        _data.Animals.RemoveAll(a =>
        {
            if (a.IsAlive) return false;
            if (a.MeatValue != 0) _data.Food.Add(_factory.CreateFood("Remains",a.Coord,null,a.MeatValue,true));
            return true;
        });
        //TODO: добавить рождение новых животных (if you feel lucky)
    }
    protected void InitEntities(Dictionary<string, uint> idAmoutRegistry)
    {
        Stack<(uint x, uint y)> positions = new(CreatePositions(_data.Width, _data.Height)?.GetShuffledCollections() ?? Array.Empty<(uint x, uint y)>());

        var animalList = _factory.GetPossibleIDs();
        foreach (var id in idAmoutRegistry)
        {
            for (uint i = 0; i < id.Value; i++)
            {
                if (positions.Count == 0)       break;          // can throw here if we are intereste in to allowing to fill the field complitely 
                var entity = _factory.Create(id.Key, positions.Pop());
                switch (entity)
                {
                    case Animal animal:
                        _data.Animals.Add(animal);
                        break;
                    case Food food:
                        _data.Food.Add(food);
                        break;
                    default: break;
                }
            }
        }
               
    }

    protected static List<(uint x, uint y)> CreatePositions(uint fieldWidth, uint fieldHeight)
    {
        uint positionsAmount = fieldWidth * fieldHeight;
        if(positionsAmount> int.MaxValue)   throw new ArgumentOutOfRangeException("Field is too large for List capacity");
        var positions = new List<(uint x, uint y)>((int)positionsAmount);
        if(fieldWidth>int.MaxValue)
        for (uint x = 0; x < fieldWidth; x++)
            for (uint y = 0; y < fieldHeight; y++)
                positions.Add((x, y));

        return positions;
    }
}