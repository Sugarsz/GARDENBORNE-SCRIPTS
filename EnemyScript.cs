using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int health = 3;
    public int xp;
    [SerializeField]
    SpriteRenderer sr;
    bool canTakeDamage = true;
    public GameObject player;
    Rigidbody2D rb;
    CameraShake camShaker;
    public GameObject particlePrefab;
    public GameObject particlePrefab2;
    public GameObject hitParticle;
    public GameObject xpParticle;
    
    AudioManager audioManager;
    

    #region AI
    


    private Material matWhite;
    private Material defaultMat;

    #endregion
    void Awake()
    {   
        camShaker = GetComponent<CameraShake>();
        player = GameObject.Find("Player");
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        matWhite = Resources.Load("WhiteFlash", typeof (Material)) as Material;
        defaultMat = sr.material;
    }

    public LayerMask whoIsPlayer;
    // Update is called once per frame
    void Update()
    {   
        if (player.transform.position.x < transform.position.x ){
            sr.flipX = true;
        }else{
            sr.flipX = false;
        }
        if (health <= 0)
        {
            LevelSystem.instance.AddXp(xp);
            GameObject XP = Instantiate(xpParticle, transform.position, Quaternion.identity);
            GameObject particle = Instantiate(particlePrefab2, transform.position, Quaternion.identity);
            Destroy(particle, 3f);
            Destroy(XP, 1f);
            DropItem();
            Destroy(gameObject);
        }
    }
    

    public void TakeDamage(int damage)
    {
        audioManager.Play("hurt", Random.Range(.7f, 1.3f), .5f);
        health -= damage;
        GameObject.Find("Camera").GetComponent<CameraShake>().shakeDuration += 0.1f;
        StartCoroutine("FlashWhite");
        GameObject hit = Instantiate(hitParticle, transform.position, Quaternion.identity);
        Destroy(hit, .1f);
        Vector2 knockbackDir = player.transform.position - transform.position;
        rb.velocity = Vector2.zero;
        rb.velocity = -knockbackDir.normalized * 10;
    }

    IEnumerator FlashWhite()
    {
        sr.material = matWhite;
        

        yield return new WaitForSeconds(0.1f);
        sr.material = defaultMat;
    }

    public int[] table = { 45, 35, 20};
    public int total;
    private int randomNumber;
    public GameObject[] seeds;
    void DropItem(){
        foreach(var item in table){
            total += item;
        }
        randomNumber = Random.Range(0, total);

        for(int i = 0; i < table.Length; i++){
            if(randomNumber <= table[i]){
                Instantiate(seeds[i], transform.position, Quaternion.identity);
                return;
            }
            else{
                randomNumber -=  table[i];
            }
        }
    }
}
