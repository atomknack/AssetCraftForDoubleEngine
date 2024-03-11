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
public class ColoredImporter : MonoBehaviour
{
    [SerializeField]
    private MeshFilter _filter;
    private Button _importMesh3DButton;
    private VisualElement _rootUI;


    private void OnEnable()
    {
        if (_filter == null)
        {
            Debug.Log("No filter placeholder selected in script");
            throw new MissingComponentException(nameof(_filter));
        }
        _rootUI = GetComponent<UIDocument>().rootVisualElement;

        _importMesh3DButton = _rootUI.Q<Button>("ImportMesh3d");
        _importMesh3DButton.RegisterCallback<ClickEvent>(ev => ImportMesh3DButtonClicked());

    }

    private void OnDisable()
    {
        _importMesh3DButton?.UnregisterCallback<ClickEvent>(ev => ImportMesh3DButtonClicked());
    }

    public void ImportMesh3DButtonClicked()
    {
        string formatName = "mesh3d12";
        var filters = new string[] { $".{formatName}" };
        FileBrowser.SetFilters(true, filters);
        FileBrowser.SetDefaultFilter(filters[0]);
        FileBrowser.ShowLoadDialog((paths) => { ImportClicked(paths[0]); }, () => Debug.Log($"Import canceled"), FileBrowser.PickMode.Files, allowMultiSelection: false, initialFilename: null, title: $"Import colored mesh {formatName}");
        //FileBrowser.SetFilters(true);
        void ImportClicked(string path)
        {
            Debug.Log($"Import path chosen: {path}");
            Try.Action(ImportModel, path, $"import from {formatName} file");
        }
        void ImportModel(string path)
        {
            var mesh = JsonHelpers.LoadFromJsonFile<MeshFragmentVec3DWithMaterials>(path);
            if (_filter.sharedMesh == null)
                _filter.sharedMesh = new Mesh();
            mesh.UpdateUnityMesh(_filter.sharedMesh);
        }
    }



}
