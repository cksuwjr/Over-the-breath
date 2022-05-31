using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public GameObject MySwamp;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.name == "Slime" && col.gameObject != transform.parent.gameObject)
        {
            if (MySwamp.GetComponent<Swamp>().King == transform.parent.gameObject)
            {
                // ½½¶óÀÓ Èí¼ö, ³» ¸öÁý ºÒ¸®±â
                MySwamp.GetComponent<Swamp>().KillOther(col.gameObject);

                Status mystat = transform.parent.GetComponent<Status>();

                mystat.MaxHp += col.GetComponent<Status>().MaxHp;
                mystat.HP += col.GetComponent<Status>().HP;
                mystat.AttackPower += col.GetComponent<Status>().AttackPower;

                Transform parentTransform = transform.parent.GetComponent<Transform>();
                parentTransform.localScale = new Vector2(parentTransform.localScale.x + 0.7f, parentTransform.localScale.y + 0.7f);

                col.GetComponent<EnemyAI2>().Die(transform.parent.gameObject);
                Debug.Log("²¨¾ï~");
            }
        }
    }
}
