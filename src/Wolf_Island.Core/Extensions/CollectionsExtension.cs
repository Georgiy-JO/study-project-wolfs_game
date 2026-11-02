using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Wolf_Island.Core.Extensions;

internal static class CollectionsExtension
{
    private static readonly Random Rnd = new Random();

    public static T[] GetShuffledCollections<T>(this IEnumerable<T> collection)
    {
        ArgumentNullException.ThrowIfNull(collection);

        T[] arrTemp = collection.ToArray();

        Random.Shared.Shuffle(arrTemp);  // only with .NET 8+

        return arrTemp;
    }
}