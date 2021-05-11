using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToken : MonoBehaviour
{
    public int myPlayerNum;
    public GameObject startSpace, mySpace;
    //float speed = 0.0f;

    void Start()
    {
        switch (myPlayerNum)
        {
            case 1:
                transform.position = startSpace.GetComponent<BoardSpace>().pos1.position;
                mySpace = startSpace;
                return;
            case 2:
                transform.position = startSpace.GetComponent<BoardSpace>().pos2.position;
                mySpace = startSpace;
                return;
            case 3:
                transform.position = startSpace.GetComponent<BoardSpace>().pos3.position;
                mySpace = startSpace;
                return;
            case 4:
                transform.position = startSpace.GetComponent<BoardSpace>().pos4.position;
                mySpace = startSpace;
                return;
            default:
                return;
        }
    }

    void Update()
    {
        //TESTING
        if (Input.GetKeyDown(KeyCode.Space) && mySpace.GetComponent<BoardSpace>().nextSpace != null)
            AdvanceSpace();

        if (Input.GetKeyDown(KeyCode.Backspace) && mySpace.GetComponent<BoardSpace>().prevSpace != null)
            RetreatSpace();
        
    }

    //Move forward to the next avalible space
    public void AdvanceSpace()
    {
        //speed += 1.1f * Time.deltaTime;

        switch (myPlayerNum)
        {
            case 1:
                //transform.position = Vector2.Lerp(mySpace.GetComponent<BoardSpace>().pos1.transform.position, mySpace.GetComponent<BoardSpace>().nextSpace.GetComponent<BoardSpace>().pos1.transform.position, speed);
                transform.position = mySpace.GetComponent<BoardSpace>().nextSpace.GetComponent<BoardSpace>().pos1.transform.position;
                mySpace = mySpace.GetComponent<BoardSpace>().nextSpace;
                transform.rotation = Quaternion.Euler(Vector3.up * mySpace.GetComponent<BoardSpace>().direction * 90);
                return;
            case 2:
                transform.position = mySpace.GetComponent<BoardSpace>().nextSpace.GetComponent<BoardSpace>().pos2.transform.position;
                mySpace = mySpace.GetComponent<BoardSpace>().nextSpace;
                return;
            case 3:
                transform.position = mySpace.GetComponent<BoardSpace>().nextSpace.GetComponent<BoardSpace>().pos3.transform.position;
                mySpace = mySpace.GetComponent<BoardSpace>().nextSpace;
                return;
            case 4:
                transform.position = mySpace.GetComponent<BoardSpace>().nextSpace.GetComponent<BoardSpace>().pos4.transform.position;
                mySpace = mySpace.GetComponent<BoardSpace>().nextSpace;
                return;
            default:
                return;
        }
    }

    //Move backward to the next avalible space
    public void RetreatSpace()
    {
        switch (myPlayerNum)
        {
            case 1:
                //transform.position = Vector2.Lerp(mySpace.GetComponent<BoardSpace>().pos1.transform.position, mySpace.GetComponent<BoardSpace>().nextSpace.GetComponent<BoardSpace>().pos1.transform.position, speed);
                transform.position = mySpace.GetComponent<BoardSpace>().prevSpace.GetComponent<BoardSpace>().pos1.transform.position;
                mySpace = mySpace.GetComponent<BoardSpace>().prevSpace;
                return;
            case 2:
                transform.position = mySpace.GetComponent<BoardSpace>().prevSpace.GetComponent<BoardSpace>().pos2.transform.position;
                mySpace = mySpace.GetComponent<BoardSpace>().prevSpace;
                return;
            case 3:
                transform.position = mySpace.GetComponent<BoardSpace>().prevSpace.GetComponent<BoardSpace>().pos3.transform.position;
                mySpace = mySpace.GetComponent<BoardSpace>().prevSpace;
                return;
            case 4:
                transform.position = mySpace.GetComponent<BoardSpace>().prevSpace.GetComponent<BoardSpace>().pos4.transform.position;
                mySpace = mySpace.GetComponent<BoardSpace>().prevSpace;
                return;
            default:
                return;
        }
    }

    public void SetMyLocation(BoardSpace newSpace)
    {
        mySpace = newSpace.gameObject;
        transform.position = mySpace.GetComponent<BoardSpace>().pos1.transform.position;
        transform.rotation = Quaternion.Euler(Vector3.up * mySpace.GetComponent<BoardSpace>().direction * 90);
    }
}
