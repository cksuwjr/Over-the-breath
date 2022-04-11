using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportCalculator : MonoBehaviour
{
    GameObject Player;
    GameObject Effect;
    Rigidbody2D rb;

    float direction;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player").gameObject;
        StartCoroutine("Move", Player.GetComponent<SpriteRenderer>().flipX);
        Effect = Resources.Load("Prefab/Player_iron_Skill1Effect") as GameObject;
        StartCoroutine("MakeShadowEffect");
        
    }
    private void Update()
    {
        FrontWallCheck();
    }
    IEnumerator Move(bool dir)
    {
        if (dir)
        {
            direction = -1;
            rb.velocity = new Vector2(-15, 0);
        }
        else
        {
            direction = 1;
            rb.velocity = new Vector2(15, 0);
        }
        yield return new WaitForSeconds(0.3f);
        Player.GetComponent<Skill>().TeleportByCalcul(transform.localPosition);
        Destroy(gameObject);
    }
    IEnumerator MakeShadowEffect()
    {
        Instantiate(Effect, transform.localPosition, Quaternion.identity);
        yield return new WaitForSeconds(0.0425f);
        StartCoroutine("MakeShadowEffect");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Wall")
        {
            Player.GetComponent<Skill>().TeleportByCalcul(transform.localPosition);
            Destroy(gameObject);
        }
    }
    void FrontWallCheck()
    {
        // 전방에 벽이 사라져 벽뜷기를 방지
        //Debug.DrawRay(transform.position, new Vector3(Input.GetAxisRaw("Horizontal") * 0.5f, 0, 0), new Color(0, 1, 0));
        RaycastHit2D rayFrontWallCheck = Physics2D.Raycast(transform.position, new Vector3(direction, 0, 0), 0.5f, (1 << LayerMask.NameToLayer("UnPassableWall")) + (1 << LayerMask.NameToLayer("Wall")));
        if (rayFrontWallCheck.collider != null)
        {
            Player.GetComponent<Skill>().TeleportByCalcul(transform.localPosition);
            Destroy(gameObject);
        }
    }
}
