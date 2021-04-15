using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFallSpawner : MonoBehaviour
{
    public Transform spawn;
    public GameObject goodItem, badItem;

    public int difficulty = 1;

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            StartCoroutine("DropTime", 3);
    }

    public IEnumerator DropTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        DropItem(difficulty);
        StartCoroutine("DropTime", 3);
    }

    public void DropItem(int level)
    {
        int rand = Random.Range(1, level+1);

        float randLoc = Random.Range(4f, -4f);
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
