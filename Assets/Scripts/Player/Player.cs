using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float health;
    public float speed;
    public float jumpForce;
    public float attackRadius;
    public float recoveryTime;

    private float recoveryCount;
    private bool isJumping;
    private bool isAttacking;
    private int attackCombo = 0;
    private int auxFlag = 0;

    bool gameOver = false;

    public Rigidbody2D rigidBody;
    public Animator animator;
    public Transform firePoint;
    public LayerMask enemyLayer;
    public GameController gameController;

    public Image healthBar;
    public Image staminaBar;
    public Image manaBar;
    public Image iconPlayer;

    [Header("Audio Settings")]
    public AudioSource attackAudio;
    public AudioClip sfx;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(gameOver)
            return;

        Jump();
        Attack();
    }

    private void FixedUpdate() {

        if(gameOver)
            return;

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

                attackAudio.PlayOneShot(sfx);
                
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

    public void onHit(float damage){

        if(gameOver)
            return;

        recoveryCount += Time.deltaTime;

        if(recoveryCount >= recoveryTime){
            health -= damage;

            iconPlayer.fillAmount -= (float)((damage/100f) + 0.075);
            healthBar.fillAmount -= damage/100f;
            staminaBar.fillAmount -= 0.078f;
            manaBar.fillAmount -= 0.0654f;

            if(health < 1){
                animator.SetTrigger("death");
                GameOver();
            }
            else
                animator.SetTrigger("takeHit");
            
            recoveryCount = 0f;
        }
    }

    private void GameOver(){
        gameOver = true;
        gameController.showGameOver();
    }
}
