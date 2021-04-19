﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    public int itemsCollected = 0;
    public int badItemsCollected = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Item>().myType == Item.itemType.good)
        {
            itemsCollected++;
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.GetComponent<Item>().myType == Item.itemType.bad)
        {
            badItemsCollected++;
            Destroy(collision.gameObject);
        }
    }
}