using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoodleJumpControles : MonoBehaviour
{
    Rigidbody rb;
    Collider c;

    public bool dead = false;
    public bool falling = false;
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

        if(rb.velocity.y < 0)
            falling = true;
        else
            falling = false;

        if (falling)
            c.enabled = true;
        else
            c.enabled = false;
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
