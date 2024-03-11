using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pool of gameobjects", menuName = "GameObject Creation System/Pools")]
public class Pool : ScriptableObject
{
    public GameObject prefab;
    public Transform ParentForInactiveObjects;

    public Queue<Poolable> pool = new Queue<Poolable>();

    public int counter = 0;
    public void ClearPool()
    {
        counter = 0;
        pool = new Queue<Poolable>();
    }
    public void FreeToPool(Poolable reuse)
    {
        GameObject gameObjectForReuse = reuse.gameObject;
        if (ParentForInactiveObjects)
        {
            gameObjectForReuse.transform.SetParent(ParentForInactiveObjects);
        }
        else
        {
            Debug.Log("Pool has not setted parent for unused objects, destroyng object", reuse);
            Destroy(reuse.gameObject);
            return;
        }
        if (! pool.Contains(reuse) )
        {
            pool.Enqueue(reuse);
            gameObjectForReuse.SetActive(false);
        }
    }

    public GameObject InstantiateFromPool()
    {
        Poolable temp = GetNewOrReused();
        GameObject gameObjectForReuse = temp.gameObject;
        gameObjectForReuse.SetActive(true);
        gameObjectForReuse.name = gameObjectForReuse.name + $" {counter}";
        counter++;
        return gameObjectForReuse;
    }
    private Poolable GetNewOrReused()
    {
        if (pool.Count>0)
        {
            return pool.Dequeue();
        }
        GameObject temp = Instantiate(prefab);
        Poolable poolable = temp.GetComponent<Poolable>();
        if (!poolable)
        {
            Debug.Log($"Prefab for pool had no poolable Component, adding default. {counter}", temp);
            poolable = temp.AddComponent<Poolable>();
        }
        poolable.SetPool(this);
        return poolable;
    }

}
