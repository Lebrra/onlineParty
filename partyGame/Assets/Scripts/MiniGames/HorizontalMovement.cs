using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMovement : MonoBehaviour
{
    Rigidbody rb;
    Collider c;

    public bool dead = false;
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
        if (!dead)
        {
            rb.isKinematic = false;
            Vector3 vel = rb.velocity;
            vel.x = movement;
            rb.velocity = vel;
        }
        else
        {
            rb.isKinematic = true;
        }
    }
}
