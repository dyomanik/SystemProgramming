using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class SecondTask : MonoBehaviour
{
    private static CancellationTokenSource _cancelTokenSource = new CancellationTokenSource();
    // Start is called before the first frame update
    async void Start()
    {       
            CancellationToken cancelToken = _cancelTokenSource.Token;
            Task task1 = new Task(() => Task1Async(cancelToken));
            Task task2 = new Task(() => Task2Async(cancelToken));
            task1.Start();
            task2.Start();
            Debug.Log(await WhatTaskFasterAsync(cancelToken, task1, task2));
            _cancelTokenSource.Dispose();
    }

    async void Task1Async(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            Debug.Log("Task1 aborted by token");
            return;
        }
        await Task.Delay(1000);
        Debug.Log("Task1 is finished");
    }

    async void Task2Async(CancellationToken cancellationToken)
    {
        for (int i = 0; i < 60; i++)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                Debug.Log($"Task2 aborted by token on frame {i}");
                return;
            }
            await Task.Yield();
        }
        Debug.Log("Task2 is finished");
    }

    async static Task<bool> WhatTaskFasterAsync(CancellationToken cancellationToken, Task task1, Task task2)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            Debug.Log($"Task aborted by token");
            return false;
        }
        await Task.Delay(0);
        var fasterTask = Task.WaitAny(task1, task2);
        if (fasterTask == 0)
        {
            _cancelTokenSource.Cancel();
            return true;
        }
        else
        {
            _cancelTokenSource.Cancel();
            return false;
        }
    }
}
