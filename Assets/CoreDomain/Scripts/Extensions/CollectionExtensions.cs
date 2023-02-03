﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CoreDomain.Scripts.Extensions
{
    public static class CollectionExtensions
    {
        private const string ClosedBrackets = "}";
        private const string Count = "Count: ";
        private const string OpenedBrackets = "{";
        private const string Zero = "0";
        private const string Coma = ", ";

        public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
        {
            return collection == null || collection.Count == 0;
        }

        public static bool TryGetValue<T, TV>(this IDictionary<T, TV> dictionary, T key, out TV val)
        {
            if (!dictionary.ContainsKey(key))
            {
                val = default;
                return false;
            }

            val= dictionary[key];
            return true;
        }

        public static string ToStringItemsSweat<T>(this ICollection<T> list)
        {
            var sb = new StringBuilder();

            sb.Append(OpenedBrackets);
            sb.Append(Count);

            if (list.IsNullOrEmpty())
            {
                sb.Append(Zero);
            }
            else
            {
                sb.Append(list.Count);

                foreach (var item in list)
                {
                    sb.AppendLine();
                    sb.Append(OpenedBrackets);
                    sb.Append(item);
                    sb.Append(ClosedBrackets);
                }
            }

            sb.AppendLine(ClosedBrackets);
            return sb.ToString();
        }
        
        public static string ToStringItems<T>(this ICollection<T> list)
        {
            return string.Join(Coma, list);
        }

        public static bool TryFind<T>(this ICollection<T> collection, Func<T, bool> condition, out T foundItem)
        {
            foreach (var item in collection)
            {
                if (condition(item))
                {
                    foundItem = item;
                    return true;
                }
            }

            foundItem = default;
            return false;
        }
    }
}