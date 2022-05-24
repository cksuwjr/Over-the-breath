using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConstraints : MonoBehaviour
{
    public bool JumpTwice = true;
    void Start()
    {
        Invoke("ApplyAll", 1f);
    }
    void ApplyAll()
    {
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        if (JumpTwice)
            Player.GetComponent<Move>().JumpMaxCount = 2;
        else
            Player.GetComponent<Move>().JumpMaxCount = 1;
    }
}
