using UnityEngine;
using System.Collections.Generic;

public class Pools : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int poolSize = 10;

    private Queue<GameObject> pool = new Queue<GameObject>();
    private bool isInitialized = false;

    //private void Awake()
    //{
    //    if (prefab == null)
    //        Debug.LogError($"[Pools] {name}: prefab not found!");
    //}

    private void Initialize()
    {
        if (isInitialized) return;
        isInitialized = true;

        for (int i = 0; i < poolSize; i++)
            pool.Enqueue(CreateObject());
    }

    private GameObject CreateObject()
    {
        GameObject obj = Instantiate(prefab);
        obj.SetActive(false);

        if (gameObject.scene.isLoaded)
            obj.transform.SetParent(transform, false);

        if (obj.TryGetComponent<Bullet>(out var bullet))
            bullet.SetPool(this);

        return obj;
    }

    public GameObject SetObject()
    {
        Initialize();

        GameObject obj = pool.Count > 0 ? pool.Dequeue() : CreateObject();
        obj.SetActive(false);
        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        if (obj == null) return;
        obj.SetActive(false);

        if (!pool.Contains(obj))
            pool.Enqueue(obj);
    }
}