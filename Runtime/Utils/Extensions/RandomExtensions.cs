using System;

namespace GoodHub.Core.Runtime.Utils
{
    public static class RandomExtensions
    {
        /// <summary>
        /// Generates a random float within the range [0 (inclusive), 1 (exclusive)]. <br/>
        /// </summary>
        public static float NextFloat(this Random random)
        {
            return (float) random.NextDouble();
        }

        /// <summary>
        /// Generates a random float within the specified range [min (inclusive), max (exclusive)]. <br/>
        /// </summary>
        public static float NextFloat(this Random random, float min, float max)
        {
            return min + ((float) random.NextDouble() * (max - min));
        }

        /// <summary>
        /// Generates two unique random indices within the range [0 (inclusive), count (exclusive)]. <br/>
        /// Ensures that the two indices are distinct.
        /// </summary>
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

        public static float[] UniformDistribution(this Random random, int count, float min = 0f, float max = 1f)
        {
            float[] values = new float[count];

            for (int i = 0; i < count; i++)
            {
                values[i] = random.NextFloat(min, max);
            }

            return values;
        }

        public static float[] NormalDistributions(this Random random, int count, float mean = 0.0f, float variance = 1.0f)
        {
            float[] values = new float[count];

            for (int i = 0; i < count; i++)
            {
                float u1 = (float) (1f - random.NextDouble());
                float u2 = (float) (1f - random.NextDouble());

                float randStdNormal = (float) (Math.Sqrt(-2f * Math.Log(u1)) * Math.Sin(2f * Math.PI * u2));

                values[i] = mean + variance * randStdNormal;
            }

            return values;
        }

        public static float NormalDistribution(this Random random, float mean = 0.0f, float variance = 1.0f)
        {
            float u1 = (float) (1f - random.NextDouble());
            float u2 = (float) (1f - random.NextDouble());

            float randStdNormal = (float) (Math.Sqrt(-2f * Math.Log(u1)) * Math.Sin(2f * Math.PI * u2));

            return mean + variance * randStdNormal;
        }
    }
}