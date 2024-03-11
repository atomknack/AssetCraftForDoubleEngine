﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class KeysOrbitImproved : MonoBehaviour
{

    public Transform target;

    public KeyCode Left = KeyCode.A;
    public KeyCode Right = KeyCode.D;
    public KeyCode Up = KeyCode.W;
    public KeyCode Down = KeyCode.S;

    public float distance = 5.0f;
    public float distanceStep = 5.0f;
    public float xSpeed = 100f;
    public float ySpeed = 100f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;

    public float flyOffdistance = 4.0f;
    public float flyOffSpeed = 1.0f;

    private Rigidbody _rigidbody;

    float x = 0.0f;
    float y = 0.0f;

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        _rigidbody = GetComponent<Rigidbody>();

        //P commented
        // Make the rigid body not change rotation
        //if (_rigidbody != null)
        //{
        //    _rigidbody.freezeRotation = true;
        //}
    }

    void LateUpdate()
    {
        if (target)
        {
            if(Input.GetKey(Left))
                x += xSpeed * distance * Time.deltaTime;
            if (Input.GetKey(Right))
                x -= xSpeed * distance * Time.deltaTime;
            //x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
            if (Input.GetKey(Up))
                y += ySpeed * distance * Time.deltaTime;
            if (Input.GetKey(Down))
                y -= ySpeed * distance * Time.deltaTime;
            //y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            y = ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);

            if (distance< flyOffdistance)
            {
                RaycastHit hitback;
                if (! Physics.Linecast(transform.position,transform.position-(Vector3.forward* (flyOffSpeed * Time.deltaTime *2)), out hitback))
                {
                    //distance -= hitback.distance; //TODO: use real distance to calsulate how far camera can move
                    distance += flyOffSpeed * Time.deltaTime; //P: slowly fly off from tracked object
                }

            }

            distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * distanceStep, distanceMin, distanceMax);

            RaycastHit hit;
            if (Physics.Linecast(target.position, transform.position, out hit))
            {
                distance -= distance - hit.distance;
            }
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position;

            transform.rotation = rotation;
            transform.position = position;
        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F; //P possible bug should be MOD of angle be used?
        if (angle > 360F)
            angle -= 360F; //P possible bug should be MOD of angle be used?
        return Mathf.Clamp(angle, min, max);
    }
}
