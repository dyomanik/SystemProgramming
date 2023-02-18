using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Lesson2.Homework
{
    public class SumPositionsPrinter : MonoBehaviour
    {
        private NativeArray<Vector3> _positions;
        private NativeArray<Vector3> _velocities;
        private NativeArray<Vector3> _finalPositions;

        [SerializeField]
        private int _quantityOfVectors = 10;

        private void Start()
        {
            _positions = new NativeArray<Vector3>(_quantityOfVectors, Allocator.TempJob);
            _velocities = new NativeArray<Vector3>(_quantityOfVectors, Allocator.TempJob);
            _finalPositions = new NativeArray<Vector3>(_quantityOfVectors, Allocator.TempJob);

            for (var i = 0; i < _positions.Length; i++)
            {
                _positions[i] = new Vector3(i, i, i);
            }

            for (var i = 0; i < _velocities.Length; i++)
            {
                _velocities[i] = new Vector3(i*i, i*i, i*i);
            }

            SumPositionsJob sumPositionsJob = new SumPositionsJob()
            {
                Positions = _positions,
                Velocities = _velocities,
                FinalPositions = _finalPositions,
            };


            JobHandle jobHandle = sumPositionsJob.Schedule(_quantityOfVectors, 0);
            jobHandle.Complete();

            
            for (var i = 0; i <= _quantityOfVectors - 1; i++)
            {
                Debug.Log(sumPositionsJob.FinalPositions[i]);
            }
            _positions.Dispose();
            _velocities.Dispose();
            _finalPositions.Dispose();
        }
    }
}