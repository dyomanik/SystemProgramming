using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Lesson1Test : MonoBehaviour
{

    void Start()
    {
        CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        CancellationToken cancelToken = cancelTokenSource.Token;
        Task task = new Task(() => FactorialAsync(cancelToken, 5));
        task.Start();
        cancelTokenSource.Cancel();
        cancelTokenSource.Dispose();
    }


    //async void Start()
    //{
    //    Task<int> task1 = WaitRandomTime();
    //    Task<int> task2 = WaitRandomTime();
    //    var taskResult = await Task.WhenAny(task1, task2);
    //    Debug.Log(taskResult.Result);
    //}

    IEnumerator MoveUp(float time, Vector3 direction)
    {
        while (transform.position.y < 10)
        {
            yield return new WaitForSeconds(time);
            transform.position += direction;
        }
    }

    IEnumerator MoveAround()
    {
        transform.position += Vector3.up;
        yield return new WaitForSeconds(1);
        transform.position += Vector3.left;
        yield return new WaitForSeconds(1);
        transform.position += Vector3.down;
        yield return new WaitForSeconds(1);
        transform.position += Vector3.right;
    }

    IEnumerator PrintAndDestroy()
    {
        int i = 10;
        while (true)
        {
            Debug.Log($"{i} seconds left.");
            i--;
            if (i == 1) this.enabled = false; // деактивирует скрипт, но всё равно продолжит работать
            if (i == 0)
            {
                Destroy(this.gameObject); // уничтожит объект и прекратит работу
            }
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator PrintMessage()
    {
        while (true)
        {
            Debug.Log("Test message!!!");
            yield return null;
        }
    }

    async void PrintAsync(string message, int times)
    {
        while (times > 0)
        {
            times--;
            Debug.Log(message);
            await Task.Yield();
        }
    }

    async void UnitTasksAsync()
    {
        //Task task1 = Task.Run(() => Unit1Async());
        //Task task2 = Task.Run(() => Unit2Async());
        await Task.WhenAll(Unit1Async(), Unit2Async());
        Debug.Log("All units have finished their tasks.");
    }
    async Task Unit1Async()
    {
        Debug.Log("Unit1 starts chopping wood.");
        await Task.Delay(3000);
        Debug.Log("Unit1 finishes chopping wood.");
    }
    async Task Unit2Async()
    {
        Debug.Log("Unit2 starts patrolling.");
        await Task.Delay(5000);
        Debug.Log("Unit2 finishes patrolling.");
    }

    async Task<int> WaitRandomTime()
    {
        int rnd = Random.Range(100, 1000);
        await Task.Delay(rnd);
        return rnd;
    }

    async Task<long> FactorialAsync(CancellationToken cancelToken, int x)
    {
        int result = 1;
        for (int i = 1; i < x; i++)
        {
            if (cancelToken.IsCancellationRequested)
            {
                Debug.Log("Операция прервана токеном.");
                return result;
            }
            result *= i;
            await Task.Yield();
        }
        return result;
    }
}
