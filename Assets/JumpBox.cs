using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBox : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        try
        {
            if (col.transform.tag == "Ground")
                GetComponentInParent<Move>().CanJump();
        }
        catch { }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        try
        {
            if (col.transform.tag == "Ground")
                GetComponentInParent<Move>().CantJump();
        }
        catch { }
    }
}
