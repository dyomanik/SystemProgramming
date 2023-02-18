using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Burst;

namespace Lesson2.Homework
{
    [BurstCompile]
    public struct RotationJob : IJobParallelForTransform
    {
        public Vector3 Direction;
        public float DeltaTime;
        public float Random;
        public float Multiplayer;
        public NativeArray<Color> ColorInput;
        public NativeArray<Color> ColorOutput;
        public NativeArray<int> Angles;
        
        public void Execute(int index, TransformAccess transform)
        {
            transform.position += Direction * DeltaTime;
            transform.localRotation = Quaternion.AngleAxis(Angles[index] * Multiplayer, Direction);
            Angles[index] = Angles[index] == 180 ? 0 : Angles[index] + 1;
            ColorOutput[index] = Color.Lerp(ColorOutput[index], ColorInput[index], DeltaTime);
        }
    }
}