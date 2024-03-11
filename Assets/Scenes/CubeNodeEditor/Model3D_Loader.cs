using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
using System.IO;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using SimpleFileBrowser;
//using static SimpleFileBrowser.FileBrowser;
//using Atom;
using DoubleEngine;
using DoubleEngine.Atom;
using System;

[RequireComponent(typeof(Model3D_FlatNodesDrawer))]
public class Model3D_Loader : MonoBehaviour
{
    //public AssetReference scene;
    //public DrawCollidedTriangle fragmentSource;
    //MeshFragment fragment;

    //public OldToDeleteGridElement def;

    private MeshFragmentVec3D _loaded;
    public GameObject PlaceholerForLoadedMesh;
    public GameObject TextPrefab;

    private Model3D_FlatNodesDrawer nodeDrawer;
    // Start is called before the first frame update

    public void OpenNewModel()
    {
        FileBrowser.ShowLoadDialog((paths) => { Debug.Log($"load path success: {paths[0]}"); LoadNewModel(paths[0], LoadMesh); }, () => Debug.Log($"load path canceled"), FileBrowser.PickMode.Files, allowMultiSelection: false, initialFilename: null, title: "Load", loadButtonText: "Select");
    }
    public void OpenNewModelAsDoubleRounded()
    {
        FileBrowser.ShowLoadDialog((paths) => { Debug.Log($"load path success: {paths[0]}"); LoadNewModel(paths[0], LoadMeshAsDoubleRounded); }, () => Debug.Log($"load path canceled"), FileBrowser.PickMode.Files, allowMultiSelection: false, initialFilename: null, title: "Load", loadButtonText: "Select");
    }

    public void LoadNewModel(string path, Action<Mesh> loader)
    {
        _loaded = null;
        GameObject newModel = UnityMeshImporter.MeshImporter.Load(path);
        newModel.SetActive(false);

        //newModel.transform.parent = PlaceholerForLoadedMesh.transform;
        Mesh m = newModel.GetComponentInChildren<MeshFilter>().sharedMesh;
        //LoadMesh(m);
        loader(m);
        Destroy(newModel);
    }

    public void LoadMeshAsDoubleRounded(Mesh m)
    {
        //var fragment = MeshFragmentVec3D.CreateMeshFragment(m.vertices.ToArrayVec3D().AsSpan().ToArrayTwiceRoundedVec3D(), m.triangles);
        var fragment = MeshFragmentVec3D.CreateMeshFragment(m.vertices.ToArrayTwiceRoundedVec3D(), m.triangles).JoinedClosestVertices();
        
        LoadMesh(fragment);
    }

    public void LoadMesh(Mesh m)
    {
        var fragment = MeshFragmentVec3D.CreateMeshFragment(m.vertices.ToArrayVec3D(), m.triangles);
        LoadMesh(fragment);
    }

    public void LoadMesh(MeshFragmentVec3D newMeshFragmentVec3D)
    {
        _loaded = newMeshFragmentVec3D;
        Mesh m = _loaded.ToNewUnityMesh();
        nodeDrawer.Clear();
        LastLoaded3dMesh lastMesh = (LastLoaded3dMesh)FindObjectOfType(typeof(LastLoaded3dMesh));
        if (lastMesh != null)
            lastMesh.mesh = m;

        PlaceholerForLoadedMesh.GetComponent<MeshFilter>().sharedMesh = m;
        MeshCollider collider = PlaceholerForLoadedMesh.GetComponent<MeshCollider>();
        collider.sharedMesh = m;

        //next parts is only for debug purposes
        // MeshFragmentVec3D baseMeshFragment = MeshFragmentVec3D.CreateMeshFragment(m.vertices.ToArrayVec3D(), m.triangles);
        if (_loaded != MeshFragmentVec3D.Empty)
        {
            Debug.Log($"Exporting loaded mesh as obj, just to check");
            File.WriteAllText(JsonHelpers.ApplicationDataPath + "loadedAsOBJ.obj", MeshFragmentVec3D.SerializeAsOBJFormatString(_loaded));
        }

        for (var i = 0; i < CubeSides.ToZNeg.Length; i++)
        {
            MeshFragmentVec3D rotated = _loaded.Rotated(CubeSides.ToZNeg[i]);//.ToMeshFragmentVec3D();
            if (rotated.TryMakeNotEmptyFragmentWhereAllVerticesIs((v => v.z <= -0.498f), out MeshFragmentVec3D outFlatNodeMesh3D))
            {
                nodeDrawer.DrawFlatNode(outFlatNodeMesh3D);
                if (FlatNodes.TrySlowlyFindWithSameVertexPositions(outFlatNodeMesh3D.To2D(), out int id, out var transfom))
                {
                }
                else
                {
                    NewFlatNodePlaceholder placeholderForNewFlatnode = (NewFlatNodePlaceholder)FindObjectOfType(typeof(NewFlatNodePlaceholder));
                    placeholderForNewFlatnode.fragment = outFlatNodeMesh3D;
                }

            }
            else
                nodeDrawer.NextOne();
        }
    }

    public void ExportIntactMesh()
    {
        if(_loaded is null)
        {
            Debug.Log("Cannot export null");
            return;
        }
        if (IntactCubeMesh.TryCreate(_loaded, out IntactCubeMesh cubeNode))
        {
            FileBrowser.ShowSaveDialog((paths) => { Debug.Log($"Export path success: {paths[0]}"); JsonHelpers.SaveToJsonFile(cubeNode, paths[0]); }, () => Debug.Log($"Export path canceled"), FileBrowser.PickMode.Files, allowMultiSelection: false, initialFilename: null, title: "Export intact mesh", saveButtonText: "Export");

            //JsonHelpers.SaveToJsonFile(cubeNode, filename);
            //Debug.Log($"saved {filename}");
        }
        else
        {
            Debug.Log("Cannot create IntactCubeNode");
        }

    }
    private void ExportIntactMesh(string filename)
    {

    }

    void Start()
    {
        nodeDrawer = GetComponent<Model3D_FlatNodesDrawer>();
        LastLoaded3dMesh lastMesh = (LastLoaded3dMesh)FindObjectOfType(typeof(LastLoaded3dMesh));
        if (lastMesh != null && lastMesh.mesh != null)
            LoadMesh(lastMesh.mesh);
        /*
        //string FlatNodes
        var json = JsonConvert.SerializeObject(transform.position);
        Debug.Log("Position as JSON: " + json);
        var ge = new GridElement();
        //ge.yPositive.enabled = true;
        //ge.yPositive.transformation = ge.yPositive.transformation.Rotate(PerpendicularAngle.a180);
        var jsonGE = JsonConvert.SerializeObject(ge);
        Debug.Log("GridElement as JSON: " + jsonGE);
        GridElement deserGE = (GridElement)JsonConvert.DeserializeObject(jsonGE,typeof(GridElement));
        def = deserGE;
        Debug.Log("GridElement as JSON: " + JsonConvert.SerializeObject(def));*/
    }

}
