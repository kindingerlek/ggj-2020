using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utils.Collection
{
    static class ExtensionCollection
    {
        public static T PopAt<T>(this List<T> list, int index)
        {
            T r = list[index];
            list.RemoveAt(index);
            return r;
        }

        /// <summary>
        /// Pick a random item from the list, returning that item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            return source.ElementAt(Random.Range(0, source.Count()));
        }

        /// <summary>
        /// Removes a random item from the list, returning that item.
        /// Sampling without replacement.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T RemoveRandom<T>(this IList<T> source)
        {
            if (source.Count == 0) throw new System.IndexOutOfRangeException("Cannot remove a random item from an empty list");
            int index = UnityEngine.Random.Range(0, source.Count);
            T item = source[index];
            source.RemoveAt(index);
            return item;
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => System.Guid.NewGuid());
        }
    }
}