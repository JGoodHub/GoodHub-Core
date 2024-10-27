using System;

namespace GoodHub.Core.Runtime.Utils
{
    public static class RandomExtensions
    {

        public static float NextFloat(this Random random)
        {
            return (float)random.NextDouble();
        }

        public static float NextFloat(this Random random, float min, float max)
        {
            return min + ((float)random.NextDouble() * (max - min));
        }

        public static void NextSwapIndices(this Random random, int count, out int indexA, out int indexB)
        {
            if (count <= 1)
            {
                indexA = 0;
                indexB = 0;
                return;
            }

            do
            {
                indexA = random.Next(count);
                indexB = random.Next(count);
            }
            while (indexA == indexB);
        }

    }
}