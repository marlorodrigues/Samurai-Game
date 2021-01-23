using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightEnemy : MonoBehaviour
{
    public int health;
    public float Speed;
    public float stopDistance;
    private float initialSpeed;
    public bool isRight;

    public Rigidbody2D rigidBody;
    public Animator animator;
    public Transform player;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        initialSpeed = Speed;
    }

    
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        float playerPosition = transform.position.x - player.position.x;

        if(playerPosition > 0)
            isRight = false;
        else
            isRight = true;

        if(distance <= stopDistance)
            Speed = 0f;
        else
            Speed = initialSpeed;
    }

    private void FixedUpdate() {
        if(isRight){
            rigidBody.velocity = new Vector2(Speed, rigidBody.velocity.y);
            transform.eulerAngles = new Vector2(0, 0);
        }
        else{
            rigidBody.velocity = new Vector2(-Speed, rigidBody.velocity.y);
            transform.eulerAngles = new Vector2(0, 180);
        }
    }

    public void onHit(bool damage){         
        if(health < 1){
            Speed = 0f;
            animator.SetTrigger("death");
            Destroy(gameObject, 1f);
        }
        else{
            animator.SetTrigger("takeHit");
            if (damage)
                health -= 1;
            else
                health -= 2;
        }    
    }
}
