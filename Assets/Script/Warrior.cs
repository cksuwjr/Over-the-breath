using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : MonoBehaviour
{
    public Transform target;
    public float movePower = 1f;
    Vector3 movement;
    int movementFlag;
    Animator anim;
    bool isTracing;
    GameObject traceTarget;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine("ChangeMovement");
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;
        string dist = "";

        if (isTracing)
        {
            Vector3 playerPos = traceTarget.transform.position;

            if (playerPos.x < transform.position.x)
                dist = "Left";
            else if (playerPos.x > transform.position.x)
                dist = "Right";
        }
        else
        {
            if (movementFlag == 1)
                dist = "Left";
            else if (movementFlag == 2)
                dist = "Right";
        }

        if (dist == "Left")
        {
            moveVelocity = Vector3.left;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (dist == "Right")
        {
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3(-1, 1, 1);
        };

        transform.position += moveVelocity * movePower * Time.deltaTime;
    }
    IEnumerator ChangeMovement()
    {
        movementFlag = Random.Range(0, 3);

        if (movementFlag == 0)
        {
            anim.SetBool("isWalking", false);
        }
        else
        {
            anim.SetBool("isWalking", true);
        }

        yield return new WaitForSeconds(3f);

        StartCoroutine("ChangeMovement");
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            traceTarget = other.gameObject;
            StopCoroutine("ChangeMovement");
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isTracing = true;
            anim.SetBool("isWalking", true);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isTracing = false;
            StartCoroutine("ChangeMovement");
        }
    }
}

