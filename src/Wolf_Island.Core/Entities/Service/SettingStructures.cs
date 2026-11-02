using static Wolf_Island.Core.Entities.Animals.Animal;

namespace Wolf_Island.Core.Entities.Service
{
    /// <summary>
    /// General settings support interface.
    /// </summary>
    internal interface IConfig;
    /// <summary>
    /// An Animal settings structure.
    /// </summary>
    /// <param name="maxHealth">how much health it has (health == 0 - death)</param>
    /// <param name="maxFullness">how hungry it can get until it starts loosing health</param>
    /// <param name="meatValue">how much value will its remains Food:Remains has</param>
    /// <param name="fieldOfView">how far on the field can it see</param>
    /// <param name="size">size of the animal (important for the life chain - who eats who)</param>
    /// <param name="hungerTimer">how many moves it can make before becoming hungry</param>
    /// <param name="lifelossTimer">how fast will it lose health if it's hungry</param>
    record struct AnimalData(uint MaxHealth = 1, uint MaxFullness = 1, uint MeatValue = 1, uint FieldOfView = 1, Sizes Size = Sizes.szMedium, uint HungerTimer = 2, uint LifelossTimer = 2, uint MoveDistance = 1) : IConfig;
    /// <summary>
    /// A food settings structure.
    /// </summary>
    /// <param name="Value"> Value of the food.</param>
    /// <param name="ChangeTimer">How fast will it grow, dissapear</param>
    /// <param name="CurChangeTimer">Current timer.</param>
    record struct FoodData(uint Value = 0, uint ChangeTimer = 4, uint CurChangeTimer = 4) : IConfig;
    /// <summary>
    /// A food settings structure.
    /// </summary>
    /// <param name="Value"> Value of the food.</param>
    /// <param name="ChangeTimer">How fast will it grow, dissapear</param>
    record struct FoodDataShort(uint Value = 0, uint ChangeTimer = 4, uint CurChangeTimer = 4) : IConfig;
}
