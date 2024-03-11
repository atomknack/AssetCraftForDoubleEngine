using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoubleEngine;
using DoubleEngine.Atom;

public static class ComplimentaryMeshFragmentBuilder
{
    //public static MeshFragment Build(List<Vector2> vertices2d, List<(int v0, int v1, int v2)> faces)
    public static MeshFragmentVec3D Build(Vec2D[] vertices2d, List<(int v0, int v1, int v2)> faces)
    {
        //List<Vector3> vertices = VectorUtil.ConvertXYtoXYZ(vertices2d, -0.5f);
        Vec3D[] vertices = vertices2d.ConvertXYtoXYZArray(-0.5);
        //return new MeshBuilder(MeshFragment.CreateMeshFragment(vertices,faces)).Build();
        return new MeshBuilderVec3D(MeshFragmentVec3D.CreateMeshFragment(vertices, faces.ToArray())).Build();
    }
}
