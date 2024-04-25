using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;

    GameObject attacker;

    Vector3 direction;
    public float damage;
    bool damaged = false;
    bool stackable = false;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }
    public void Init(GameObject attacker, Vector3 direction, float damage, float zRotation = 0, float speed = 15f, float duration = 0.3f, bool stackable = false)
    {
        damaged = false;

        this.attacker = attacker;
        this.direction = direction;
        this.damage = damage;
        this.stackable = stackable;
        transform.rotation = Quaternion.Euler(0, 0, zRotation);
        rb.velocity = direction * speed;

        Invoke("SetActiveFalse", duration);
    }
    void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (damaged) return;

        if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "Neutrality")
        {
            if (col.GetComponent<Monster>().die) return;
            damaged = true;
            FireEffect(col.transform.position);
            col.GetComponent<Monster>().GetDamaged(damage, attacker, stackable);
            CancelInvoke("SetActiveFalse");
            gameObject.SetActive(false);
        }
    }
    void FireEffect(Vector3 pos)
    {
        var effect = PoolManager.Instance.Get(10);
        float scale;
        if (damage < 100)
            scale = damage * 0.005f;
        else if (damage < 1000)
            scale = 0.5f + damage * 0.0003f;
        else
            scale = 1f;

        if (scale <= 0.5f)
            scale = 0.5f;
        else if (scale >= 1f)
            scale = 1f;
        effect.transform.localScale = new Vector3(scale, scale, 1);
        effect.transform.position = pos;
    }
}
