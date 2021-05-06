using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSpace : MonoBehaviour
{
    public static BoardSpace inst;

    public GameObject mySpace, nextSpace, prevSpace;
    public Transform pos1, pos2, pos3, pos4;

    [Tooltip("Forward = 0\nLeft = -1\nRight = 1\nBackward = 2")]
    public int direction;

    private void Start()
    {
        inst = this;
    }
}
