using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Wolf_Island.Data
{
    public interface IDto
    {
        public string Id { get; set; }
    }

    public sealed class AnimalConfigDTO : IDto
    {
        public string Id { get; set; } = "";
        public string Type { get; set; } = "";
        public uint MaxHealth { get; set; }
        public uint MaxFullness { get; set; }
        public uint MeatValue { get; set; }
        public uint FieldOfView { get; set; }
        public string Size { get; set; } = "Medium";
        public uint HungerTimer { get; set; }
        public uint LifelossTimer { get; set; }
        public uint MoveDistance { get; set; }
    }

    public sealed class AnimalsListDTO
    {
        public List<AnimalConfigDTO> Animals { get; set; } = new();
    }
}