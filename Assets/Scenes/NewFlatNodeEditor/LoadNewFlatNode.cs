using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoubleEngine;
using DoubleEngine.Atom;
using System;

[RequireComponent(typeof(MeshFilter))]
public class LoadNewFlatNode : MonoBehaviour
{
    public FlatNode flatnode;
    private FlatNodeEditorStartFragment startMeshFragment;
    private MeshFilter _mesh;

    //private MeshFragment _fragment;
    private bool _initOK = false;
    //private FlatNodeTransform flatTransform = FlatNodeTransform.Default;

    public SetLabelText currentTransformLabel;
    public SetLabelText savedTransformLabel;
    private FlatNodeTransform? savedTransform;
    //private PerpendicularAngle angle = PerpendicularAngle.a0;
    // Start is called before the first frame update
    void Start()
    {
        _mesh = GetComponent<MeshFilter>();
        startMeshFragment = (FlatNodeEditorStartFragment)FindObjectOfType(typeof(FlatNodeEditorStartFragment));
        if (startMeshFragment != null && startMeshFragment.Fragment!= null//)// && startMeshFragment.Fragment.Vertices!=null)
                && startMeshFragment.Fragment.Vertices.Length>0 && startMeshFragment.Fragment.Triangles.Length>0)
        {
            _initOK = true;
            flatnode = new FlatNode(0, startMeshFragment.Fragment.To2D());
            //_fragment = startMeshFragment.Fragment;
            UpdateMesh();
        }
        else throw new Exception("startMeshFragment is null or empty");
    }

    public void SaveFlatTransform() {
        savedTransform = flatnode.flatTransform;
        if (savedTransformLabel != null)
        {
            savedTransformLabel.SetText(savedTransform.ToString());
        }
    }
    public void ApplyFlatTransform()
    {
        //if(savedTransform != null)
        //    flatTransform = flatTransform.Transform(savedTransform.Value);
        if (savedTransform != null)
            flatnode = flatnode.TransformedByFlatNodeTransform(savedTransform.Value);
        UpdateMesh();
    }

    public void Rotate90()
    {
        Debug.Log("Rotate90");
        //flatTransform = flatTransform.Rotate(PerpendicularAngle.a90);
        flatnode = flatnode.TransformedByFlatNodeTransform(new FlatNodeTransform(PerpendicularAngle.a90));
        UpdateMesh();
    }


    public void RotateMinus90()
    {
        Debug.Log("RotateMinus90");
        //flatTransform = flatTransform.Rotate(PerpendicularAngle.aNegative90);
        flatnode = flatnode.TransformedByFlatNodeTransform(new FlatNodeTransform(PerpendicularAngle.aNegative90));
        UpdateMesh();
    }

    public void InvertX()
    {
        Debug.Log("InvertX");
        //flatTransform = flatTransform.InvertX();
        flatnode = flatnode.TransformedByFlatNodeTransform(new FlatNodeTransform().InvertX());
        UpdateMesh();
    }
    public void InvertY()
    {
        Debug.Log("InvertY");
        //flatTransform = flatTransform.InvertY();
        flatnode = flatnode.TransformedByFlatNodeTransform(new FlatNodeTransform().InvertY());
        UpdateMesh();
    }

    private void UpdateMesh()
    {
        if (_initOK)
        {
            if (currentTransformLabel != null)
                currentTransformLabel.SetText(flatnode.flatTransform.ToString());
            //Debug.Log($"init OK, updating, {flatTransform.rotation.Float()}");
            //MeshFragment temp = _fragment.Scaled(new Vector3(flatTransform.inverted ? -1 : 1, 1, 1));
            //temp = temp.Rotated(Quaternion.Euler(0, 0, flatTransform.rotation.Float()));
            //_mesh.sharedMesh = (Mesh)temp;
            _mesh.sharedMesh = flatnode.Transformed.ToNewUnityMesh();//.ToMeshFragmentVector3().ToUnityMesh();
        }
    }
}
