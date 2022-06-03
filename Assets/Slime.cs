using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public List<string> AbsorbableTarget;
    public bool NeedSwamp = true;
    public GameObject MySwamp;
    private void OnTriggerEnter2D(Collider2D col)
    {
        bool Absorbable = false;
        foreach (string target in AbsorbableTarget) {
            if (col.name == target)
                Absorbable = true;
        }
        if(Absorbable && col.gameObject != transform.parent.gameObject)
        {   // 늪이 있어야만 해?
            Transform parentTransform = transform.parent.GetComponent<Transform>();
            Transform colTransform = col.transform;
            if (NeedSwamp)
            {   //  둘 다 늪으로부터 나왔니?
                if (MySwamp != null && col.GetComponentInChildren<Slime>().MySwamp != null)
                {   // 같은 늪 출신이야?
                    if (MySwamp == col.GetComponentInChildren<Slime>().MySwamp)
                    {
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
            else // 늪이 없어도 크기만 크면 흡수 가능해
            {   // 걔가 늪 출신이야?
                if(col.GetComponentInChildren<Slime>().MySwamp != null)
                    col.GetComponentInChildren<Slime>().MySwamp.GetComponent<Swamp>().KillOther(col.gameObject);

                if (parentTransform.localScale.x >= colTransform.localScale.x && parentTransform.localScale.y >= colTransform.localScale.y)
                {
                    Status mystat = transform.parent.GetComponent<Status>();

                    mystat.MaxHp += col.GetComponent<Status>().MaxHp;
                    mystat.HP += col.GetComponent<Status>().HP;
                    mystat.AttackPower += col.GetComponent<Status>().AttackPower;

                    parentTransform.localScale = new Vector2(parentTransform.localScale.x + colTransform.localScale.x * (1f / 10f), parentTransform.localScale.y + colTransform.localScale.y * (1f / 10f));

                    col.GetComponent<EnemyAI2>().Die(transform.parent.gameObject);
                }
            }
        }
    }
}
