using System.Collections.Generic;
using System;
using System.Linq;
using Random = UnityEngine.Random;

namespace Helpers.Extensions
{
    public static class IEnumerableExtensions
    {
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source);
        }

        /// <summary>
        /// Randomizes the collection.
        /// NOTES:
        /// Does not work on iOS (JIT exception). Use source.OrderBy(i => Random.value) instead.
        /// Null throws an exception.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IOrderedEnumerable<T> Randomize<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(i => Random.value);
        }

        /// Randomizes the collection and returns the first element.
        /// NOTE1: Does not work on iOS (JIT exception). Use source.OrderBy(i => Random.value) instead.
        /// NOTE2: For better efficiency, use collection[Random.Range(0, collection.Length - 1)] instead.
        /// </summary>
        /// <returns>The random.</returns>
        /// <param name="source">Source.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T GetRandom<T>(this IEnumerable<T> source)
        {
            return source.Randomize().FirstOrDefault();
        }

        /// <summary>
        /// Executes an action that modifies the collection on each element (such as removing items from the list).
        /// Creates a temporary list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        public static void ForEachMod<T>(this IEnumerable<T> source, Action<T> action)
        {
            var temp = new List<T>(source);
            temp.ForEach(i => action(i));
        }

        /// <summary>
        /// Performs the specified action on each element of the collection (short hand of a foreach loop).
        /// This is a custom, generic version of the List.ForEach method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

        /// <summary>
        /// Shorthand for !source.Any(i => predicate(i)) -> i.e. not any.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static bool None<T>(this IEnumerable<T> source, Func<T, bool> predicate = null)
        {
            if (predicate == null)
            {
                return !source.Any();
            }
            else
            {
                return !source.Any(i => predicate(i));
            }
        }

        /// <summary>
        /// If a predicate is given, uses System.Linq Where method, otherwise simply checks if there is more than one item in the collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static bool Multiple<T>(this IEnumerable<T> source, Func<T, bool> predicate = null)
        {
            if (predicate == null)
            {
                return source.Count() > 1;
            }
            else
            {
                return source.Where(i => predicate(i)).Count() > 1;
            }
        }
    }
}

