using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public GameObject MySwamp;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.name == "Slime(Clone)" && col.gameObject != transform.parent.gameObject && MySwamp != null)
        {
            if (MySwamp.GetComponent<Swamp>().King == transform.parent.gameObject)
            {
                Transform parentTransform = transform.parent.GetComponent<Transform>();
                Transform colTransform = col.transform;
                // 내가 쟤보다 커?
                if (parentTransform.localScale.x >= colTransform.localScale.x && parentTransform.localScale.y >= colTransform.localScale.y)
                {
                    // 슬라임 흡수, 내 몸집 불리기
                    MySwamp.GetComponent<Swamp>().KillOther(col.gameObject);

                    Status mystat = transform.parent.GetComponent<Status>();

                    mystat.MaxHp += col.GetComponent<Status>().MaxHp;
                    mystat.HP += col.GetComponent<Status>().HP;
                    mystat.AttackPower += col.GetComponent<Status>().AttackPower;

                    parentTransform.localScale = new Vector2(parentTransform.localScale.x + colTransform.localScale.x * (2f / 7f), parentTransform.localScale.y + colTransform.localScale.y * (2f / 7f));

                    col.GetComponent<EnemyAI2>().Die(transform.parent.gameObject);
                }
            }
        }
    }
}
