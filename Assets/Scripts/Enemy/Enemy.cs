using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public float Speed;
    public float damage;
    public float stopDistance;
    public bool isRight;
    public bool isFirst = true;
    public bool isAttacking;
    public bool isDeath = false;
    
    private float initialSpeed;
    private int attackCombo = 0;
    private float distance;
    private float playerPosition;
    private bool auxAnimationAttack = false;
    

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
        if (isDeath)
            return;

        if(!isAttacking){
            distance = Vector2.Distance(transform.position, player.position);
            playerPosition = transform.position.x - player.position.x;

            if (playerPosition > 0)
                isRight = false;
            else
                isRight = true;
        }

        if (distance <= stopDistance)
        {
            Speed = 0f;
            isAttacking = true;
            player.GetComponent<Player>().onHit(damage);

            if(!auxAnimationAttack){
                if (isFirst)
                {
                    isFirst = false;
                    animator.SetInteger("transition", 3);
                }
                else if (attackCombo > 2)
                {
                    attackCombo = 0;
                    animator.SetInteger("transition", 3);
                }
                else
                {
                    attackCombo++;
                    animator.SetInteger("transition", 2);
                }

                auxAnimationAttack = true;
                StartCoroutine(onAttacking());             //wait for the time of animation
            }
        }
        else{
            Speed = initialSpeed;
        }
    }

    private void FixedUpdate()
    {
        if (isDeath)
            return;
        

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
        auxAnimationAttack = false;
    }
}
