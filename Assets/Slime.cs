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
        {   // ���� �־�߸� ��?
            Transform parentTransform = transform.parent.GetComponent<Transform>();
            Transform colTransform = col.transform;
            if (NeedSwamp)
            {   //  �� �� �����κ��� ���Դ�?
                if (MySwamp != null && col.GetComponentInChildren<Slime>().MySwamp != null)
                {   // ���� �� ����̾�?
                    if (MySwamp == col.GetComponentInChildren<Slime>().MySwamp)
                    {
                        // ���� ������ Ŀ?
                        if (parentTransform.localScale.x >= colTransform.localScale.x && parentTransform.localScale.y >= colTransform.localScale.y)
                        {
                            // ������ ���, �� ���� �Ҹ���
                            MySwamp.GetComponent<Swamp>().KillOther(col.gameObject);

                            Status mystat = transform.parent.GetComponent<Status>();

                            mystat.MaxHp += col.GetComponent<Status>().MaxHp;
                            mystat.HP += col.GetComponent<Status>().HP;
                            mystat.AttackPower += col.GetComponent<Status>().AttackPower;

                            parentTransform.localScale = new Vector2(parentTransform.localScale.x + colTransform.localScale.x * (2f / 7f), parentTransform.localScale.y + colTransform.localScale.y * (2f / 7f));

                            //col.GetComponent<EnemyAI2>().Die(transform.parent.gameObject);
                        }
                    }
                }
            }
            else // ���� ��� ũ�⸸ ũ�� ��� ������
            {   // �°� �� ����̾�?
                if(col.GetComponentInChildren<Slime>().MySwamp != null)
                    col.GetComponentInChildren<Slime>().MySwamp.GetComponent<Swamp>().KillOther(col.gameObject);

                if (parentTransform.localScale.x >= colTransform.localScale.x && parentTransform.localScale.y >= colTransform.localScale.y)
                {
                    Status mystat = transform.parent.GetComponent<Status>();

                    mystat.MaxHp += col.GetComponent<Status>().MaxHp;
                    mystat.HP += col.GetComponent<Status>().HP;
                    mystat.AttackPower += col.GetComponent<Status>().AttackPower;

                    parentTransform.localScale = new Vector2(parentTransform.localScale.x + colTransform.localScale.x * (1f / 10f), parentTransform.localScale.y + colTransform.localScale.y * (1f / 10f));

                    //col.GetComponent<EnemyAI2>().Die(transform.parent.gameObject);
                }
            }
        }
    }
}
