using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_Queue
{
    public static Queue<GameObject> q = new Queue<GameObject>();

    public static void Push(GameObject go)
    {
        q.Enqueue(go);
    }

    public static GameObject Pop()
    {
        GameObject go = q.Dequeue();
        return go;
    }

    public static int Count()
    {
        return q.Count;
    }

    public static void Clear()
    {
        q.Clear();
    }
}
