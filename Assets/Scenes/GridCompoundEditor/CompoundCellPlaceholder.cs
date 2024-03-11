using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoubleEngine;
using DoubleEngine.Atom;
using System;

public class CompoundCellPlaceholder : MonoBehaviour
{
    public GridMeshGenerator gridMesh;

    int x = 0;
    int y = 0;
    int z = 0;

    byte materialIndex;

    int meshIndex = 1;
    IThreeDimensionalGrid[] brushes;
    Grid6SidesCached orientation;
    // Start is called before the first frame update
    Transform emptySpaceCube;
    MeshFilter meshFilter;
    Mesh meshFilterDefaultCube;
    void Start()
    {
        Debug.Log(JsonHelpers.ApplicationDataPath);
        int numberOfBrushes = 8;
        brushes = new IThreeDimensionalGrid[numberOfBrushes];
        for (int i = 0; i < brushes.Length; i++)
        {
            brushes[i] = ThreeDimensionalGridLayeredBuilder.Create(10, 10, 10);
            GridLoaders.LoadGrid(brushes[i], JsonHelpers.ApplicationDataPath + $"/Brushes/{i}.bg10");
        }

        materialIndex = DEMaterials.DefaultMaterial.id;//GridMaterials.DefaultMaterial;
        GetComponent<Renderer>().sharedMaterial = DEMaterials.GetUnityMaterial(materialIndex);// GridMaterials.Instance.GetUnityMaterial(materialIndex);
        orientation = Grid6SidesCached.Default;
        if (gridMesh == null)
            gridMesh = FindObjectOfType<GridMeshGenerator>();
        meshFilter = GetComponent<MeshFilter>();
        meshFilterDefaultCube = meshFilter.sharedMesh;
        emptySpaceCube = transform.GetChild(0);
    }
    public void RotateAroundZ()=> UpdateOrientation(orientation.RotateZ90());
    public void RotateAroundX() => UpdateOrientation(orientation.RotateX90());
    public void RotateAroundY() => UpdateOrientation(orientation.RotateY90());
    public void InvertZAxis() => UpdateOrientation(orientation.InvertZ());
    public void InvertXAxis() => UpdateOrientation(orientation.InvertX());
    public void InvertYAxis() => UpdateOrientation(orientation.InvertY());

    private void UpdateOrientation(Grid6SidesCached newOrientation)
    {
        orientation = newOrientation;
        UpdateOrientation();
    }
    private void UpdateOrientation()
    {
        transform.rotation = orientation._rotation.ToQuaternion();
        transform.localScale = new Vector3(1, orientation._invertedY ? -1 : 1, 1);
        transform.position = new Vector3(x, y, z);
    }

    public void NextMaterial()
    {
        ChangeMaterial(DEMaterials.NextMaterialId(materialIndex));
    }

    private void ChangeMaterial(byte newMaterialIndex)
    {
        materialIndex = newMaterialIndex;
        GetComponent<Renderer>().sharedMaterial = DEMaterials.GetUnityMaterial(materialIndex);
    }

    private void NextMesh()
    {
        //Debug.Log("TODO change");
        //ChangeMeshIndex(1);//meshIndex.NextIntCyclic(ThreeDimensionalCellMeshes.GetCount()));
        ChangeMeshIndex(meshIndex.NextIntCyclic(brushes.Length));
    }
    private void ChangeMeshIndex(int newIndex)
    {
        meshIndex = newIndex;
        if (meshIndex == 0)
        {
            meshFilter.sharedMesh = null;
            emptySpaceCube.gameObject.SetActive(true);
        }
        else
        {
            meshFilter.sharedMesh = brushes[meshIndex].BuildMesh().ToNewUnityMesh();//ThreeDimensionalCellMeshes.GetUnityMesh((short)meshIndex);
            //meshFilter.sharedMesh = meshFilterDefaultCube;
            emptySpaceCube.gameObject.SetActive(false);
        }
            
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            NextMaterial();
        if (Input.GetKeyDown(KeyCode.Keypad8))
            z += 1;
        if (Input.GetKeyDown(KeyCode.Keypad2))
            z -= 1;
        if (Input.GetKeyDown(KeyCode.Keypad6))
            x += 1;
        if (Input.GetKeyDown(KeyCode.Keypad4))
            x -= 1;
        if (Input.GetKeyDown(KeyCode.Keypad7))
            y += 1;
        if (Input.GetKeyDown(KeyCode.Keypad1))
            y -= 1;
        if (Input.GetKeyDown(KeyCode.Keypad9))
            NextMesh();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (var newCell in brushes[meshIndex].Transform(x, y, z, orientation.OrientationIndex()))
            {
                gridMesh.Put(newCell.pos.x, newCell.pos.y, newCell.pos.z, newCell.cell);
            }
            //gridMesh.Put(x, y, z, ThreeDimensionalCell.Create((short)meshIndex, orientation.OrientationIndex(), materialIndex));
        }

        UpdateOrientation();
    }
}
