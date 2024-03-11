using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load3DModed_NodeRotator : MonoBehaviour
{
    private Quaternion _rotation;
    // Start is called before the first frame update
    void Start()
    {
        LastLoaded3dMesh lastMesh = (LastLoaded3dMesh)FindObjectOfType(typeof(LastLoaded3dMesh));
        if (lastMesh != null)
            GetComponent<MeshFilter>().mesh = lastMesh.mesh;
        _rotation = transform.rotation;
    }

    public void RotateX90()
    {
        Quaternion rotation = Quaternion.Euler(90f,0,0);
        ApplyRotation(rotation);
    }
    public void RotateY90()
    {
        Quaternion rotation = Quaternion.Euler(0, 90, 0);
        ApplyRotation(rotation);
    }
    public void RotateZ90()
    {
        Quaternion rotation = Quaternion.Euler(0, 0, 90);
        ApplyRotation(rotation);
    }

    public void ApplyRotation(Quaternion addRotation)
    {
        //Quaternion oldRotation = transform.rotation;
        _rotation = addRotation * _rotation;
        transform.rotation = _rotation;//addRotation*oldRotation;
    }
}
