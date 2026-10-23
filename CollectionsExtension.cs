using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static WolfGame.Game;

namespace Wolf_Island;

internal static class CollectionsExtension
{
    private static readonly Random Rnd = new Random();

    public static IEnumerable<T> GetShuffledCollections<T>(this IEnumerable<T> collection, out IEnumerable<T> outPut)
    {

        if (collection == null)
            throw new ArgumentNullException("The collection must not be NULL");

        T[] arrTemp = collection.ToArray();

        for (int i = collection.Count() - 1; i > 0; i--)
        {
            int j = Rnd.Next(i + 1);
            (arrTemp[i], arrTemp[j]) = (arrTemp[j], arrTemp[i]);
        }

        outPut = arrTemp;

        return arrTemp;
    }
    public static T[] GetShuffledCollections<T>(this IEnumerable<T> collection)
    {
        ArgumentNullException.ThrowIfNull(collection);
        
        T[] arrTemp = collection.ToArray();

        Random.Shared.Shuffle(arrTemp);  // only with .NET 8+

        return arrTemp;
    }

}