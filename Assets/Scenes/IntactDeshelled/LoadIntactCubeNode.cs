using SimpleFileBrowser;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoubleEngine;
using DoubleEngine.Atom;
using System;

public class LoadIntactCubeNode : MonoBehaviour
{
    public MeshFilter LoadedIntactPlaceholder;
    public IntactDesheller IntactDesheller;
    private IntactCubeMesh intact;

    private void Start()
    {
        if (LoadedIntactPlaceholder == null)
            Debug.LogError("Placeholder for intact mesh not set");
        if (IntactDesheller == null)
            Debug.LogError("IntactDesheller not set");
    }
    public void LoadNewModel(string path)
    {
        Clear();
        try
        {
            intact = JsonHelpers.LoadFromJsonFile<IntactCubeMesh>(path);
            LoadedIntactPlaceholder.sharedMesh = ((ISerializableCubeMesh)intact).Mesh.ToNewUnityMesh();
            IntactDesheller.Recieve(intact);
        }
        catch (Exception ex)
        {
            Debug.Log($"Cannot load file: {path}");
            Debug.LogException(ex);
        }
    }
    public void OpenLoadIntactCubeNodeDialog()
    {
        var filters = new string[] { ".IntactJson" };
        FileBrowser.SetFilters(true, filters);
        FileBrowser.SetDefaultFilter(filters[0]);
        FileBrowser.ShowLoadDialog((paths) => { Debug.Log($"load path success: {paths[0]}"); LoadNewModel(paths[0]); }, () => Debug.Log($"load path canceled"), FileBrowser.PickMode.Files, allowMultiSelection: false, initialFilename: null, title: "Load", loadButtonText: "Select");
    }

    public void Clear()
    {
        intact = null;
        LoadedIntactPlaceholder.sharedMesh = null;
        IntactDesheller.Clear();
    }

    public void AddToCellMeshes()
    {
        if (intact != null)
        {
            ThreeDimensionalCellMeshes.AddCellMesh(intact);
            ThreeDimensionalCellMeshes.ExportToJsonFile();
            Debug.Log($"Added as {ThreeDimensionalCellMeshes.GetCount() - 1}");
        }
        else
            Debug.Log("Cannot add null as IntactMesh");
    }
}
