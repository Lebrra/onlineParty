using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public List<GameObject> platformPool;
    public GameObject platformToPool;
    public int poolAmount;

    void Start()
    {
        platformPool = new List<GameObject>();
        for (int i = 0; i < poolAmount; i++)
        {
            GameObject platform = (GameObject)Instantiate(platformToPool);
            platform.SetActive(false);
            platformPool.Add(platform);
        }

        PlatformSetup();
    }

    void Update()
    {
        
    }

    public GameObject GetPooledPlatform()
    {
        for (int i = 0; i <platformPool.Count; i++)
        {
            if (!platformPool[i].activeInHierarchy)
                return platformPool[i];
        }
        return null;
    }

    public void PlatformSetup()
    {
        float num = 1f;
        foreach(GameObject p in platformPool)
        {
            int randX = Random.Range(-6, 7);
            p.transform.position = new Vector3(randX, num, 0);
            p.SetActive(true);
            num += 2f;
        }
    }
}

/*
  GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject(); 
  if (bullet != null) {
    bullet.transform.position = turret.transform.position;
    bullet.transform.rotation = turret.transform.rotation;
    bullet.SetActive(true);
  }
 */