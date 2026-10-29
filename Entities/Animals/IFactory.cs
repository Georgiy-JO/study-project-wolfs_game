using System;
using System.Collections.Generic;
using System.Text;

namespace Wolf_Island.Entities.Animals
{
    internal interface IFactory<TBase, TConfig> where TBase : class where TConfig : IConfig
    {
        void Register<T>(Func<TConfig, (uint x, uint y), T?> creator) where T : TBase;

        public TBase? Create<T>((uint x, uint y) coords) where T : TBase;

        public bool CanCreate<T>();
    }
}