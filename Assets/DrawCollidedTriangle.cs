using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DoubleEngine;
using DoubleEngine.Atom;


public class DrawCollidedTriangle : MonoBehaviour
{
    public Mesh clickedMesh;
    public Transform clickedMeshTransform;
    public List<int> clickedTriangles = new List<int>();

    public NewFlatNodePlaceholder placeholder;

    //public Vector3[] DebugNewMeshVertices;
    //public int[] DebugNewMeshTriangles;


    public void CreateNewMeshFromClickedTriangles()
    {

        //Vec3D v = new Vec3D(0.8,1,0);
        //Debug.Log($"Vec3D {v}");


        var builder = new MeshBuilderVec3D(clickedMesh.vertices.ToArrayVec3D(), clickedMesh.triangles);
        builder.OnlyUseFaces(clickedTriangles.ToArray());
        //builder.OnlyUseFaces(new int[] { 0, 1, 2, 3, 4 });
        //builder.RemoveFaces(clickedTriangles.ToArray());

        //(int[] remapedVertexIndexes, int[] newMeshTriangles) = MeshUtil.CreatePartialMesh(clickedMesh.triangles, clickedTriangles.ToArray());
        //Vector3[] newMeshVertices = MeshUtil.remapVertices(clickedMesh.vertices, remapedVertexIndexes);

        var scale = clickedMeshTransform.localScale;
        //var rotation = clickedMeshTransform.localRotation;
        //builder.ApplyScaleRotationTranslation( scale: scale, rotation: rotation);
        
        builder.AddScale(scale.ToVec3D());
        builder.AddRotation(ChangeCamera.rotations[ChangeCamera._cameraIndex]);
        //builder.AddTranslation(new Vector3(2,0,0));

        /*if (fragment == null)
            Debug.Log($"Mesh fragment is null: {fragment}");
        else
            Debug.Log($"Mesh fragment: vertices {fragment.vertices}, triangles {fragment.triangles}");
        */

        placeholder.fragment = builder.Build();//.ToMeshFragmentVec3D();

        /*if (fragment == null)
            Debug.Log($"Mesh fragment is null: {fragment}");
        else
            Debug.Log($"Mesh fragment: vertices {fragment.vertices}, triangles {fragment.triangles}");
        */


        /*DebugNewMeshVertices = mesh.vertices;
        DebugNewMeshTriangles = mesh.triangles;

        Debug.Log(0.56622885888511233135576443);
        Debug.Log((float)0.56622885888511233135576443);
        Debug.Log((double)(float)0.56622885888511233135576443);*/
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider is MeshCollider)
            {
                var (v0, v1, v2) = HitTriangleVertices(hit);
                DrawTriangle(v0,v1,v2,hit.transform);

                CheckClick(hit.collider as MeshCollider, hit.triangleIndex);
            }
            else Debug.Log("Raycast hit not a MeshCollider");
        }

        DrawClickedTriangles();

    }

    void DrawClickedTriangles()
    {
        foreach (int triangle in clickedTriangles)
        {
            var (v0, v1, v2) = TriangleVertices(clickedMesh, triangle);
            DrawTriangle(v0, v1, v2, clickedMeshTransform,Color.white, Color.white, Color.white);
        }
    }

    void CheckClick(MeshCollider collider, int triangleIndex)
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (collider.sharedMesh != clickedMesh)
                ReplaceMesh(collider.transform, collider.sharedMesh);
            AddTriangleToList(triangleIndex);
        }    
    }

    void ReplaceMesh(Transform from, Mesh mesh)
    {
        clickedMeshTransform = from;
        ReplaceMesh(mesh);
    }
    void ReplaceMesh(Mesh mesh)
    {
        clickedMesh = mesh;
        clickedTriangles = new List<int>();
    }    

    void AddTriangleToList(int triangle)
    {
        if (clickedTriangles.Contains(triangle) == false)
            clickedTriangles.Add(triangle);
    }

    public void DrawTriangle(Vector3 v0, Vector3 v1, Vector3 v2, Transform t,
 Color? side0Color = null, Color? side1Color = null, Color? side2Color = null)
    {
        var tv0 = t.TransformPoint(v0);
        var tv1 = t.TransformPoint(v1);
        var tv2 = t.TransformPoint(v2);
        DebugLine.DrawLine(tv0, tv1, side0Color ?? Color.red);
        DebugLine.DrawLine(tv1, tv2, side0Color ?? Color.green);
        DebugLine.DrawLine(tv2, tv0, side0Color ?? Color.blue);
    }

    public void DrawTriangle(Vector3 v0, Vector3 v1, Vector3 v2,
    Color? side0Color = null, Color? side1Color = null, Color? side2Color = null)
    {
        DebugLine.DrawLine(v0, v1, side0Color ?? Color.red);
        DebugLine.DrawLine(v1, v2, side0Color ?? Color.green);
        DebugLine.DrawLine(v2, v0, side0Color ?? Color.blue);
    }


    public (Vector3 v0, Vector3 v1, Vector3 v2) HitTriangleVertices(RaycastHit hit)
    {
        if (hit.collider is not MeshCollider)
            throw new System.ArgumentException("To return triangle vertices collider must be MeshCollider");
        Mesh mesh = ((MeshCollider)hit.collider).sharedMesh;
        return TriangleVertices(mesh, hit.triangleIndex);
    }

    public (Vector3 v0, Vector3 v1, Vector3 v2) TriangleVertices(Mesh mesh, int triangleIndex)
    {
        int[] triangles = mesh.triangles;
        Vector3[] vertices = mesh.vertices;
        return (
                vertices[triangles[triangleIndex * 3 + 0]],
                vertices[triangles[triangleIndex * 3 + 1]],
                vertices[triangles[triangleIndex * 3 + 2]]
                );
    }

}
