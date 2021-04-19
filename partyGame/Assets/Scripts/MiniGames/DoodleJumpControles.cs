using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoodleJumpControles : MonoBehaviour
{
    Rigidbody rb;
    Collider c;

    public float moveSpeed;
    float movement = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        c = GetComponent<Collider>();
    }

    void Update()
    {
        movement = Input.GetAxis("Horizontal") * moveSpeed;
    }

    private void FixedUpdate()
    {
        Vector3 vel = rb.velocity;
        vel.x = movement;
        rb.velocity = vel;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            Physics.IgnoreCollision(c, other, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            Physics.IgnoreCollision(c, other, false);
        }
    }
}
