using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed;
    public float jumpForce;
    public float attackRadius;
    private bool isJumping;
    private bool isAttacking;
    private int attackCombo = 0;
    private int auxFlag = 0;

    public Rigidbody2D rigidBody;
    public Animator animator;
    public Transform firePoint;
    public LayerMask enemyLayer;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Jump();
        Attack();
    }

    private void FixedUpdate() {
        float direction = Input.GetAxis("Horizontal");

        rigidBody.velocity = new Vector2(speed * direction, rigidBody.velocity.y);

        if(direction == 0){
            if(auxFlag > 60)
                auxFlag = 0;
            else
                auxFlag++;
 
            changeAnimation(0);
        }
        else if(direction > 0){
            transform.eulerAngles = new Vector2(0, 0);
            changeAnimation(1);
        }
        else{
            if(!isAttacking){
                transform.eulerAngles = new Vector2(0, -180);
                changeAnimation(1);
            }
        }
    }

    private void Jump(){
        if(Input.GetButtonDown("Jump")){
            if(!isJumping){
                rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                changeAnimation(2);
                isJumping = true;
            }
        }
    }

    private void Attack(){
        if(Input.GetButtonDown("Fire1")){
            if (!isAttacking)
            {
                bool damage = false;
                Collider2D hit = Physics2D.OverlapCircle(firePoint.position, attackRadius, enemyLayer);

                if(attackCombo < 2){
                    AttackLight();
                    attackCombo++; 
                    damage = true;
                }
                else{
                    AttackHeavy();
                    attackCombo = 0;  
                    damage = false;
                }

                if(hit != null)
                    hit.GetComponent<FlightEnemy>().onHit(damage);
                
            }
        }
    }

    private void AttackLight(){
        changeAnimation(3);
        isAttacking = true;
        StartCoroutine(onAttacking());          //wait for the time of animation
    }

    private void AttackHeavy(){
        changeAnimation(4);
        isAttacking = true;
        StartCoroutine(onAttacking());             //wait for the time of animation
    }

    private void changeAnimation(int number){
        if(!isJumping && !isAttacking)
            animator.SetInteger("transition", number);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.layer == 8){
            isJumping = false;
        }
    }

    //Wait for the time of animation
    IEnumerator onAttacking(){
        yield return new WaitForSeconds(0.6f);
        isAttacking = false;
    }
}
