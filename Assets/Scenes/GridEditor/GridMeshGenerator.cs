using DoubleEngine.Atom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoubleEngine;
using DoubleEngine.Atom;
using System;

public class GridMeshGenerator : MonoBehaviour
{
    private MeshFilter _meshFilter;
    private ThreeDimensionalGridLayeredBuilder _grid;
    public int xSize = 10;
    public int ySize = 4;
    public int zSize = 8;
    //private MeshFragmentVec3D _meshFragmentVec3D;
    private MeshFragmentVec3DWithMaterials _coloredMesh;
    private MeshFragmentVec3DWithMaterials _coloredDecimated;

    public MeshFragmentVec3DWithMaterials GetMeshFragmentVec3D()
    {
        if (_coloredMesh == null)
            UpdateMesh();
        return _coloredMesh;
    }
    public MeshFragmentVec3DWithMaterials GetDecimatedMeshFragmentVec3D()
    {
        if (_coloredDecimated == null)
            UpdateMesh();
        return _coloredDecimated;
    }
    public IThreeDimensionalGrid GetIGridReference() => _grid;

    public void Put(int x, int y, int z, ThreeDimensionalCell cell)
    {
        _grid.UpdateCell(x,y,z,cell);
        UpdateMesh();
    }

    public void UpdateMesh()
    {
        _grid.BuildMesh();
        _coloredMesh =  _grid.BuildMeshWithMaterials();
        _coloredDecimated = DecimatorColored.Decimate(_coloredMesh);
        _coloredDecimated.UpdateUnityMesh(_meshFilter.sharedMesh);//, GridMaterials.Instance.GetMaterialColors());
        //_meshFragmentVec3D = _coloredMesh.fragment;//BuildMesh();
        /*if (_meshFilter.sharedMesh == null)
        {
            _meshFilter.sharedMesh = _meshFragmentVec3D.ToNewUnityMesh();
            return;
        }*/
        //_meshFilter.sharedMesh = _meshFragmentVec3D.ToNewUnityMesh();



    }
    void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshFilter.sharedMesh = new Mesh();
        _grid = ThreeDimensionalGridLayeredBuilder.Create(xSize, ySize, zSize);

    }

    //private MeshFragmentVec3D BuildMesh() => Decimator3D.Decimate(_grid.BuildMesh());

}
