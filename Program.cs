using Microsoft.Win32;
using System.Runtime.CompilerServices;
using Wolf_Island.Data;
using Wolf_Island.Entities.Animals;
using Wolf_Island.Entities.Animals.Service;
using static Wolf_Island.Game;

namespace Wolf_Island
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Data", "animals.json");

            var dtos = AnimalConfigLoader.Load(path);
            var registry = ConfigRegistry.FromDtos(dtos, (dto) =>
            {
                if (!Enum.TryParse<Animal.Sizes>(dto.Size, ignoreCase: true, out var size))
                    throw new InvalidDataException($"Unknown size '{dto.Size}' for animal '{dto.Id}'");

                return new AnimalData(dto.MaxHealth, dto.MaxFullness, dto.MeatValue, dto.FieldOfView, size, dto.HungerTimer, dto.LifelossTimer);
            });

            IFactory<Animal, AnimalData> factory = new ObjectsFactory<Animal, AnimalData>(registry);

            RegisterAnimals(factory);

            Game game = new Game(factory);

            StartPlay(2000, game);
        }

        static void RegisterAnimals(IFactory<Animal, AnimalData> factory)
        {
            factory.Register((d, p) => new Wolf(d, p));
            factory.Register((d, p) => new Rabbit(d, p));
        }

        static void StartPlay(uint frames, Game game)
        {
            IRenderer view = new ConsoleView();

            while (true)
            {
                game.Tick();
                view.Render(game.GetSnapshot());

                Thread.Sleep((int)frames);
            }
        }
    }
}