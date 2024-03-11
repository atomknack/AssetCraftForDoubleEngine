using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMeshFromMeshFilter : MonoBehaviour
{
    public MeshFilter debugMesh = null;
    public Vector3[] DebugNewMeshVertices;
    public int[] DebugNewMeshTriangles;

    
    void Start()
    {
        if (debugMesh == null)
        {
            debugMesh = gameObject.GetComponent<MeshFilter>();
        }
        DebugNewMeshVertices = debugMesh.mesh.vertices;
        DebugNewMeshTriangles = debugMesh.mesh.triangles;
    }

}
