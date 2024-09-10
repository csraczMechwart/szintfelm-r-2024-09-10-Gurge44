using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable ConvertIfStatementToReturnStatement
// ReSharper disable ForCanBeConvertedToForeach

namespace Foci
{
    public static class CollectionExtensions
    {
        public static TKey GetKeyByValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TValue value)
        {
            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
            {
                if (pair.Value.Equals(value))
                {
                    return pair.Key;
                }
            }

            return default;
        }

        public static void SetAllValues<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TValue value)
        {
            foreach (TKey key in dictionary.Keys.ToArray())
            {
                dictionary[key] = value;
            }
        }

        public static void AdjustAllValues<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Func<TValue, TValue> adjust)
        {
            foreach (TKey key in dictionary.Keys.ToArray())
            {
                dictionary[key] = adjust(dictionary[key]);
            }
        }

        public static T RandomElement<T>(this IList<T> collection)
        {
            if (collection.Count == 0) return default;
            return collection[new Random().Next(collection.Count)];
        }

        public static T RandomElement<T>(this IEnumerable<T> collection)
        {
            if (collection is IList<T> list) return list.RandomElement();
            return collection.ToList().RandomElement();
        }

        public static IEnumerable<T> CombineWith<T>(this IEnumerable<T> firstCollection, params IEnumerable<T>[] collections)
        {
            return firstCollection.Concat(collections.Flatten());
        }

        public static void Do<T>(this IEnumerable<T> collection, Action<T> action)
        {
            if (collection is List<T> list)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    action(list[i]);
                }

                return;
            }

            foreach (T element in collection)
            {
                action(element);
            }
        }

        public static void DoIf<T>(this IEnumerable<T> collection, Func<T, bool> predicate, Action<T> action, bool fast = true)
        {
            if (fast)
            {
                if (collection is List<T> list)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        T element = list[i];
                        if (predicate(element))
                        {
                            action(element);
                        }
                    }

                    return;
                }

                foreach (T element in collection)
                {
                    if (predicate(element))
                    {
                        action(element);
                    }
                }

                return;
            }

            collection.Where(predicate).ToArray().Do(action);
        }

        public static (List<T> TrueList, List<T> FalseList) Split<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
        {
            var list1 = new List<T>();
            var list2 = new List<T>();
            foreach (T element in collection)
            {
                if (predicate(element))
                {
                    list1.Add(element);
                }
                else
                {
                    list2.Add(element);
                }
            }

            return (list1, list2);
        }

        public static Dictionary<TKey, TValue> AddRange<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Dictionary<TKey, TValue> other, bool overrideExistingKeys = true)
        {
            foreach ((TKey key, TValue value) in other)
            {
                if (overrideExistingKeys || !dictionary.ContainsKey(key))
                {
                    dictionary[key] = value;
                }
            }

            return dictionary;
        }

        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> collection)
        {
            return collection.SelectMany(x => x);
        }

        public static bool FindFirst<T>(this IEnumerable<T> collection, Func<T, bool> predicate, out T element)
        {
            if (collection is List<T> list)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    T item = list[i];
                    if (predicate(item))
                    {
                        element = item;
                        return true;
                    }
                }

                element = default;
                return false;
            }

            foreach (T item in collection)
            {
                if (predicate(item))
                {
                    element = item;
                    return true;
                }
            }

            element = default;
            return false;
        }

        #region Without

        public static IEnumerable<T> Without<T>(this IEnumerable<T> collection, T element)
        {
            return collection.Where(x => !x.Equals(element));
        }

        #endregion

        #region Shuffle

        public static List<T> Shuffle<T>(this IEnumerable<T> collection)
        {
            var list = collection.ToList();
            int n = list.Count;
            var r = new Random();
            while (n > 1)
            {
                n--;
                int k = r.Next(n + 1);
                (list[n], list[k]) = (list[k], list[n]);
            }

            return list;
        }

        #endregion

        #region Partition

        public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> collection, int parts)
        {
            var list = collection.ToList();
            var length = list.Count;
            if (parts <= 0 || length == 0) yield break;
            if (parts > length) parts = length;
            int size = length / parts;
            int remainder = length % parts;
            int index = 0;
            for (int i = 0; i < parts; i++)
            {
                int partSize = size + (i < remainder ? 1 : 0);
                yield return list.Skip(index).Take(partSize);
                index += partSize;
            }
        }

        /// <summary>
        /// Partitions a list into a specified number of parts
        /// </summary>
        /// <param name="collection">The list to partition</param>
        /// <param name="parts">The number of parts to partition the list into</param>
        /// <typeparam name="T">The type of the elements in the list</typeparam>
        /// <returns>A list of lists, each containing a part of the original list</returns>
        public static IEnumerable<IEnumerable<T>> Partition<T>(this IList<T> collection, int parts)
        {
            var length = collection.Count;
            if (parts <= 0 || length == 0) yield break;
            if (parts > length) parts = length;
            int size = length / parts;
            int remainder = length % parts;
            int index = 0;
            for (int i = 0; i < parts; i++)
            {
                int partSize = size + (i < remainder ? 1 : 0);
                yield return collection.Skip(index).Take(partSize);
                index += partSize;
            }
        }

        #endregion
    }

    public static class Loop
    {
        public static void Times(int count, Action<int> action)
        {
            for (int i = 0; i < count; i++)
            {
                action(i);
            }
        }
    }
}