using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoodleJumpManager : MonoBehaviour
{
    public Transform target;
    public float totalDist = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        totalDist = Vector3.Distance(transform.position, target.transform.position);
    }
}
