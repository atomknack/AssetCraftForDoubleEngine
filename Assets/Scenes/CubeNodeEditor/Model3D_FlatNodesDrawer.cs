using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoubleEngine;
using DoubleEngine.Atom;

public class Model3D_FlatNodesDrawer : MonoBehaviour
{
    public GameObject TextPrefab;
    public Material red;
    public Material green;
    private int _offset = 0;
    public List<(GameObject model, GameObject text)> _drawed = new List<(GameObject model, GameObject text)>();

    public void Clear()
    {
        foreach (var drawed in _drawed)
        {
            DestroyImmediate(drawed.model.GetComponent<MeshFilter>().mesh);
            DestroyImmediate(drawed.model);
            DestroyImmediate(drawed.text);
        }
        _drawed.Clear();
        _offset = 0;
    }
    public void NextOne()
    {
        _offset++;
    }
    public void DrawFlatNode(MeshFragmentVec3D fragment)
    {
        GameObject newObje = new GameObject("Empty", typeof(MeshFilter), typeof(MeshRenderer));
        newObje.GetComponent<MeshFilter>().mesh = fragment.ToNewUnityMesh(); //.ToMeshFragmentVector3().ToUnityMesh(); //(Mesh)
        newObje.transform.Translate(1.2f + _offset * 1.2f, 0, 0);

        GameObject text = Instantiate(TextPrefab, new Vector3(1.2f + _offset * 1.2f, -1.2f, -0.5f), Quaternion.identity);
        _drawed.Add((newObje, text));
        if (FlatNodes.TrySlowlyFindWithSameVertexPositions(fragment.To2D(), out int id, out var transfom))
        {

            newObje.GetComponent<MeshRenderer>().material = green;
            text.GetComponent<SetText_TMP>().SetText($"{id}, {transfom.ToString()}");
        }
        else
        {
            newObje.GetComponent<MeshRenderer>().material = red;
            text.GetComponent<SetText_TMP>().SetText("not found");
        }

        //newModel.transform.position = new Vector3(4, 0, 0);
        //newModel.transform.rotation = Quaternion.identity;

        NextOne();
    }
}
