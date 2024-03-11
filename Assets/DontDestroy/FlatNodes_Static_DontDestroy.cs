using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoubleEngine;
using DoubleEngine.Atom;

public class FlatNodes_Static_DontDestroy : MonoBehaviour
{
    private static FlatNodes_Static_DontDestroy instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        FlatNodes.LoadFromJsonFile();
    }
}
