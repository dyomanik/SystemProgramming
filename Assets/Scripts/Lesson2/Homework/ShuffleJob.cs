using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Unity.Burst;

namespace Lesson2.Homework
{
    [BurstCompile]
    public struct ShuffleJob : IJob
    {
        public NativeArray<Color> Colors;
        public uint Seed;

        public void Execute()
        {
            Unity.Mathematics.Random random = new Unity.Mathematics.Random(Seed);
            for (var i = 0; i < Colors.Length; i++)
            {
                Color tempColor;
                int intRandom = random.NextInt(0, Colors.Length);
                tempColor = Colors[intRandom];
                Colors[intRandom] = Colors[i];
                Colors[i] = tempColor;
            }
        }
    }
}