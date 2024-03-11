using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DoubleEngine;
using DoubleEngine.Atom;
using SimpleFileBrowser;

public class PureElementRotator : MonoBehaviour
{
    GridMesh pureCompositeGridElement;
    public bool invertedY;
    public QuaternionD rotation;
    public bool[] skipSides = { true, false, false, false, false, false };

    void Start()
    {
        LastLoaded3dMesh lastMesh = (LastLoaded3dMesh)FindObjectOfType(typeof(LastLoaded3dMesh));
        LoadGridElement(lastMesh.mesh);
        UpdateMeshFromGridElement();
    }

    public void GridMeshToJSon()
    {
        //FileBrowser.SetDefaultFilter(".json");
        FileBrowser.ShowSaveDialog(
            (paths) => { SaveToJson(paths[0]); },
            () => Debug.Log($"load path canceled"),
            FileBrowser.PickMode.Files,
            allowMultiSelection: false,
            initialFilename: null,
            title: "Save",
            saveButtonText: "Save"
            );

    }
    private void SaveToJson(string path)
    {
        MeshFragmentVec3D meshVec3D = pureCompositeGridElement.BaseMeshFragment();
        Debug.Log($"save file path success: {path}"); 
        JsonHelpers.SaveToJsonFile<GridMesh>(pureCompositeGridElement, path);

        JsonHelpers.SaveToJsonFile(pureCompositeGridElement.GetCoreMesh(false, false, false, false, false, false).Vertices.ToArray(),
            path + "_verticesCoreMesh.json");
        JsonHelpers.SaveToJsonFile(pureCompositeGridElement.GetCoreMesh(false, false, false, false, false, false).Vertices.ToArrayTwiceRoundedVec3D(),
            path + "_verticesCoreMeshTwiceRounded.json");

        JsonHelpers.SaveToJsonFile(meshVec3D.Vertices.ToArray(),
            path + "_BaseMeshFragment.json");
        Vec3D[] twiceRoundedVec3D = meshVec3D.Vertices.ToArrayTwiceRoundedVec3D();
        JsonHelpers.SaveToJsonFile(twiceRoundedVec3D,
            path + "_BaseMeshFragmentTwiceRounded.json");


        JsonHelpers.SaveToJsonFile(MeshFragmentVec3D.CreateMeshFragment(twiceRoundedVec3D, meshVec3D.Triangles.ToArray()),
            path + "_NOTjoinedTwiceRoundedVec3DMesh.json");

        MeshFragmentVec3D joined = meshVec3D.JoinedClosestVertices();
        Vec3D[] joinedVec3DVertices = joined.Vertices.ToArrayTwiceRoundedVec3D();
        MeshFragmentVec3D joinedVec3DMesh = MeshFragmentVec3D.CreateMeshFragment(joinedVec3DVertices, joined.Triangles.ToArray());
            JsonHelpers.SaveToJsonFile(joinedVec3DMesh,
                path + "_joinedTwiceRoundedVec3DMesh.json");
    }

    public void CycleSkipSide()
    {
        skipSides = skipSides.CyclicLeftShift().ToArray();
        UpdateMeshFromGridElement();
    }

    public void GridElementInvertX()
    {
        pureCompositeGridElement.InvertX();
        UpdateMeshFromGridElement();
    }

    public void GridElementInvertY()
    {
        pureCompositeGridElement.InvertY();
        UpdateMeshFromGridElement();
    }

    public void GridElementInvertZ()
    {
        pureCompositeGridElement.InvertZ();
        UpdateMeshFromGridElement();
    }

    public void GridElementRotateY90()
    {
        pureCompositeGridElement.RotateY90();
        UpdateMeshFromGridElement();
    }

    public void GridElementRotateX90()
    {
        pureCompositeGridElement.RotateX90();
        UpdateMeshFromGridElement();
    }

    public void GridElementRotateZ90()
    {
        pureCompositeGridElement.RotateZ90();
        UpdateMeshFromGridElement();
    }

    void UpdateMeshFromGridElement()
    {
        MeshFragmentVec3D recreated = GridMesh.BuildMeshFragmentFromSides(pureCompositeGridElement.GridElementSides());
        GetComponent<MeshFilter>().mesh = recreated.ToNewUnityMesh();

        GameObject pureElementCoreMesh =  GameObject.Find("PureElementCoreMesh");
        MeshFragmentVec3D coreFragment = pureCompositeGridElement.GetCoreMesh( skipSides[0], skipSides[1], skipSides[2], skipSides[3], skipSides[4], skipSides[5]);
        pureElementCoreMesh.GetComponent<MeshFilter>().mesh = coreFragment.ToNewUnityMesh();

        invertedY = pureCompositeGridElement.ge._invertedY;
        rotation = pureCompositeGridElement.ge._rotation;
        //pureElementCoreMesh.transform.rotation = rot;

        GameObject pureElementComplement = GameObject.Find("PureElementComplement");
        MeshFragmentVec3D complement = GridMesh.BuildMeshFragmentFromSides(pureCompositeGridElement.GridElementComplementSides());
        pureElementComplement.GetComponent<MeshFilter>().mesh = complement.ToNewUnityMesh();
    }

    public void SetOther3dModelRotationFromQuaternion()
    {
        GameObject other3d = GameObject.Find("3d Model");
        other3d.transform.rotation = pureCompositeGridElement.ge._rotation.ToQuaternion();
        other3d.transform.localScale = new Vector3(1,pureCompositeGridElement.ge._invertedY? -1: 1,1);
    }

    public void LoadGridElement(Mesh m)
    {
        MeshFragmentVec3D fragment = MeshFragmentVec3D.CreateMeshFragment(m.vertices.ToArrayVec3D(), m.triangles);
        FlatNode[] sides = new FlatNode[6];
        int[][] facesToRemove = new int[6][];
        for (var i = 0; i <6; i++)
        {
            sides[i] = null;
            MeshFragmentVec3D rotated = fragment.Rotated(CubeSides.ToZNeg[i]);//.ToMeshFragmentVec3D();
            if (rotated.TryMakeNotEmptyFragmentWhereAllVerticesIs((v => v.z <= -0.498f), out MeshFragmentVec3D outFlatNode))
            {
                if (FlatNodes.TrySlowlyFindWithSameVertexPositions(outFlatNode.To2D(), out int id, out var transfom))
                {
                    if (FlatNodes.TryFind(id, out FlatNode foundFlatNode))
                        sides[i] = foundFlatNode.TransformedByFlatNodeTransform(transfom);
                }
            }
            facesToRemove[i] = rotated.SelectFacesWhereAllVerticesIs((v => v.z <= -0.498f)).ToArray();

        }
        pureCompositeGridElement = new GridMesh(fragment, sides, facesToRemove);
    }

}
