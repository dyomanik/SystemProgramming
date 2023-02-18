using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

namespace Lesson2
{
    public struct MoveJob : IJobParallelForTransform
    {
        public NativeArray<Vector3> Positions;
        public NativeArray<Vector3> Velocities;
        public NativeArray<Vector3> Accelerations;

        [ReadOnly]
        public float DeltaTime;

        public void Execute(int index, TransformAccess transform)
        {
            Vector3 velocity = Velocities[index] + Accelerations[index];
            transform.position += velocity * DeltaTime;
            Positions[index] = transform.position;
            Velocities[index] = velocity;
            Accelerations[index] = Vector3.zero;
        }

    }
}