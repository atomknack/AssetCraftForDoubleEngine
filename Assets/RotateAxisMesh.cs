using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoubleEngine;
using DoubleEngine.Atom;

[RequireComponent(typeof(MeshFilter))]
public class RotateAxisMesh : MonoBehaviour
{
    public MeshFilter meshFilterForNewMesh;
    public void CreateMeshCopyAndRotate()
    {

        var builder = new MeshBuilderVec3D(GetComponent<MeshFilter>().mesh.vertices.ToArrayVec3D(), GetComponent<MeshFilter>().mesh.triangles);

        builder.AddRotation(AngleMethods.ZNegToXPos);

        var fragment = builder.Build();

        var mesh = new Mesh();
        mesh.vertices = fragment.Vertices.ToArrayVector3();// ToArray();
        mesh.triangles = fragment.Triangles.ToArray();

        meshFilterForNewMesh.sharedMesh = mesh;
    }
}
