using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikeScript : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    public GameObject player;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.Find("Player");
    }
    public float speed = 10f;
    public float followRange = 20f;
    public float attackRange = 50f;
    bool isCharging = false;
    float timeSinceLastAttack;
    public float attackCooldown;
    bool canAttack = true;
    public LayerMask whoIsPlayer;

    // Update is called once per frame
    void Update()
    {
        if(!isCharging ){
            ChasePlayer();
        }
        if (isAttacking){
            Collider2D[] playerInside = Physics2D.OverlapCircleAll(transform.position, 1.3f, whoIsPlayer);
            for (int i = 0; i < playerInside.Length; i++){
                playerInside[i].GetComponent<PlayerScripts>().TakeDamage(10);
                Rigidbody2D playerRb = playerInside[i].GetComponent<Rigidbody2D>();
                Vector3 playerPos = playerInside[i].transform.position;
                Vector2 knockbackDir = transform.position - playerPos;
                playerInside[i].GetComponent<PlayerScripts>().DisableMovementForAWhile();
                playerRb.velocity += -knockbackDir.normalized * 15f;

                if(playerInside.Length > 0){
                    isAttacking = false;
                 }
            }
            
        }
        
    }

    void ChasePlayer(){
        anim.Play("spike_run");
        if (!isCharging && Vector2.Distance(transform.position, player.transform.position) <= followRange){
            
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 2f * Time.deltaTime);
        }else{
            rb.velocity = Vector2.zero;
        }
        if (canAttack && Vector2.Distance(transform.position, player.transform.position) <= attackRange){
            StartCoroutine("Charge");
        }

    }

    bool isAttacking;
    IEnumerator Charge(){
        anim.Play("spike_idle");
        isCharging = true;
        yield return new WaitForSeconds(1f);
        anim.Play("spike_run");
        isAttacking = true;
        Vector2 dir = player.transform.position - transform.position;
        rb.velocity = dir.normalized * 35f;
        isCharging = false;
        canAttack = false;
        yield return new WaitForSeconds (4f);
        isAttacking = false;
        canAttack = true;
        
        
    }
}
