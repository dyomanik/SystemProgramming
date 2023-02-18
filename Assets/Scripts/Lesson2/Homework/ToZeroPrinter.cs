using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Lesson2.Homework
{
    public class ToZeroPrinter : MonoBehaviour
    {
        private NativeArray<int> _numbersArray;

        // Start is called before the first frame update
        void Start()
        {
            _numbersArray = new NativeArray<int>(new int[] {1,15,9,10,22,13,8,3}, Allocator.TempJob);

            ToZeroJob toZeroJob = new ToZeroJob()
            {
                NumbersArray = _numbersArray
            };
            JobHandle jobHandle = toZeroJob.Schedule();
            jobHandle.Complete();

            
            for (var i = 0; i < toZeroJob.NumbersArray.Length; i++)
            {
                Debug.Log(toZeroJob.NumbersArray[i]);
            }
            _numbersArray.Dispose();

        }
    }
}