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
    }
}