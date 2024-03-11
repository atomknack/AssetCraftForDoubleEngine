using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoubleEngine;

public class FlatNodeEditorStartFragment : MonoBehaviour
{
    public MeshFragmentVec3D Fragment;
    private static FlatNodeEditorStartFragment instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else if(instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
