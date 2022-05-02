using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBox : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "Ground")
            GetComponentInParent<Move>().CanJump();
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.transform.tag == "Ground")
            GetComponentInParent<Move>().CantJump();
    }
}
