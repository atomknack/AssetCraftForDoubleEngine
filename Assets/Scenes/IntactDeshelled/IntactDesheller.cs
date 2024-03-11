using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoubleEngine;
using DoubleEngine.Atom;
using SimpleFileBrowser;

public class IntactDesheller : MonoBehaviour
{
    public MeshFilter DeshelledPlaceholder;
    public Resheller ReshellerPlaceholder;
    private DeshelledCubeMesh deshelled;

    private void Start()
    {
        if (DeshelledPlaceholder == null)
            Debug.LogError("Placeholder for deshelled mesh not set");
        if (ReshellerPlaceholder == null)
            Debug.LogError("Placeholder for reshelled mesh not set");
        FlatNodes.LoadFromJsonFile();
    }

    public void LoadNewModel(string path)
    {
        Clear();
        try
        {
            Recieve( JsonHelpers.LoadFromJsonFile<DeshelledCubeMesh>(path));
        }
        catch (Exception ex)
        {
            Debug.Log($"Cannot load file: {path}");
            Debug.LogException(ex);
        }
    }
    public void OpenLoadDeshelledCubeNodeDialog()
    {
        var filters = new string[] { ".DeshelledJson" };
        FileBrowser.SetFilters(true, filters);
        FileBrowser.SetDefaultFilter(filters[0]);
        FileBrowser.ShowLoadDialog((paths) => { Debug.Log($"load path success: {paths[0]}"); LoadNewModel(paths[0]); }, () => Debug.Log($"load path canceled"), FileBrowser.PickMode.Files, allowMultiSelection: false, initialFilename: null, title: "Load", loadButtonText: "Select");
    }

    public void ShowSaveDeshelledDialog()
    {
        if(deshelled == null)
        {
            Debug.Log("No deshelled model");
            return;
        }
        var filters = new string[] { ".DeshelledJson" };
        FileBrowser.SetFilters(true, filters);
        FileBrowser.SetDefaultFilter(filters[0]);
        FileBrowser.ShowSaveDialog((paths) => { Debug.Log($"save path success: {paths[0]}"); SaveDeshelledModel(paths[0]); }, () => Debug.Log($"save deshelled canceled"), FileBrowser.PickMode.Files, allowMultiSelection: false, initialFilename: null, title: "Save");
        //FileBrowser.SetFilters(true);
    }

    public void SaveDeshelledModel(string path)
    {
        try
        {
            JsonHelpers.SaveToJsonFile(deshelled,path);
        }
        catch (Exception ex)
        {
            Debug.Log($"Cannot save file: {path}");
            Debug.LogException(ex);
        }
    }

    public void Recieve(IntactCubeMesh intact)
    {
        Recieve(DeshelledCubeMesh.Create(intact));
    }

    public void Recieve(DeshelledCubeMesh recived)
    {
        Clear();
        deshelled = recived;
        DeshelledPlaceholder.sharedMesh = ((ISerializableCubeMesh)deshelled).Mesh.ToNewUnityMesh();
        ReshellerPlaceholder.Recieve(deshelled);
    }

    public void Clear()
    {
        deshelled = null;
        DeshelledPlaceholder.sharedMesh = null;
        ReshellerPlaceholder.Clear();
    }
}
