using Wolf_Island.Core.Entities.Animals;
using Wolf_Island.Core.Entities.Objects;

namespace Wolf_Island.Core.Entities.Service
{
    internal sealed class EntityFactory
    {
        private readonly Dictionary<string, IConfig> _registry;
        private readonly Dictionary<string, Type> _types;

        public EntityFactory(Dictionary<string, IConfig> registry)
        {
            _registry = registry;
            //_types = GetTypes<Entity, IConfig>();   // can't be used because if constructor check in GetTypes
            _types = GetTypes<Food, FoodData>();
            var foodTypes = GetTypes<Animal, AnimalData>();
            foreach (var foodType in foodTypes)
                _types[foodType.Key] = foodType.Value;
        }
        private static Dictionary<string, Type> GetTypes<TBase, TData>()
            where TBase : class
            where TData : class
        {
            var types = typeof(TBase).Assembly.GetTypes()
                .Where(t => t.IsClass
                         && !t.IsAbstract
                         && t.IsSealed
                         && typeof(TBase).IsAssignableFrom(t));

            var dict = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

            foreach (var type in types)
            {
                var constructor = type.GetConstructor(new[] { typeof(TData), typeof((uint, uint)) });

                //if (constructor is null)
                //    throw new InvalidOperationException(
                //        $"{type.Name} must have constructor ({typeof(TData).Name}, (uint, uint))");
                if(constructor is not null)
                    dict[type.Name] = type;
            }

            return dict;
        }

        public List<string> GetPossibleIDs()
        {
            List<string> idsList = new();
            foreach (var id in _types)
                idsList.Add(id.Key);

            return idsList;
        }
        public List<Type> GetPossibleTypes()
        {
            List<Type> typesList = new();
            foreach (var id in _types)
                typesList.Add(id.Value);

            return typesList;
        }

        public Entity? Create(string id, (uint x, uint y) coords)
        {
            if (!_types.ContainsKey(id)) return null; // can be rewritten to throw

            if (_types.TryGetValue(id, out Type? type) && typeof(Animal).IsAssignableFrom(type))        // type can't be null here
                return CreateAnimal(id, coords);
            else if (type is not null && typeof(Food).IsAssignableFrom(type))
                return CreateFood(id, coords, null, null);
            else
                return null;
        }

        public Animal CreateAnimal(string id, (uint x, uint y) coords)
        {
            if (id is null || !_registry.TryGetValue(id, out var conf) || conf is not AnimalData data)
                throw new KeyNotFoundException($"Animal config '{id}' not found");
            if (!_types.TryGetValue(id, out var type))
                throw new NotSupportedException($"Unknown animal type: {id}");
            return (Animal)(Activator.CreateInstance(type, data, coords) ?? throw new Exception("CreateInstance somehow failed."));
        }
        public Food CreateFood(string id, (uint x, uint y) coords, uint? curChangeTimer, uint? value, bool setTimerMax = false)
        {
            if (id is null || !_registry.TryGetValue(id, out var conf) || conf is not FoodData data)
                throw new KeyNotFoundException($"Food config '{id}' not found");
            if (!_types.TryGetValue(id, out var type))
                throw new NotSupportedException($"Unknown food type: {id}");
            if (!setTimerMax)
            {
                if (curChangeTimer != null) data.CurChangeTimer = (uint)curChangeTimer;
                else
                {
                    var rand = new Random();
                    data.CurChangeTimer = (uint)rand.Next((data.ChangeTimer + 1 < int.MaxValue) ? (int)(data.ChangeTimer + 1) : int.MaxValue);
                }
            }
            if (value != null) data.Value = (uint)value;
            return (Food)(Activator.CreateInstance(type, data, coords) ?? throw new Exception("CreateInstance somehow failed."));
        }
    }
}