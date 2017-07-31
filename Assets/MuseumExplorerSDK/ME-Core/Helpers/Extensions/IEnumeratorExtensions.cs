using System.Collections.Generic;
using System;
using System.Linq;
using Random = UnityEngine.Random;

namespace Helpers.Extensions
{
    public static class IEnumeratorExtensions
    {
        /// <summary>
        /// Advances the enumerator/iterator to the next and by default resets when the last item is reached. 
        /// Resetting can be disabled, in which case returns the last item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iterator"></param>
        /// <param name="resetAfterLast"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetNext<T>(this IEnumerator<T> iterator, bool resetAfterLast = true)
        {
            if (!iterator.MoveNext())
            {
                if (resetAfterLast)
                {
                    iterator.Reset();
                    iterator.MoveNext();
                }
            }
            yield return iterator.Current;
        }
    }
}
