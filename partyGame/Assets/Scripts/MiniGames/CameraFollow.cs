using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public bool StartCam = false;
    public static CameraFollow inst;
    public float smoothSpeed = .3f;

    private void Start()
    {
        inst = this;
    }

    private void Update()
    {
        if (target.position.y > transform.position.y)
        {
            Vector3 newPos = new Vector3(transform.position.x, target.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, newPos, smoothSpeed);
        }
    }

    private void LateUpdate()
    {
        /*
        if(StartCam)
        {
            Vector3 newPos = new Vector3(transform.position.x, transform.position.y + .003f, transform.position.z);
            transform.position = newPos;
        }
        */
    }
}
