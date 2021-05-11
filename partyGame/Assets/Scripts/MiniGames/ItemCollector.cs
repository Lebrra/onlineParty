using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemCollector : MonoBehaviour
{
    public int itemsCollected = 0;
    public int badItemsCollected = 0;
    public static ItemCollector inst;
    public TextMeshProUGUI scoreText;

    private void Start()
    {
        inst = this;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            if (collision.gameObject.GetComponent<Item>().myType == Item.itemType.good)
            {
                itemsCollected++;
                Destroy(collision.gameObject);
            }
            else if (collision.gameObject.GetComponent<Item>().myType == Item.itemType.bad)
            {
                itemsCollected--;
                Destroy(collision.gameObject);
            }
        }
    }

    private void Update()
    {
        //scoreText.text = "Final Score: " + itemsCollected.ToString();
    }
}
