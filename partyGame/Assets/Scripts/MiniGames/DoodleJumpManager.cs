using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoodleJumpManager : MinigameLoader
{
    public Transform target;
    public float totalDist = 0f;

    public override void StartGame()
    {
        base.StartGame();
        target.GetComponent<DoodleJumpControles>().dead = false;
    }

    void Update()
    {
        totalDist = Vector3.Distance(transform.position, target.transform.position);
    }
}
