using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageUI : MonoBehaviour
{
    RectTransform rt;
    void Start()
    {
        rt = GetComponent<RectTransform>();
    }
    public void Spawn(int Damage, GameObject target)
    {
        GetComponent<Text>().text = Damage.ToString();
        transform.localPosition = new Vector2(0, target.GetComponent<CapsuleCollider2D>().size.y);
        StartCoroutine("GoUp");
    }
    IEnumerator GoUp()
    {
        float distance = 0.2f;
        while (distance > 0)
        {
            distance -= Time.deltaTime / 3f;
            transform.Translate(new Vector3(0, Time.deltaTime / 3f));

            yield return null;
        }
        Destroy(transform.parent.gameObject);
    }
}
