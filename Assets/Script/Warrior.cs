using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : MonoBehaviour
{
    public float moveSpeed = 5f;      // ������ �ӵ�
    int movementFlag;                 // ������ ����(1 2)
    bool isTracing;                   // ���� ����(O/X)
    Vector2 moveVelocity;     // ������ ���ӵ� �ʱ�ȭ


    public Transform target;          // Ÿ�� ��ġ���� ������������
    Animator anim;                    // �ִϸ�����
    GameObject traceTarget;           // ���� Ÿ�� ���ӿ�����Ʈ
    Rigidbody2D rb;
    SpriteRenderer sr;
    enum Dist
    {
        Left,
        Right
    }
    Dist dist;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine("ChangeMovement"); // ChangeMovement() ȣ��
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    void Move()
    {
        
        if (isTracing)          // ���� ���̸�
        {
            Vector2 playerPos = traceTarget.transform.position;     // ��� ��ġ���� ������

            if (playerPos.x < transform.position.x)                 // ������ �ڸ�(����) ���⼳��(��)  
                dist = Dist.Left;
            else if (playerPos.x > transform.position.x)            // ������ ���̸�(������) ���⼳��(��)
                dist = Dist.Right;
        }
        else                   // ���� ���� �ƴϸ�
        {
            if (movementFlag == 1)          // ������ ������ 1�̸� ���� ����(��)
                dist = Dist.Left;
            else if (movementFlag == 2)     // ������ ������ 2�̸� ���� ����(��)
                dist = Dist.Right;
        }

        if (dist == Dist.Left)              // ����(��)�̸� �����Ӱ��ӵ�(����) ���� new Vector(-1,0) , ���� �ٶ󺸰�(flipX)
        {
            moveVelocity = Vector2.left;
            sr.flipX = false;
        }
        else        // ����(��)�̸� �����Ӱ��ӵ�(����) ���� new Vector(+1,0) , ������ �ٶ󺸰�(flipX)
        {
            moveVelocity = Vector2.right;
            sr.flipX = true;
        };

        rb.velocity = moveVelocity * moveSpeed;        //    Vector(+-1, 0) * 1f * (���������� ������ �����ð��� ���)/ �� �÷��̾� ���� ������
        Debug.Log(rb.velocity);
        Debug.Log(moveVelocity);
        Debug.Log(moveSpeed);

    }

    IEnumerator ChangeMovement()            // ������ȯ �޼���(�ൿ���Ϻ�ȭ)
    {
        movementFlag = Random.Range(0, 3);      // 0~2���� �������� �����ؼ� movementFlag�� ����

        if (movementFlag == 0)                     // �÷��̾�ִϸ��̼� ����(����)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);


        yield return new WaitForSeconds(3f);        // 3�� ����� 

        StartCoroutine("ChangeMovement");           // �ٽ� �� �޼��� ����
    }
    void OnTriggerEnter2D(Collider2D other)         // ������ �������
    {
        if (other.gameObject.tag == "Player")       // �÷��̾��
        {
            traceTarget = other.gameObject;         // ������� �÷��̾� ����
            StopCoroutine("ChangeMovement");        // ���� �޼���(�ൿ���Ϻ�ȭ) �����
        }
    }
    void OnTriggerStay2D(Collider2D other)          // ��� ���� ����ִµ���
    {
        if (other.gameObject.tag == "Player")
        {
            isTracing = true;                       //  �������ΰ�? => ��
            anim.SetBool("isWalking", true);        //  �ִϸ��̼� �۵�(�ȱ�Ȱ��ȭ)
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isTracing = false;                     // �������ΰ�? => �ƴϿ�
            StartCoroutine("ChangeMovement");       // �ൿ���Ϻ�ȭ
        }
    }
}

