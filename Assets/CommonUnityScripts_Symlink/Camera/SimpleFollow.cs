using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollow : MonoBehaviour
{
    public Transform target;
    public float rotateSpeed = 5.0f;
    public float _rotation = 0.0f;
    private Vector3 _initialPosition;
    void Start()
    {
        _initialPosition = transform.position;
    }
    void LateUpdate()
    {
        //transform.LookAt(target);
        //transform.Translate(Vector3.right * Time.deltaTime);
        transform.position = target.position + _initialPosition; //Vector3.Lerp(transform.position, target.position + _initialPosition,Time.deltaTime * smooth);
        
        /*if (Input.GetKey(KeyCode.E))
        {
            _rotation += rotateSpeed;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            _rotation -= rotateSpeed;
        }

        transform.RotateAround(target.position, Vector3.up, _rotation * Time.deltaTime);
        */
    }
}
