using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoubleEngine;
using DoubleEngine.Atom;

public class Resheller : MonoBehaviour
{
    public MeshFilter ReshellerPlaceholder;

    private void Start()
    {
        if (ReshellerPlaceholder == null)
            Debug.LogError("Placeholder for reshelled mesh not set");
        FlatNodes.LoadFromJsonFile();
    }

    public void Recieve(DeshelledCubeMesh deshelled)
    {
        Clear();
        MeshFragmentVec3D reshelledMesh = deshelled.Reshelled_ForDebug();
        ReshellerPlaceholder.sharedMesh = reshelledMesh.ToNewUnityMesh();
        //throw new NotImplementedException();
    }

    public void Clear()
    {
        ReshellerPlaceholder.sharedMesh = null;
    }
}
