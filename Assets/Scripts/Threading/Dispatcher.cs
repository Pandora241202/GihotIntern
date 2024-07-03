using System;
using System.Collections.Generic;
using UnityEngine;

public class Dispatcher : MonoBehaviour
{
    private static readonly Queue<Action> _executionQueue = new Queue<Action>();

    private static Dispatcher _instance = null;
    public static Dispatcher Instance()
    {
        //if (!_instance)
        //{
        //    _instance = FindObjectOfType<Dispatcher>();
        //    if (!_instance)
        //    {
        //        var obj = new GameObject("Dispatcher");
        //        _instance = obj.AddComponent<Dispatcher>();
        //    }
        //}
        return _instance;
    }

    private void Start()
    {
        _instance = FindObjectOfType<Dispatcher>();
        DontDestroyOnLoad( _instance );
    }

    private void Update()
    {
        lock (_executionQueue)
        {
            while (_executionQueue.Count > 0)
            {
                _executionQueue.Dequeue().Invoke();
            }
        }
    }

    public void Enqueue(Action action)
    {
        lock (_executionQueue)
        {
            _executionQueue.Enqueue(action);
        }
    }

    public static void EnqueueToMainThread(Action action)
    {
        Instance().Enqueue(action);
    }
}
