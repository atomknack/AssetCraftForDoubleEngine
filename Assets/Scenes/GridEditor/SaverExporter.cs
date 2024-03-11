using SimpleFileBrowser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
using DoubleEngine;
using DoubleEngine.Atom;

[RequireComponent(typeof(UIDocument))]
public class SaverExporter : MonoBehaviour
{
    [SerializeField]
    private GridMeshGenerator gridMeshGenerator;

    private Button _loadButton;
    private Button _saveButton;
    private Button _exportMesh3DButton;
    private Button _exportDecimatedMesh3DButton;
    private Button _exportObjButton;
    private VisualElement _rootUI;


    private void OnEnable()
    {
        if (gridMeshGenerator == null)
        {
            Debug.Log("No gridMeshGenerator component");
            throw new MissingComponentException(nameof(gridMeshGenerator));
        }
        _rootUI = GetComponent<UIDocument>().rootVisualElement;
        _loadButton = _rootUI.Q<Button>("Load");
        _loadButton.RegisterCallback<ClickEvent>(ev => LoadButtonClicked());
        _saveButton = _rootUI.Q<Button>("Save");
        _saveButton.RegisterCallback<ClickEvent>(ev => SaveButtonClicked());
        _exportMesh3DButton = _rootUI.Q<Button>("ExportMesh3d");
        _exportMesh3DButton.RegisterCallback<ClickEvent>(ev => ExportMesh3DButtonClicked());
        
        _exportDecimatedMesh3DButton = _rootUI.Q<Button>("ExportDecimatedMesh3d");
        _exportDecimatedMesh3DButton.RegisterCallback<ClickEvent>(ev => ExportDecimatedMesh3DButtonClicked());
        _exportObjButton = _rootUI.Q<Button>("ExportObj");
        _exportObjButton.RegisterCallback<ClickEvent>(ev => ExportObjButtonClicked());

    }

    private void OnDisable()
    {
        _loadButton?.UnregisterCallback<ClickEvent>(ev => LoadButtonClicked());
        _saveButton?.UnregisterCallback<ClickEvent>(ev => SaveButtonClicked());
        _exportMesh3DButton?.UnregisterCallback<ClickEvent>(ev => ExportMesh3DButtonClicked());
        _exportDecimatedMesh3DButton?.UnregisterCallback<ClickEvent>(ev => ExportDecimatedMesh3DButtonClicked());
        _exportObjButton?.UnregisterCallback<ClickEvent>(ev => ExportObjButtonClicked());
    }

    public void SaveButtonClicked()
    {
        //var filters = new string[] { ".bg10" };
        FileBrowser.SetFilters(true, GridLoaders.CanSaveToExtensions);//filters);
        FileBrowser.SetDefaultFilter(GridLoaders.Extension_BG10);//filters[0]);
        FileBrowser.ShowSaveDialog((paths) => { SaveClicked(paths[0]); }, () => Debug.Log($"Save grid to bg file canceled"), FileBrowser.PickMode.Files, allowMultiSelection: false, initialFilename: null, title: "Save grid to bg file");
        
        void SaveClicked(string path)
        {
            Debug.Log($"Export path chosen: {path}");
            Try.Action(x => GridLoaders.SaveGrid(gridMeshGenerator.GetIGridReference(), x), path, "save to bg file");
        }
    }



    public void LoadButtonClicked()
    {
        //var filters = new string[] { ".bg10" };
        FileBrowser.SetFilters(true, GridLoaders.CanLoadFromExtensions);
        FileBrowser.SetDefaultFilter(GridLoaders.Extension_BG10);
        FileBrowser.ShowLoadDialog((paths) => { LoadClicked(paths[0]); }, () => Debug.Log($"Load grid from bg file canceled"), FileBrowser.PickMode.Files, allowMultiSelection: false, initialFilename: null, title: "Load grid from bg file");
        
        void LoadClicked(string path)
        {
            Debug.Log($"Load path chosen: {path}");
            Try.Action(x => {
                GridLoaders.LoadGrid(gridMeshGenerator.GetIGridReference(), x);
                gridMeshGenerator.UpdateMesh();
            }, path, "load from bg file");
        }
    }
    
    public void ExportMesh3DButtonClicked()
    {
        string formatName = "mesh3d12";
        var filters = new string[] { $".{formatName}" };
        FileBrowser.SetFilters(true, filters);
        FileBrowser.SetDefaultFilter(filters[0]);
        FileBrowser.ShowSaveDialog((paths) => { ExportClicked(paths[0]); }, () => Debug.Log($"Export canceled"), FileBrowser.PickMode.Files, allowMultiSelection: false, initialFilename: null, title: $"Export {formatName}");
        //FileBrowser.SetFilters(true);
        void ExportClicked(string path)
        {
            Debug.Log($"Export path chosen: {path}");
            Try.Action(ExportModel, path, $"export to {formatName} file");
        }
        void ExportModel(string path)
        {
            JsonHelpers.SaveToJsonFile(gridMeshGenerator.GetMeshFragmentVec3D(), path);
        }
    }
    public void ExportDecimatedMesh3DButtonClicked()
    {
        string formatName = "mesh3d12";
        var filters = new string[] { $".{formatName}" };
        FileBrowser.SetFilters(true, filters);
        FileBrowser.SetDefaultFilter(filters[0]);
        FileBrowser.ShowSaveDialog((paths) => { ExportClicked(paths[0]); }, () => Debug.Log($"Export canceled"), FileBrowser.PickMode.Files, allowMultiSelection: false, initialFilename: null, title: $"Export decimated {formatName}");
        //FileBrowser.SetFilters(true);
        void ExportClicked(string path)
        {
            Debug.Log($"Export path chosen: {path}");
            Try.Action(ExportModel, path, $"export to {formatName} file");
        }
        void ExportModel(string path)
        {
            JsonHelpers.SaveToJsonFile(gridMeshGenerator.GetDecimatedMeshFragmentVec3D(), path);
        }
    }

    public void ExportObjButtonClicked()
    {
        var filters = new string[] { ".obj" };
        FileBrowser.SetFilters(true, filters);
        FileBrowser.SetDefaultFilter(filters[0]);
        FileBrowser.ShowSaveDialog((paths) => { ExportClicked(paths[0]); }, () => Debug.Log($"Export canceled"), FileBrowser.PickMode.Files, allowMultiSelection: false, initialFilename: null, title: "Export obj");
        //FileBrowser.SetFilters(true);
        void ExportClicked(string path)
        {
            Debug.Log($"Export path chosen: {path}");
            Try.Action(ExportModel, path, "export to obj file");
        }
        void ExportModel(string path)
        {
            File.WriteAllText(path, gridMeshGenerator.GetDecimatedMeshFragmentVec3D().SerializeAsOBJFormatString());//JsonHelpers.SaveToJsonFile(deshelled, path);
        }
    }


}
