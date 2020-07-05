using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coguScript : MonoBehaviour
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
        if(!isCharging){
            ChasePlayer();
        }
        
    }

    void ChasePlayer(){
        anim.Play("cogu_run");
        if (!isCharging && Vector2.Distance(transform.position, player.transform.position) <= followRange){
            
            Vector2 dir = player.transform.position - transform.position;
            rb.velocity = dir.normalized * speed;
        }else{
            rb.velocity = Vector2.zero;
        }
        if (canAttack && Vector2.Distance(transform.position, player.transform.position) <= attackRange){
            StartCoroutine("Charge");
        }

    }
    public GameObject bulletPrefab;
    bool isAttacking;
    IEnumerator Charge(){
        anim.Play("cogu_idle");
        isCharging = true;
        yield return new WaitForSeconds(1f);
        Vector2 upRight = new Vector2(.1f,.1f);
        Vector2 upLeft = new Vector2(.1f,-.1f);
        Vector2 downRight = new Vector2(-.1f,.1f);
        Vector2 downLeft = new Vector2(-.1f,-.1f);
        Vector2 myPos = transform.position;

        GameObject bulletLeftUp = Instantiate(bulletPrefab, myPos+upLeft, Quaternion.identity);
        GameObject bulletLeftDown = Instantiate(bulletPrefab,myPos+ downLeft, Quaternion.identity);
        GameObject bulletRightUp = Instantiate(bulletPrefab, myPos+upRight, Quaternion.identity);
        GameObject bulletRightDown = Instantiate(bulletPrefab, myPos + downRight, Quaternion.identity);

        bulletLeftUp.GetComponent<Rigidbody2D>().velocity = upLeft.normalized * 7 ;
        bulletLeftDown.GetComponent<Rigidbody2D>().velocity = downLeft.normalized * 7; 
        bulletRightUp.GetComponent<Rigidbody2D>().velocity = upRight.normalized * 7 ;
        bulletRightDown.GetComponent<Rigidbody2D>().velocity = downRight.normalized * 7;
        Destroy(bulletLeftDown, 4f);
        Destroy(bulletRightDown, 4f);
        Destroy(bulletLeftUp, 4f);
        Destroy(bulletRightUp, 4f);

        yield return new WaitForSeconds (.5f);
        anim.Play("cogu_run");
        isCharging = false;
        canAttack = false;

        Vector2 up = new Vector2(.1f,0f);
        Vector2 Left = new Vector2(0,-.1f);
        Vector2 Right = new Vector2(0,.1f);
        Vector2 down = new Vector2(-.1f,0);

        GameObject bulletUp = Instantiate(bulletPrefab, myPos+up, Quaternion.identity);
        GameObject bulletLeft = Instantiate(bulletPrefab,myPos+ Left, Quaternion.identity);
        GameObject bulletRight = Instantiate(bulletPrefab, myPos+Right, Quaternion.identity);
        GameObject bulletDown = Instantiate(bulletPrefab, myPos + down, Quaternion.identity);

        bulletUp.GetComponent<Rigidbody2D>().velocity = up.normalized * 5 ;
        bulletLeft.GetComponent<Rigidbody2D>().velocity = Left.normalized * 5; 
        bulletRight.GetComponent<Rigidbody2D>().velocity = Right.normalized * 5 ;
        bulletDown.GetComponent<Rigidbody2D>().velocity = down.normalized * 5;
        Destroy(bulletLeft, 4f);
        Destroy(bulletRight, 4f);
        Destroy(bulletUp, 4f);
        Destroy(bulletDown, 4f);

        isAttacking = false;

        yield return new WaitForSeconds (3f);
        canAttack = true;
        
        //this code is trash dont ever do this again
    }
}
