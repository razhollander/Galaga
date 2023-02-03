using System;

namespace CoreDomain.Scripts.Extensions
{
    public static class FloatExtensions
    {
        private const float Tolerance = 0.001f;
        private static readonly Random _random = new();

        public static bool EqualsWithTolerance(this float number, float otherNumber)
        {
            return Math.Abs(number - otherNumber) < Tolerance;
        }

        public static bool Raffle(this double chance)
        {
            return _random.NextDouble() <= chance;
        }
        
        /// <summary>
        /// Rounds a double-precision floating-point value to the nearest int value.
        /// </summary>
        /// <param name="value">A double-precision floating-point number to be rounded</param>
        /// <returns></returns>
        public static int RoundToInt(this float value)
        {
            return (int) Math.Round(value);
        }
    }
}