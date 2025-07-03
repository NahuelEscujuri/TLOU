using System;
using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
    public static class Extensions
    {
        public static T GetRandomElement<T>(this IList<T> list)
        {
            if (list == null || list.Count == 0)
                throw new ArgumentException("La lista no puede estar vacía.");

            return list[UnityEngine.Random.Range(0, list.Count)];
        }
    }
}
