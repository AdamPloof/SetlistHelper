using System;
using System.Collections.Generic;

namespace SetlistHelper.Extensions;

public static class ListExtensions {
    private static Random rng = new Random();

    /// <summary>
    /// Shuffle the elements of a list randomly in-place using the
    /// Fisher-Yates algorithm.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static void Shuffle<T>(this List<T> list) {
        int n = list.Count;
        while (n > 1) {
            n--;
            int k = rng.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }
}
