using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSpace : MonoBehaviour
{
    public static BoardSpace inst;

    public GameObject mySpace, nextSpace, prevSpace;
    public Transform pos1, pos2, pos3, pos4;

    private void Start()
    {
        inst = this;
    }
}
