using Unity.Collections;
using Unity.Jobs;

namespace Lesson2.Homework
{

    public struct ToZeroJob : IJob
    {
        public NativeArray<int> NumbersArray;

        public void Execute()
        {
            for (var i = 0; i < NumbersArray.Length; i++)
            {
                if (NumbersArray[i] > 10)
                {
                    NumbersArray[i] = 0;
                }
            }
        }


    }
}