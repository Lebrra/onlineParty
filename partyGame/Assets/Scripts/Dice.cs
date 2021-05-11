using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour
{
    public static Dice inst;
    public Animator anim;
    public GameObject dieScreen;
    bool rolling = false;

    void Awake()
    {
        inst = this;
        anim = GetComponent<Animator>();
    }

    public void PickRightNum(int rollNum)
    {
        /*
        switch (rollNum)
        {
            case 1:
                anim.SetTrigger("Roll1");
                return;
            case 2:
                anim.SetTrigger("Roll2");
                return;
            case 3:
                anim.SetTrigger("Roll3");
                return;
        }
        */
        if(!rolling)
            StartCoroutine("PickRightDieWaitTimer", rollNum);
    }

    public void ResetDie()
    {
        //StartCoroutine("DiceResetCountdown");
        anim.SetTrigger("ResetDie");
        dieScreen.SetActive(false);
        rolling = false;
    }

    public IEnumerator DiceResetCountdown()
    {
        yield return new WaitForSeconds(1f);
        //dieScreen.SetActive(false);
        anim.SetTrigger("ResetDie");
        dieScreen.SetActive(false);
        rolling = false;
        //GameManager.inst.EndTurn();
    }

    
    public IEnumerator PickRightDieWaitTimer(int rollNum)
    {
        rolling = true;
        yield return new WaitForSeconds(.5f);

        switch (rollNum)
        {
            case 1:
                anim.SetTrigger("Roll1");
                break;
            case 2:
                anim.SetTrigger("Roll2");
                break;
            case 3:
                anim.SetTrigger("Roll3");
                break;
            default:
                break;
        }
    }
    
}
