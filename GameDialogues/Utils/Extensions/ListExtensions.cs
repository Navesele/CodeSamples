using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Marshtown.Extensions
{
    public static class ListExtensions
    {
        public static bool IsNullOrEmpty<T>(this List<T> list)
        {
            return list == null || list.Count == 0;
        }

        public static List<T> RemoveNulls<T>(this List<T> list)
        {
            if (list.IsNullOrEmpty())
            {
                return list;
            }

            return list.Where(item => item != null).ToList();
        }
    }
}
