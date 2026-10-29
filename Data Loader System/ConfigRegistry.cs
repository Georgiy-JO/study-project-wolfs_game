using System;
using System.Collections.Generic;
using System.Text;
using Wolf_Island.Entities.Animals;
using Wolf_Island.Entities.Animals.Service;

namespace Wolf_Island.Data
{
    internal sealed class ConfigRegistry
    {
        private readonly Dictionary<string, IConfig> _byId = new();

        public static ConfigRegistry FromDtos<TDto, TConfig>(IEnumerable<TDto> dtos, Func<TDto, TConfig> createConfig) where TConfig : IConfig where TDto : IDto
        {
            var registry = new ConfigRegistry();

            foreach (var dto in dtos)
            {
                var config = createConfig(dto);
                if (string.IsNullOrWhiteSpace(dto.Id))
                    throw new InvalidDataException("Config has empty id");

                registry._byId[dto.Id] = config;
            }

            //foreach (var dto in dtos)
            //{
            //    if (string.IsNullOrWhiteSpace(dto.Id))
            //        throw new InvalidDataException("Animal config has empty id");

            //    if (!Enum.TryParse<Animal.Sizes>(dto.Size, ignoreCase: true, out var size))
            //        throw new InvalidDataException($"Unknown size '{dto.Size}' for animal '{dto.Id}'");

            //    var data = new AnimalData(dto.MaxHealth, dto.MaxFullness, dto.MeatValue, dto.FieldOfView, size, dto.HungerTimer, dto.LifelossTimer);

            //    registry._byId[dto.Id] = data;
            //}

            return registry;
        }

        public T Get<T>(string id) where T : IConfig
        {
            if (!_byId.TryGetValue(id, out var config))
                throw new KeyNotFoundException($"Animal config '{id}' not found");

            if (config is not T typedConfig)
                throw new InvalidCastException($"Config '{id}' имеет тип {config.GetType().Name}, но запрашивался тип {typeof(T).Name}. Проверьте соответствие ID и типа.");

            return typedConfig;
        }

        public bool TryGet<T>(string id, out T? config) where T : IConfig
        {
            config = default;

            if (!_byId.TryGetValue(id, out var value))
                return false;

            if (value is T typed)
            {
                config = typed;

                return true;
            }

            return false;
        }
    }
}