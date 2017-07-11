using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour{

    public int PoolInitSize;
    public int PoolIncrement;
    public GameObject PoolObject;

    private List<GameObject> pool;

    void Awake()
    {
        AddObjectsToPool(PoolInitSize);
    }

    private void AddObjectsToPool(int amount)
    {
        for (int i = 0; i < amount; ++i)
        {
            GameObject go = Instantiate(PoolObject, transform);
            go.SetActive(false);
            pool.Add(go);
        }
    }

    public GameObject FetchObject()
    {
        foreach (GameObject go in pool)
            if (!go.activeSelf)
                return go;
        
        //If there are no inactive objects, create more into the pool
        AddObjectsToPool(PoolIncrement);
        return FetchObject();
    }

    public void ClearPool()
    {
        foreach (GameObject go in pool)
            Destroy(go);

        pool.Clear();
    }
}
