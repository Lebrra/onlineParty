using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSuperiorityManager : MinigameLoader
{
    public bool dead = false;
    public GameObject particle;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            dead = true;
            Instantiate(particle, collision.gameObject.transform.position, Quaternion.identity);
            collision.gameObject.SetActive(false);
        }
    }
}
