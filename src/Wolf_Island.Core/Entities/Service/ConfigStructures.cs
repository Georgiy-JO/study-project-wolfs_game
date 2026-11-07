using static Wolf_Island.Core.Entities.Animals.Animal;

namespace Wolf_Island.Core.Entities.Service
{
    /// <summary>
    /// General settings support interface.
    /// </summary>
    internal interface IConfig
    {
        public string Id { get; set; }
    }
    /// <summary>
    /// An Animal settings structure.
    /// </summary>
    /// <param name="id">The Id (used to determine the class).</param>
    /// <param name="maxHealth">how much health it has (health == 0 - death).</param>
    /// <param name="maxFullness">how hungry it can get until it starts loosing health.</param>
    /// <param name="meatValue">how much value will its remains Food:Remains has.</param>
    /// <param name="fieldOfView">how far on the field can it see.</param>
    /// <param name="size">size of the animal (important for the life chain - who eats who).</param>
    /// <param name="hungerTimer">how many moves it can make before becoming hungry.</param>
    /// <param name="lifelossTimer">how fast will it lose health if it's hungry.</param>
    /// <param name="moveDistance">how far will it be able to move.</param>
    internal sealed class AnimalData(string id = "", uint maxHealth = 1, uint maxFullness = 1,
                                     uint meatValue = 1, uint fieldOfView = 1, Sizes size = Sizes.szMedium,
                                     uint hungerTimer = 2, uint lifelossTimer = 2, uint moveDistance = 1) : IConfig
    {
        public string Id { get; set; } = id;
        public uint MaxHealth { get; set; } = maxHealth;
        public uint MaxFullness { get; set; } = maxFullness;
        public uint MeatValue { get; set; } = meatValue;
        public uint FieldOfView { get; set; } = fieldOfView;
        public Sizes Size { get; set; } = size;
        public uint HungerTimer { get; set; } = hungerTimer;
        public uint LifelossTimer { get; set; } = lifelossTimer;
        public uint MoveDistance { get; set; } = moveDistance;
    }
    internal sealed class AnimalsList
    {
        public List<AnimalData> Animals { get; set; } = new();
    }
    /// <summary>
    /// A food settings structure.
    /// </summary>
    /// <param name="id">The Id (used to determin the class).</param>
    /// <param name="value"> Value of the food.</param>
    /// <param name="changeTimer">How fast will it grow, dissapear</param>
    /// <param name="curChangeTimer">Current timer.</param>
    internal sealed class FoodData(string id = "", uint value = 0, uint changeTimer = 4, uint curChangeTimer = 4) : IConfig
    {
        public string Id { get; set; } = id;
        public uint Value { get; set; } = value;
        public uint ChangeTimer { get; set; } = changeTimer;
        public uint CurChangeTimer { get; set; } = curChangeTimer;
    }
    internal sealed class FoodList
    {
        public List<FoodData> Food { get; set; } = new();
    }
}
