using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public float Speed;
    public float damage;
    public float stopDistance;
    private float initialSpeed;
    public bool isRight;
    public bool isFirst = true;
    public bool isAttacking;
    private int attackCombo = 0;
    public bool isDeath = false;

    public Rigidbody2D rigidBody;
    public Animator animator;
    public Transform player;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        initialSpeed = Speed;
        isAttacking = false;
    }

    void Update()
    {
        if (isDeath || isAttacking)
            return;

        float distance = Vector2.Distance(transform.position, player.position);
        float playerPosition = transform.position.x - player.position.x;

        if (playerPosition > 0)
            isRight = false;
        else
            isRight = true;

        if (distance < stopDistance)
        {
            Speed = 0f;
            isAttacking = true;

            if (isFirst)
            {
                isFirst = false;

                StartCoroutine(sleep());             //wait for the time of animation

                // rigidBody.velocity = new Vector2(0, 0);
                // transform.eulerAngles = new Vector2(0, 180);

                animator.SetInteger("transition", 3);
            }
            else if (attackCombo > 2)
            {
                animator.SetInteger("transition", 3);
                attackCombo = 0;
            }
            else
            {
                attackCombo++;
                animator.SetInteger("transition", 2);
            }

            StartCoroutine(onAttacking());             //wait for the time of animation
        }
        else
            Speed = initialSpeed;
    }

    private void FixedUpdate()
    {
        if (isDeath || isAttacking)
        {
            Speed = 0f;
            return;
        }

        changeAnimation(1);

        if (isRight)
        {
            rigidBody.velocity = new Vector2(Speed, 0);
            transform.eulerAngles = new Vector2(0, 0);
        }
        else
        {
            rigidBody.velocity = new Vector2(-Speed, 0);
            transform.eulerAngles = new Vector2(0, 180);
        }
    }

    public void changeAnimation(int number)
    {
        if (!isAttacking)
            animator.SetInteger("transition", number);
    }

    public void onHit(bool damage)
    {

        if (damage)
            health -= 1;
        else
            health -= 2;

        if (health < 1)
        {
            Speed = 0f;
            animator.SetTrigger("death");
            isDeath = true;
            Destroy(gameObject, 1f);
        }
        else
        {
            animator.SetTrigger("takeHit");
        }
    }

    //Wait for the time of animation
    IEnumerator onAttacking()
    {
        yield return new WaitForSeconds(1f);
        isAttacking = false;
    }

    IEnumerator sleep()
    {
        yield return new WaitForSeconds(1f);
    }
}
