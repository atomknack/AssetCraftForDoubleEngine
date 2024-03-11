using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoubleEngine;
using DoubleEngine.Atom;

[RequireComponent(typeof(LoadNewFlatNode))]
public class ExportFlatNodeToJson : MonoBehaviour
{
    LoadNewFlatNode loader;
    private void Start()
    {
     loader = GetComponent<LoadNewFlatNode>();   
    }
    public void SaveFlatNodeToJson()
    {

        if (loader.flatnode!=null)
        {
            SaveNewFlatNodeToJson(loader.flatnode.Transformed);
        Debug.Log("Added new FlatNode");
        } else
        {
        Debug.Log("no flatnode to export to JSON");
        }
    }
    public static void SaveNewFlatNodeToJson(MeshFragmentVec3D fragment)
    {
            FlatNodes.CreateNewAndAdd(fragment);
            FlatNodes.SaveToJsonFile();
    }


}
