// ReSharper disable ConvertIfStatementToReturnStatement
// ReSharper disable ForCanBeConvertedToForeach

namespace Foci
{
    public static class CollectionExtensions
    {
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
    }
}