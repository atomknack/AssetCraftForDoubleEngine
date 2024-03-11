using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public static partial class MeshUtil
{
    public static int[] SingleEdgesToArray((int start, int end)[] singleEdges)
    {
        int[] result = new int[singleEdges.Length*2];
        for (int i = 0; i < singleEdges.Length; i++)
        {
            result[(i*2)] = singleEdges[i].start;
            result[(i*2)+1] = singleEdges[i].end;
        }
        return result;
    }
    public static (int start, int end)[] SingleEdges(IReadOnlyList<int> triangles)
    {
        Dictionary<(int start, int end), int> edgesCount = new();

        for (var i = 0; i < triangles.Count; i += 3)
        {
            AddEdge(triangles[i], triangles[i + 1]);
            AddEdge(triangles[i+1], triangles[i + 2]);
            AddEdge(triangles[i + 2], triangles[i]);
        }
        //foreach (var keyValue in edgesCount)
        //    Debug.Log($"{keyValue.Key.a} {keyValue.Key.b} {keyValue.Value}");
        var filtered = edgesCount.Where(keyvaluePair => keyvaluePair.Value == 1); // make single lined
        var onlyKeys = filtered.Select(keyvaluePair => keyvaluePair.Key); // make single lined
        return onlyKeys.ToArray(); // make single lined

        void AddEdge(int start, int end)
        {
            var edge = (start, end);
            if (edgesCount.ContainsKey(edge))
            {
                edgesCount[edge]++;
                return;
            }
            var edgeBackwards = (end, start);
            if (edgesCount.ContainsKey(edgeBackwards))
            {
                edgesCount[edgeBackwards]++;
                return;
            }
            edgesCount.Add(edge, 1);
        }

    }

    public static (List<Vector3>, List<int>) RemoveUnusedFacesAndVertices(IList<int> _triangles, IList<Vector3> _vertexes, IEnumerable<int> newMeshFaceIdexes)
    {
        List<int> newVerticesIndexes = new List<int>();
        Dictionary<int, int> oldNewVerticesMap = new Dictionary<int, int>();
        int newVerticesCount = 0;
        List<int> newTriangles = new List<int>();
        if (newMeshFaceIdexes != null)
            foreach (int faceIndex in newMeshFaceIdexes)
            {
                AddTriangleVertice(_triangles[faceIndex * 3 + 0]);
                AddTriangleVertice(_triangles[faceIndex * 3 + 1]);
                AddTriangleVertice(_triangles[faceIndex * 3 + 2]);
            }
        else
            foreach (var verticeIndex in _triangles)
                AddTriangleVertice(verticeIndex);

        return (RemapVertices(), newTriangles);

        void AddTriangleVertice(int oldVerticeIndex)
        {
            if (oldNewVerticesMap.ContainsKey(oldVerticeIndex) == false)
            {
                newVerticesIndexes.Add(oldVerticeIndex);
                oldNewVerticesMap.Add(oldVerticeIndex, newVerticesCount);
                newVerticesCount++;
            }
            newTriangles.Add(oldNewVerticesMap[oldVerticeIndex]);
        }

        List<Vector3> RemapVertices()
        {
            List<Vector3> newVertices = new List<Vector3>(newVerticesIndexes.Count);
            foreach (int oldplace in newVerticesIndexes)
                newVertices.Add(_vertexes[oldplace]);
            return newVertices;
        }
    }

    public static HashSet<int> SelectVertices(this Vector3[] vertices, Func<Vector3, bool> s)
    {
        if (vertices == null)
            throw new ArgumentNullException("vertices[] is null");
        HashSet<int> set = new HashSet<int>();
        for (var i = 0; i < vertices.Length; i++)
        {
            if (s(vertices[i]))
                set.Add(i);
        }
        return set;
    }

    public static List<int> SelectFaces(this int[] triangles, HashSet<int> verticesIndexes)
    {
        if (triangles == null)
            throw new ArgumentNullException("triangles[] is null");
        if (verticesIndexes == null)
            throw new ArgumentNullException("verticesIndexes[] is null");
        List<int> list = new List<int>();
        var f = 0;
        for (var i = 0; i < triangles.Length; i += 3) 
        {
            if (verticesIndexes.Contains(triangles[i]) && verticesIndexes.Contains(triangles[i + 1]) && verticesIndexes.Contains(triangles[i + 2]))
                {
                list.Add(f);
                //list.Add(triangles[i]);
                //list.Add(triangles[i + 1]);
                //list.Add(triangles[i + 2]);
                }
            f++;
            }
        return list;
    }
    //Not Tryed, Not Tested. TODO: Try, Test
    /*public static int[] FindFacesWithAllVertices(HashSet<int> verticesIndexes, int[] triangles)
    {
        List<int> faces = new List<int>();
        for(var i=0; i < triangles.Length; i += 3)
            if( verticesIndexes.Contains(triangles[i]) && verticesIndexes.Contains(triangles[i+1]) && verticesIndexes.Contains(triangles[i+2]) )
                faces.Add(i);
        return faces.ToArray();
    }*/

    //manual Tested, looks working, TODO: Test more, automate tests
    public static (int[] remapedVerticeIndexes, int[] newTriangles) CreatePartialMesh(int[] triangles, int[] newMeshFaceIdexes = null)
    {
        if (triangles.Length % 3 != 0)
            throw new ArgumentException("Every triangle must contain 3 vertices");
        List<int> newVertices = new List<int>();
        Dictionary<int, int> oldNewVerticesMap = new Dictionary<int, int>();
        int newVerticesCount = 0;
        List<int> newTriangles = new List<int>();
        if (newMeshFaceIdexes != null)
            foreach (int faceIndex in newMeshFaceIdexes)
            {
                AddTriangleVertice(triangles[faceIndex * 3 + 0]);
                AddTriangleVertice(triangles[faceIndex * 3 + 1]);
                AddTriangleVertice(triangles[faceIndex * 3 + 2]);
            }
        else
            for (var vertIndex = 0; vertIndex < triangles.Length; vertIndex++)
                AddTriangleVertice(triangles[vertIndex]);

        return (newVertices.ToArray(), newTriangles.ToArray());

        void AddTriangleVertice(int oldVerticeIndex)
        {
            if (oldNewVerticesMap.ContainsKey(oldVerticeIndex) == false)
            {
                newVertices.Add(oldVerticeIndex);
                oldNewVerticesMap.Add(oldVerticeIndex, newVerticesCount);
                newVerticesCount++;
            }
            newTriangles.Add(oldNewVerticesMap[oldVerticeIndex]);
        }
    }

    //manual Tested, looks working, TODO: Test more, automate tests
    public static TVertice[] remapVertices<TVertice>(TVertice[] oldVertices, int[] vericesToNewArray) where TVertice : struct
    {
        TVertice[] newVertices = new TVertice[vericesToNewArray.Length];
        for (int i = 0; i < newVertices.Length; i++)
        {
            newVertices[i] = oldVertices[vericesToNewArray[i]];
        }
        return newVertices;
    }

    public static int[] remapTriangles(int[] oldTriangles, int[] newVerticeIndexes)
    {
        int[] newTriangles = new int[oldTriangles.Length];
        for(int i= 0; i< oldTriangles.Length; i++)
        {
            newTriangles[i] = newVerticeIndexes[oldTriangles[i]];
        }
        return newTriangles;
    }

    /*
    public static Vector3[] Scaled(this Vector3[] vertices, Vector3 scale) => vertices.Select(v => v.Scaled(scale)).ToArray();
    public static Vector3[] Rotated(this Vector3[] vertices, Quaternion rotation) => vertices.Select(v => v.Rotated(rotation)).ToArray();
    public static Vector3[] Translated(this Vector3[] vertices, Vector3 translation) => vertices.Select(v => v.Translated(translation)).ToArray();
    */

    public static void ApplyNormalsRotation(Vector3[] normals, Quaternion rotation) //?
    {
        for (int i = 0; i < normals.Length; ++i)
            normals[i] = rotation * normals[i];
        //InPlaceApplyScaleRotationTranslation(normals, scale: Vector3.one);
    }


    /* Pavel: previous, but maybe it overcomplicated?
   private delegate Vector3 VerticeOperation(Vector3 v);
   public static void ApplyScaleRotationTranslation(ref Vector3[] vertices, ref Vector3[] normals,
                                       Vector3? newScale = null, Quaternion? newRotation = null, Vector3? newTranslation = null)
   {
       Vector3 scale;
       Quaternion rotation;
       Vector3 translation;
       VerticeOperation verticeOperations = null;
       if (newScale != null) 
       {
           scale = (Vector3)newScale;
           verticeOperations += ApplyScale;
       }
       if (newRotation != null)
       {
           rotation = (Quaternion)newRotation;
           verticeOperations += ApplyRotation;
           if (normals != null)
               ApplyNormals(normals);
       }
       if (newTranslation != null)
       {
           translation = (Vector3)newTranslation;
           verticeOperations += ApplyTranslation;
       }
       if (vertices != null && verticeOperations != null)
           ApplyVertices(vertices);

       void ApplyVertices(Vector3[] vertices)
       {
           for (int i = 0; i < vertices.Length; ++i)
           {
               vertices[i] = verticeOperations(vertices[i]);
           }
       }
       void ApplyNormals(Vector3[] normals)
       {
           for (int i = 0; i < normals.Length; ++i)
           {
               normals[i] = ApplyRotation(normals[i]);
           }
       }
       Vector3 ApplyScale(Vector3 v) => new Vector3(v.x + scale.x, v.y + scale.y, v.z + scale.z);
       Vector3 ApplyRotation(Vector3 v) => rotation * v;
       Vector3 ApplyTranslation(Vector3 v) => v + translation;
   }
   */



    /*
    https://github.com/mariosubspace/mesh-apply-transform-unity/blob/master/Assets/Gigableh/Mesh%20Apply%20Transform/Scripts/Editor/MeshApplyTransform.cs
    // Apply a transform to a mesh. The transform needs to be
    // reset also after this application to keep the same shape.
    public static Mesh ApplyTransform(
        Transform transform,
        Mesh mesh,
        bool applyTranslation,
        bool applyRotation,
        bool applyScale)
    {
        var verts = mesh.vertices;
        var norms = mesh.normals;

        // Handle vertices.
        for (int i = 0; i < verts.Length; ++i)
        {
            var nvert = verts[i];

            if (applyScale)
            {
                var scale = transform.localScale;
                nvert.x *= scale.x;
                nvert.y *= scale.y;
                nvert.z *= scale.z;
            }

            if (applyRotation)
            {
                nvert = transform.rotation * nvert;
            }

            if (applyTranslation)
            {
                nvert += transform.position;
            }

            verts[i] = nvert;
        }

        // Handle normals.
        for (int i = 0; i < verts.Length; ++i)
        {
            var nnorm = norms[i];

            if (applyRotation)
            {
                nnorm = transform.rotation * nnorm;
            }

            norms[i] = nnorm;
        }

        mesh.vertices = verts;
        mesh.normals = norms;

        mesh.RecalculateBounds();
        mesh.RecalculateTangents();

        return mesh;
    }
    */
}

