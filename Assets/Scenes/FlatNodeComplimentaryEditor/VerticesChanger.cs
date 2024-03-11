using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoubleEngine;
using DoubleEngine.Atom;

public class VerticesChanger : MonoBehaviour
{
    public VerticeDrawerClicker Drawer;
    public GameObject sealingMeshPlaceholder;
    public Material splitterVerticeBaseMaterial;
    private FlatNode _baseNode;
    private FlatNode _complimentary;


    private readonly Vec2DCloseEnoughSet _vectorsSet = Vec2DCloseEnoughSet.NewHashSet();
    private readonly List<Vec2D> _vector2s = new();

    private readonly List<(int v0, int v1, int v2)> _createdFaces = new();
    private readonly Vec2DCloseEnoughSet _splitterVertices = Vec2DCloseEnoughSet.NewHashSet();

    void OnEnable()
    {
        if (Drawer == null)
            Drawer = FindObjectOfType<VerticeDrawerClicker>();
    }
    private void UpdateSealingMesh()
    {
        ClearSealingMesh();
        MeshFilter sealingMeshFilter = sealingMeshPlaceholder.GetComponent<MeshFilter>();
        sealingMeshFilter.sharedMesh = (Mesh)ComplimentaryMeshFragmentBuilder.Build(_vector2s.ToArray(), _createdFaces).ToNewUnityMesh();//.ToMeshFragmentVector3().ToUnityMesh();
    }
    private void ClearSealingMesh()
    {
        MeshFilter sealedMeshFilter = sealingMeshPlaceholder.GetComponent<MeshFilter>();
        if (sealedMeshFilter.sharedMesh != null)
            Destroy(sealedMeshFilter.sharedMesh);
        sealedMeshFilter.sharedMesh = null;
    }

    void AddCreatedFace(int v0Index, int v1Index, int v2Index)
    {
        if (TriVec2D.IsTriangleVisible(_vector2s[v0Index], _vector2s[v1Index], _vector2s[v2Index]))
        {
            //Debug.Log("Triangle visible (Clockwise)");
            _createdFaces.Add((v0Index, v1Index, v2Index));
        }
        else
        {
            //Debug.Log("Triangle is not visible (Counter clockwise) - need reversing");
            _createdFaces.Add((v2Index, v1Index, v0Index));
        }
        UpdateSealingMesh();
    }

    void SelectedVerticesChanged(int[] verticesIds)
    {
        Debug.Log("SelectedVerticesChanged called");
        if (verticesIds.Length>2)
        {
            AddCreatedFace(verticesIds[0], verticesIds[1], verticesIds[2]);
            Drawer.UnSelectAll();
        }
    }
    public void AddVertice(Vec2D v2, ClickableVertice.ClickDelegate _1, Material unselectedMaterial = null, string gameobjectName = null)
    {
        if (!_vectorsSet.Contains(v2))
            AddVerticeAnyway(v2, _1, unselectedMaterial, gameobjectName);
    }
    public void AddSplitterVertice(Vec2D v2, ClickableVertice.ClickDelegate _1, Material unselectedMaterial = null, string gameobjectName = null)
    {
        if (!_vectorsSet.Contains(v2))// && (!_splitterVertices.Contains(v2)))
        {
            _splitterVertices.Add(v2);
            AddVerticeAnyway(v2, _1, unselectedMaterial, gameobjectName);
        }
    }
    private void AddVerticeAnyway(Vec2D v2, ClickableVertice.ClickDelegate _1, Material unselectedMaterial, string gameobjectName)
    {
        Drawer.AddVertice((Vector3)v2.ToVector2(), _1, _vector2s.Count, unselectedMaterial, gameobjectName);
        _vectorsSet.Add(v2);
        _vector2s.Add(v2);
    }
    public void UpdateFlatNodes(FlatNode newBaseNode, FlatNode newComplimentary)
    {
        if (newBaseNode != _baseNode || newComplimentary != _complimentary)
        {
            CleanFlatNodes();
            Drawer.ClearAndRemoveAll();
            Drawer.OnSelectionChange = SelectedVerticesChanged;

            _baseNode = newBaseNode;
            _complimentary = newComplimentary;

            MeshFragmentVec3D newBaseNodeFragment = newBaseNode.Transformed.JoinedClosestVertices(1e-05f);
            Vec2D[] newBaseVertices2D = newBaseNodeFragment.Vertices.ConvertXYZtoXYArray();
            EdgeIndexed[] singleEdgesBaseNode = newBaseNode.singleEdges;//MeshUtil.SingleEdgesToArray(MeshUtil.SingleEdges(newBaseNodeFragment.triangles));
            //for (int i = 0; i< singleEdgesBaseNode.Length;i+=2) Debug.Log($"{singleEdgesBaseNode[i]} {singleEdgesBaseNode[i+1]} singleEdges");
            foreach (var vertice in newBaseVertices2D)
            {
                AddVertice(vertice, null, null, "baseNodeVertice"); //new Vector2(vertice.x, vertice.y)
            }

            MeshFragmentVec3D newComplimentaryFragment = newComplimentary.Transformed.JoinedClosestVertices(1e-05f);
            Vec2D[] newComplimentaryVertices2D = newComplimentaryFragment.Vertices.ConvertXYZtoXYArray();

            foreach (var vertice in newComplimentaryVertices2D)
            {
                if (EdgeVec2D.IsVerticeOnEdges(newBaseNodeFragment.Vertices.ConvertXYZtoXYArray(), singleEdgesBaseNode, vertice, out EdgeIndexed edgeContainingVec2D))
                {
                    //Debug.Log($"Have splitter_: {vertice}");
                    AddSplitterVertice(vertice, null, splitterVerticeBaseMaterial, "ComplimentaryOnBaseNodeEdge"); //new Vector2(vertice.x, vertice.y)
                }
            }
            //var interceptionVertices = SingleEdges2DIntersections(ConvertXYZtoXY(newBaseNodeFragment.vertices.ToArrayVector3()), newBaseNodeFragment.triangles,
            //     ConvertXYZtoXY(newComplimentaryFragment.vertices.ToArrayVector3()), newComplimentaryFragment.triangles);
            var interceptionVertices = CollisionDiscrete2D.SingleEdges2DIntersections(newBaseVertices2D, newBaseNodeFragment.Triangles,//
                 newComplimentaryVertices2D, newComplimentaryFragment.Triangles);
            foreach (var v2 in interceptionVertices)
             {
                AddSplitterVertice(v2, null, splitterVerticeBaseMaterial, "InterceptionVertice");
             }

            foreach (var v2 in newComplimentaryVertices2D)
            {
                AddVertice(v2, null, null,"complimentaryNodeVertice");
            }
        }
    }

    public void CleanFlatNodes()
    {
        Drawer.ClearAndRemoveAll();
        _vectorsSet.Clear();
        _vector2s.Clear();
        _createdFaces.Clear();
        _splitterVertices.Clear();
        ClearSealingMesh();
    }
}
