using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer sr;
    GameObject AttackTarget = null;

    enum State
    {
        Idle,
        Move,
        Jump,
        SeeO
    }
    State state;
    float WaitingTime;
    float HittedTime;

    bool isGround;
    int Direction = 1;


    const float MinActionTime = 1f;
    const float MaxActionTime = 3f;
    const int StateCount = 4;
    const float MoveSpeed = 2f;
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        WaitingTime = 0f;
        state = State.Idle;    
        
    }

    void Update()
    {
        // //������ �ð�ȿ�� Ȱ��ȭ
        // ��
        //Debug.DrawRay(transform.position, new Vector3(1.4f * Direction, 0, 0), new Color(0, 1, 0));
        // �ٴ�
        //Debug.DrawRay(transform.position + new Vector3(0.35f * -Direction, 0, 0), new Vector3(0, -1, 0) * 0.5f, new Color(0, 1, 0));





        // flipX�� ���� ���� ����(1/-1)
        Direction = sr.flipX ? -1 : 1; 


        // ���κ��� ���������̸� �����ϱ� ���� ������ üũ
        int layerMask = 1 << LayerMask.NameToLayer("Ground");  // Ground ���̾ �浹 üũ��
        RaycastHit2D rayFrontGroundCheck;
        rayFrontGroundCheck = Physics2D.Raycast(transform.position, new Vector3(Direction, 0, 0) * 1.4f, 1f, layerMask);

        // �������� ���� �߰������� + �������� �տ� ���� �ִٸ�
        if (isGround && rayFrontGroundCheck.collider != null)
            rb.velocity = new Vector2(MoveSpeed * 2f * Direction, 7f);

        // �������� �ٴ� üũ
        RaycastHit2D rayUnderGroundCheck;
        rayUnderGroundCheck = Physics2D.Raycast(transform.position + new Vector3(0.35f * -Direction, 0, 0), new Vector3(0, -1, 0), 0.5f, layerMask);

        isGround = (rayUnderGroundCheck.collider == null) ? false : true;

            // ���� ����� ���ٸ�
            if (AttackTarget == null)
                switch (state)
                {
                    // ����
                    case State.Idle:
                        rb.velocity = new Vector2(0, rb.velocity.y);

                        anim.SetBool("Idle", true);
                        anim.SetBool("Walk", false);

                        rb.velocity = new Vector2(0, rb.velocity.y);

                        if (WaitingTime < 0)
                        {
                            SetAction();
                            SetWait();
                        }
                        break;

                    // ������
                    case State.Move:
                        rb.velocity = new Vector2(0, rb.velocity.y);

                        anim.SetBool("Idle", false);
                        anim.SetBool("Walk", true);

                        rb.velocity = new Vector2(MoveSpeed * Direction, rb.velocity.y);


                        if (WaitingTime < 0)
                        {
                            SetAction();
                            SetWait();
                        }
                        break;
                    // ����
                    case State.Jump:
                        if (isGround)
                            rb.velocity = new Vector2(rb.velocity.x, 7);
                        SetAction();
                        break;
                    // ���� ��ȯ
                    case State.SeeO:
                        sr.flipX = (bool)(Random.value > 0.5f); // flipX�� �������� true false �ο�
                        SetAction();
                        break;
                }


            // ���� ����� �ִٸ�
            else
            {
                if (HittedTime < 0)
                { // �ǰݽð� ������
                    sr.flipX = (transform.position.x < AttackTarget.transform.position.x) ? false : true; // ���� ����

                    anim.SetBool("Idle", false);
                    anim.SetBool("Walk", true);

                    rb.velocity = new Vector2(MoveSpeed * 1.2f * Direction, rb.velocity.y);
                }

                if(HittedTime < 0 && Mathf.Abs(transform.position.x - AttackTarget.transform.position.x ) < 2f)
                {
                    float dinstanceGap = transform.position.y - AttackTarget.transform.position.y;
                    if(Mathf.Abs(dinstanceGap) > 0.5f && dinstanceGap < 0 && isGround)
                        rb.velocity = new Vector2(rb.velocity.x, 7);

            }


            if (WaitingTime < 0)
                    AttackTarget = null;
            }


        // ���ð�(����) Ÿ�̸�
        if(WaitingTime >= 0)
            WaitingTime -= Time.deltaTime;


        // �ǰݽð�(��ĩ) Ÿ�̸�
        if (HittedTime >= 0)
            HittedTime -= Time.deltaTime;

    }

    // ���� ���� �ο�  
    void SetAction()
    {
        if(isGround)
            switch (Random.Range(0, StateCount))
            {
                case 0:
                    state = State.Idle;
                    break;
                case 1:
                    state = State.Move;
                    break;
                case 2:
                    state = State.Jump;
                    break;
                case 3:
                    state = State.SeeO;
                    break;
            }
    }

    // ���ð�(����) ������ �ο�
    void SetWait()
    {
        WaitingTime = Random.Range(MinActionTime, MaxActionTime); ;
    }


    // Trigger ���۽�
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "PlayerAttack")
        {
            rb.velocity = Vector2.zero;

            Destroy(col.gameObject);
            AttackTarget = GameObject.FindGameObjectWithTag("Player");
            sr.flipX = (transform.position.x < AttackTarget.transform.position.x) ? false : true; // ���� ����
            rb.velocity = new Vector2(0.8f * -Direction, rb.velocity.y);

            anim.SetTrigger("Hitted");
            HittedTime = 0.5f;
            WaitingTime = 10f;


        }
    }
}
