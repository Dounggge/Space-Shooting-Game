using UnityEngine;
using System.Collections.Generic;

public class Pools : MonoBehaviour
{
    [SerializeField] protected GameObject prefab;
    [SerializeField] public int poolSize = 4;

    private Queue<GameObject> pool = new Queue<GameObject>();

    public void InitPool(int size)
    {
        pool = new Queue<GameObject>();

        if (prefab == null)
        {
            Debug.LogError("Pools: prefab is not assigned!");
            return;
        }

        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject SetObject()
    {
        /*if (pool == null)
        {
            //Debug.LogError("Pools: pool is null. Did you call InitPool()?");
            return null;
        }*/

        GameObject obj;

        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
            //Debug.LogWarning("Pools exhausted, instantiating extra object.");
            obj = Instantiate(prefab);
        }

        obj.SetActive(true);
        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        if (obj == null) return;

        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}