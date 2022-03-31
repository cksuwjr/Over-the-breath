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
        transform.Translate(new Vector3(0, target.GetComponent<SpriteRenderer>().size.y * 0.8f));
        StartCoroutine("GoUp");
    }
    IEnumerator GoUp()
    {
        Debug.Log("ÀÌµ¿");
        float distance = 0.2f;
        while (distance > 0)
        {
            distance -= Time.deltaTime / 3f;
            transform.Translate(new Vector3(0, Time.deltaTime / 3f));

            yield return null;
        }
        Destroy(gameObject);
    }
}
