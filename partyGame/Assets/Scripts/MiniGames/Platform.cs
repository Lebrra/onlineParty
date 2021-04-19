using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float jumpForce;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.relativeVelocity.y <= 0f)
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if(rb != null)
            {
                Vector3 vel = rb.velocity;
                vel.y = jumpForce;
                rb.velocity = vel;
            }
        }
    }
}
