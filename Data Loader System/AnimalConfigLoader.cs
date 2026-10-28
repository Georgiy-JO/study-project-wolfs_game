using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Wolf_Island.Data
{
    internal static class AnimalConfigLoader
    {
        public static List<AnimalConfigDTO> LoadWithPath(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"Animal config not found: {path}");

            string json = File.ReadAllText(path);
            var root = JsonSerializer.Deserialize<AnimalsListDTO>(json);

            if (root == null || root.Animals.Count == 0)
                throw new InvalidDataException($"Animal config is empty: {path}");

            return root.Animals;
        }

        public static List<AnimalConfigDTO> LoadFromFile(string file)
        {
            if (string.IsNullOrEmpty(file))
                throw new FileNotFoundException($"Animal found are empty or null", nameof(file));

            var root = JsonSerializer.Deserialize<AnimalsListDTO>(file);

            if (root == null || root.Animals.Count == 0)
                throw new InvalidDataException($"Animal config is empty: {file}");

            return root.Animals;
        }
    }
}