using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public bool StartCam = false;
    public static CameraFollow inst;

    private void Start()
    {
        inst = this;
    }

    /*
    public IEnumerator MoveCamera()
    {

    }
    */

    private void LateUpdate()
    {
        if(StartCam)
        {
            Vector3 newPos = new Vector3(transform.position.x, transform.position.y + .003f, transform.position.z);
            transform.position = newPos;
        }
    }
}
