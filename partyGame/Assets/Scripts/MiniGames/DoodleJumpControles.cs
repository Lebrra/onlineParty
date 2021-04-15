using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoodleJumpControles : MonoBehaviour
{
    Rigidbody2D rb;

    public float moveSpeed;
    float movement = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement = Input.GetAxis("Horizontal") * moveSpeed;
    }

    private void FixedUpdate()
    {
        Vector2 vel = rb.velocity;
        vel.x = movement;
        rb.velocity = vel;
    }
}
