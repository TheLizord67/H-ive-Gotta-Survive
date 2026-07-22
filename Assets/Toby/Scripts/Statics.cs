using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Statics
{
    public static System.Random randyTheRandom = new();
    public static void Move<T>(this List<T> list, int oldIndex, int newIndex)
    {
        T item = list[oldIndex];
        list.RemoveAt(oldIndex);
        list.Insert(newIndex, item);
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> list)
    {
        List<T> shuffledList = list.OrderBy(x => randyTheRandom.Next()).ToList();
        return shuffledList;
    }
}
