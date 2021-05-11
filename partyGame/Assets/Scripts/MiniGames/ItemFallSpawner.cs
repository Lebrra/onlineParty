using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFallSpawner : MinigameLoader
{
    public Transform spawn;
    public GameObject goodItem, badItem;

    public int difficulty = 1;
    public float gameDuration = 15f;
    public bool gameState = false;

    public override void StartGame()
    {
        base.StartGame();
        StartCoroutine("DropTime", 3);
        minigameState = true;
    }

    void Update()
    {
        if (minigameState)
        {
            ItemCollector.inst.GetComponent<DoodleJumpControles>().dead = false;
            gameDuration -= Time.deltaTime;

            if (gameDuration < 0)
            {
                minigameState = false;
                SendFinalScore(ItemCollector.inst.itemsCollected.ToString());
            }
        }
        else if (!minigameState)
        {
            //StopCoroutine("DropTime");
            //ItemCollector.inst.GetComponent<DoodleJumpControles>().enabled = false;
            ItemCollector.inst.GetComponent<DoodleJumpControles>().dead = true;
        }
    }

    public IEnumerator DropTime(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (minigameState)
        {
            DropItem(difficulty);
        }
        StartCoroutine("DropTime", 1.5);
    }

    public void DropItem(int level)
    {
        int rand = Random.Range(1, level+1);

        float randLoc = Random.Range(7f, -7f);
        Vector2 spawnLoc = new Vector2(randLoc, transform.position.y);

        switch (difficulty)
        {
            case 1:
                if(rand == 1)
                {
                    Instantiate(goodItem, spawnLoc, Quaternion.identity);
                }
                return;

            case 2:
                if (rand == 1)
                {
                    Instantiate(goodItem, spawnLoc, Quaternion.identity);
                }
                else if (rand == 2)
                {
                    Instantiate(badItem, spawnLoc, Quaternion.identity);
                }
                return;

            default:
                return;
        }
    }
}
