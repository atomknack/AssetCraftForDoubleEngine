using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[DisallowMultipleComponent]
public class Poolable : MonoBehaviour
{
    private Pool pool;

    public void SetPool(Pool _pool)
    {
        pool = _pool;
    }

    public void RemoveToPool()
    {
        pool.FreeToPool(this);
    }

}
