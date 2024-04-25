using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
public class Move : MonoBehaviour
{
    Rigidbody2D rb;     
    SpriteRenderer sr;  
    Animator anim;

    Status stat;   
    PlayerSkill skill;

    int JumpCount = 2;
    public int JumpMaxCount = 2;
    public bool isWall;
    float direction;

    public bool Movable = true;
    public bool isGround;
    bool Jumpable = true;

    public Transform groundCheck;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        stat = GetComponent<Status>();
        skill = GetComponent<PlayerSkill>();
    }

    void Update()
    {
        if (skill.isMumchit || GameManager.Instance.StoryManager.nowStoryReading)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            anim.SetBool("Idle", true);
            anim.SetBool("Walk", false);
        }
        else
            PlayerKeyboardInput();

        FrontWallCheck();
        GroundCheck();

        FallSpeedLimit();

    }
    public void FallSpeedLimit()
    {
        if(rb.velocity.y < -15)
            rb.velocity = new Vector2(rb.velocity.x, -15);
    }

    // Player 키보드 입력 (움직임)
    void PlayerKeyboardInput()
    {
        if (!Movable) return;

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

        if (Input.GetKeyDown(KeyCode.UpArrow))
            Jumpable = true;

        // 점프!!
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Jump();
        }

   
    }
    void Jump()
    {
        if (isGround)
        {
            Jumpable = false;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, 2) * stat.JumpPower, ForceMode2D.Impulse);
            JumpCount = JumpMaxCount - 1;
        }
        else
        {
            if (JumpCount > 0 && Jumpable)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(new Vector2(0, 2) * stat.JumpPower, ForceMode2D.Impulse);
                anim.SetBool("Jump", true);
                JumpCount--;
            }
        }

    }

    void FrontWallCheck()
    {
        // 전방에 벽이 있다면 해당방향 이동시 이동속도(X축)를 0으로 만들어 벽뜷기를 방지
        //Debug.DrawRay(transform.position, new Vector3(Input.GetAxisRaw("Horizontal") * 0.5f, 0, 0), new Color(0, 1, 0));
        if (Input.GetAxisRaw("Horizontal") != 0)
            direction = Input.GetAxisRaw("Horizontal");

        RaycastHit2D rayFrontWallCheck = Physics2D.Raycast(transform.position, new Vector3(direction, 0, 0), 0.5f, (1 << LayerMask.NameToLayer("UnPassableWall")) + (1 << LayerMask.NameToLayer("Wall") + (1 << LayerMask.NameToLayer("Ground"))));
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
    void GroundCheck()
    {
        Collider2D collider = Physics2D.OverlapBox(groundCheck.position, new Vector2(0.46f, 0.06f), 0, GetComponent<Player>().groundLayer);
        if (collider && rb.velocity.y < 0.2f)
        {
            isGround = true;
            anim.SetBool("Jump", false);
        }
        else
        {
            isGround = false;
            anim.SetBool("Jump", true);
        }

    }
    private void OnEnable()
    {
        Movable = true;
    }
}
