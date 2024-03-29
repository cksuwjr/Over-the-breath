using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Move : MonoBehaviour
{
    Rigidbody2D rb;     
    SpriteRenderer sr;  
    Animator anim;
    UIManager officialUI;

    Status stat;   
    Skill skill;

    int JumpCount = 2;
    public int JumpMaxCount = 2;
    public bool isWall;
    float direction;




    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        officialUI = GameObject.Find("UI").GetComponent<UIManager>();

        stat = GetComponent<Status>();
        skill = GetComponent<Skill>();
    }

    void Update()
    {
        if (skill.isMumchit || officialUI.isStoryTelling)
            rb.velocity = new Vector2(0, rb.velocity.y);
        else
            PlayerKeyboardInput();

        FrontWallCheck();

        FallSpeedLimit();

    }
    public void FallSpeedLimit()
    {
        if(rb.velocity.y < -15)
            rb.velocity = new Vector2(rb.velocity.x, -15);
    }
    public void SceneChangeUIChanged()
    {
        officialUI = GameObject.Find("UI").GetComponent<UIManager>();
    }

    // Player 키보드 입력 (움직임)
    void PlayerKeyboardInput()
    {
        // 키보드 입력!1
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            float direction = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * stat.MoveSpeed, rb.velocity.y);
            if (direction == -1)
                sr.flipX = true;
            else if (direction == 1)
                sr.flipX = false;

            anim.speed = stat.MoveSpeed / stat.BasicSpeed;
            anim.SetBool("Idle", false);
            anim.SetBool("Walk", true);
        }
        else
        {
            if (Mathf.Abs(rb.velocity.x) <= stat.MoveSpeed || !skill.isMumchit)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                anim.SetBool("Idle", true);
                anim.SetBool("Walk", false);
            }
        }


        // 점프!!
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (JumpCount > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(new Vector2(0, 2) * stat.JumpPower, ForceMode2D.Impulse);
                anim.SetBool("Jump", true);
                JumpCount--;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene("SampleScene");
        }




        if (Input.GetKeyDown(KeyCode.T))
        {
            stat.AttackPower += 30;
            GetComponent<Player>().CheckEvent();
        }


    }
    void FrontWallCheck()
    {
        // 전방에 벽이 있다면 해당방향 이동시 이동속도(X축)를 0으로 만들어 벽뜷기를 방지
        //Debug.DrawRay(transform.position, new Vector3(Input.GetAxisRaw("Horizontal") * 0.5f, 0, 0), new Color(0, 1, 0));
        if (Input.GetAxisRaw("Horizontal") != 0)
            direction = Input.GetAxisRaw("Horizontal");

        RaycastHit2D rayFrontWallCheck = Physics2D.Raycast(transform.position, new Vector3(direction, 0, 0), 0.5f, (1 << LayerMask.NameToLayer("UnPassableWall")) + (1 << LayerMask.NameToLayer("Wall")));
        if (rayFrontWallCheck.collider != null)
        {
            isWall = true;
            if (direction == 1 && Input.GetKey(KeyCode.RightArrow))
                rb.velocity = new Vector2(0, rb.velocity.y);
            else if (direction == -1 && Input.GetKey(KeyCode.LeftArrow))
                rb.velocity = new Vector2(0, rb.velocity.y);

        }
        else
            isWall = false;
    }


    // 충돌 설정 ===> JumpBox 추가 후 아래와 같이 수정하였음.
    //void OnCollisionEnter2D(Collision2D col)
    //{
    //    if (col.transform.tag == "Ground")
    //    {
    //        if (JumpCount != 2)
    //            JumpCount = 2;
    //        anim.SetBool("Jump", false);
    //    }
    //}

    // 점프 가능
    public void CanJump()
    {
        if (JumpCount != JumpMaxCount)
            JumpCount = JumpMaxCount;
        anim.SetBool("Jump", false);
    }

    // 바닥없이 점프 방지
    public void CantJump()
    {
        if (JumpCount != JumpMaxCount - 1 && rb.velocity.y <= 0)
            JumpCount = 0;
        anim.SetBool("Jump", true);
    }

    public bool AreYouGround()
    {
        if (JumpCount == JumpMaxCount)
            return true;
        else
            return false;
    }

    public void ApplyDebuff(string what, float amount)
    {
        switch (what)
        {
            case "Speed":
                stat.MoveSpeed = stat.BasicSpeed - amount;
                break;
            case "Jump":
                stat.JumpPower = stat.BasicJumpPower - amount;
                break;
        }
    }
    public void RemoveDebuff(string what, float time = 0)
    {
        StartCoroutine(RemoveDebuff22(what, time));
    }
    IEnumerator RemoveDebuff22(string what, float time)
    {
        if (gameObject.activeSelf == false)
            yield break;
        yield return new WaitForSeconds(time);
        switch (what)
        {
            case "Speed":
                stat.MoveSpeed = stat.BasicSpeed;
                break;
            case "Jump":
                stat.JumpPower = stat.BasicJumpPower;
                break;
        }

    }
}
