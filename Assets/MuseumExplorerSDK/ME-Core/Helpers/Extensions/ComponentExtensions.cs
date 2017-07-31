using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;

namespace Helpers.Extensions
{
    public static class ComponentExtensions
    {
        /// <summary>
        /// Finds and returns all components of type T that are found under this game object excluding the components found on the caller itself.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mb"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetComponentsOnlyInChildren<T>(this Component c, bool includeInactive = false)
        {
            var components = c.transform.GetComponents<T>();
            return c.transform.GetComponentsInChildren<T>(includeInactive).Where(obj => !components.Contains(obj));
        }

        /// <summary>
        /// Finds and returns all components of type T that are found from parents excluding the components found on the caller itself.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mb"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetComponentsOnlyInParents<T>(this Component c, bool includeInactive = false)
        {
            var components = c.transform.GetComponents<T>();
            return c.transform.GetComponentsInParent<T>(includeInactive).Where(obj => !components.Contains(obj));
        }

        /// <summary>
        /// Filters duplicates.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetComponentsInChildrenAndParents<T>(this Component c, bool includeInactive = false)
        {
            var children = c.GetComponentsInChildren<T>(includeInactive);
            var parents = c.GetComponentsOnlyInParents<T>(includeInactive);
            return children.Concat(parents);
        }
    }
}

