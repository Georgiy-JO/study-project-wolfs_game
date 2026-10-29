using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Wolf_Island.Entities.Animals;
using Wolf_Island.Entities.Objects;

namespace Wolf_Island
{
    public interface IRenderer
    {
        void Render(GameSnapshot snapshot);
    }

    public sealed class GameSnapshot
    {
        public uint Width { get; }
        public uint Height { get; }
        public IReadOnlyList<Animal> Animals { get; }
        public IReadOnlyList<Food> Food { get; }

        public GameSnapshot(
            uint width,
            uint height,
            IReadOnlyList<Animal> animals,
            IReadOnlyList<Food> food)
        {
            Width = width;
            Height = height;
            Animals = animals;
            Food = food;
        }
    }

    public struct Cell
    {
        public enum CellType { Empty, Grass, Rabbit, Wolf, Remains }

        private uint _x, _y;
        private CellType _type;

        public uint X => _x;
        public uint Y => _y;
        public CellType Type => _type;

        public Cell(uint x, uint y, CellType type)
        {
            _x = x;
            _y = y;
            _type = type;
        }
    }
}