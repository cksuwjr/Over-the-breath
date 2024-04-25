using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DamageUI : MonoBehaviour
{
    RectTransform rt;
    void Start()
    {
        rt = GetComponent<RectTransform>();
    }
    public void Spawn(int Damage, Vector3 position)
    {
        GetComponent<Text>().text = Damage.ToString();
        transform.position = position;

        StartCoroutine("GoUp");
    }
    public void Spawn(int[] Damage, Vector3 position)
    {
        transform.position = position;
        StartCoroutine(ShowDamages(Damage));
        StartCoroutine("GoUp");
    }
    IEnumerator ShowDamages(int[] damages)
    {
        int count = 0;
        string texts = "";
        var textComponent = GetComponent<Text>();
        while(count < damages.Length)
        {
            texts += damages[count++].ToString() + "\n";

            textComponent.text = texts;
            yield return new WaitForSeconds(0.01f);
        }
        yield return null;
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
