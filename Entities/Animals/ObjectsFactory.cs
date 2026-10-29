using System;
using System.Collections.Generic;
using System.Text;
using Wolf_Island.Data;
using Wolf_Island.Entities.Animals.Service;

namespace Wolf_Island.Entities.Animals
{
    internal sealed class ObjectsFactory<TBase, TConfig> : IFactory<TBase, TConfig> where TBase : class where TConfig : IConfig
    {
        private readonly ConfigRegistry _registry;
        private readonly Dictionary<Type, Func<TConfig, (uint x, uint y), TBase?>> _creators = new();

        public ObjectsFactory(ConfigRegistry registry) => _registry = registry;

        public void Register<T>(Func<TConfig, (uint x, uint y), T?> creator) where T : TBase
        {
            Type id = typeof(T);

            if (id is null)
                throw new ArgumentException("Id must not be empty", nameof(id));

            _creators[typeof(T)] = (config, coord) => creator(config, coord);
        }

        public TBase? Create<T>((uint x, uint y) coords) where T : TBase
        {
            Type id = typeof(T);

            var data = _registry.Get<TConfig>(id.Name);

            if (id is null)
                return null;

            if (!_creators.TryGetValue(id, out var creator))
                return null;

            return creator(data, coords);
        }

        public bool CanCreate<T>() => _creators.ContainsKey(typeof(T));
    }
}