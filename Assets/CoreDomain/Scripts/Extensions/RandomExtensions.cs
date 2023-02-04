using System;
using System.Collections.Generic;
using System.Linq;
using Services.Logs.Base;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CoreDomain.Scripts.Extensions
{
    public static class RandomExtensions
    {
        private const float MinPercent = 0f;
        private const float MaxPercent = 1f;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="percentChange">value between 0 to 1</param>
        /// <returns></returns>
        public static bool TossRandomPercentChance(this float percentChange)
        {
            return Random.Range(MinPercent, MaxPercent) < percentChange;
        }
        
        public static float RandomFloatFromRange(this Vector2 valueRange)
        {
            return Random.Range(valueRange.x, valueRange.y);
        }
        
        public static T GetRandomItem<T>(this IEnumerable<T> itemsEnumerable, Func<T, int> weightKey)
        {
            var items = itemsEnumerable.ToList();

            var totalWeight = items.Sum(x => weightKey(x));
            var randomWeightedIndex = Random.Range(0, totalWeight);
            var itemWeightedIndex = 0;
            
            foreach(var item in items)
            {
                itemWeightedIndex += weightKey(item);

                if (randomWeightedIndex < itemWeightedIndex)
                {
                    return item;
                }
            }
            
            LogService.LogError("Collection count and weights must be greater than 0");
            return default;
        }
    }
}