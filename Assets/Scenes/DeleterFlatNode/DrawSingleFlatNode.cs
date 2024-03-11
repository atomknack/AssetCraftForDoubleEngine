using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoubleEngine;
using DoubleEngine.Atom;

[System.Serializable]
public class DrawSingleFlatNode : MonoBehaviour
{
    [SerializeField] private GameObject flatNodePlaceholder;

    public void UpdateFlatNode(FlatNode node)
        {
        flatNodePlaceholder.GetComponent<MeshFilter>().sharedMesh = node.Transformed.ToNewUnityMesh();//.ToMeshFragmentVector3().ToUnityMesh();
    }
    public void CleanFlatNode()
    {
        flatNodePlaceholder.GetComponent<MeshFilter>().sharedMesh = null;
    }
}
