using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastLoaded3dMesh : MonoBehaviour
{
    public Mesh mesh;
    private static LastLoaded3dMesh instance;
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
}
