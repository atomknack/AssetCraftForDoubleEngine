
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoubleEngine;
using DoubleEngine.Atom;

[System.Serializable]
public class DrawSingleThreeDimensionalCellMesh : MonoBehaviour
{
    [SerializeField] private GameObject flatNodePlaceholder;

    public void UpdateMesh(Mesh mesh)
        {
        flatNodePlaceholder.GetComponent<MeshFilter>().sharedMesh = mesh;//.ToMeshFragmentVector3().ToUnityMesh();
    }
    public void CleanMesh()
    {
        flatNodePlaceholder.GetComponent<MeshFilter>().sharedMesh = null;
    }
}