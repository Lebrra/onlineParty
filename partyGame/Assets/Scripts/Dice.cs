using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour
{
    public Sprite image1, image2, image3;
    public static Dice inst;
    public int num;
    public Animator anim;

    // Start is called before the first frame update
    void Awake()
    {
        inst = this;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Roll()
    {
        anim.SetTrigger("Roll3");
    }

    public void GetNum(int rollNum)
    {
        this.num = rollNum;
    }

    public void PickRightNum()
    {
        switch (num)
        {
            case 1:
                GetComponent<Image>().sprite = image1;
                return;
            case 2:
                GetComponent<Image>().sprite = image2;
                return;
            case 3:
                GetComponent<Image>().sprite = image3;
                return;
        }
    }
}
