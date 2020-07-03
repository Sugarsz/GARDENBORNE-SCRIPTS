using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantScript : MonoBehaviour
{
    public float waterHealth = 0;
    private float maxWater = 100;
    public bool growed;
    bool spawnRunning;

    public GameObject enemyPrefab;
    [SerializeField] Sprite[] sprites;
    SpriteRenderer spriteRenderer;

    public float spawnRate;
    public float maxSpawnWait;
    public float minSpawnWait;
    public float startWait = 1;

    public ParticleSystem particle;
    public ParticleSystem growparticle;
    public GameObject xpParticle;


    public Slider slider;
    public float lastSpawn;
    GameObject player;

    void Awake(){
        slider.gameObject.SetActive(false);
        waterHealth = 0;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[0];
        player = GameObject.Find("Player");


    }
    bool played;
    void Update(){
        
        if(growed){
            if (Vector2.Distance(transform.position, player.gameObject.transform.position) < 6f){
                if(!spawnRunning){
                    StartCoroutine("SpawnTimer");
                }
            }
        }

        if (waterHealth >= 100){
            spriteRenderer.sprite = sprites[1];
            slider.gameObject.SetActive(false);
            growed = true;
            if (!played){
                growparticle.Play();
                LevelSystem.instance.AddXp(5);
                GameObject XP = Instantiate(xpParticle, transform.position, Quaternion.identity);
                Destroy(XP, 1f);
                StartCoroutine("SpawnTimer");
                played = true;
            }
        }
        else if (waterHealth <= 0){
            slider.gameObject.SetActive(false);
        }else{
            slider.gameObject.SetActive(true);
        }
        slider.value = waterHealth/maxWater;
    }

    bool coRunning;
    bool particleRunning;
    public void TakeWater(float waterdamage){
        waterHealth += waterdamage;
        if (!coRunning && !growed){
            StartCoroutine("Particleplay");
        }
    }

    IEnumerator Particleplay(){
        particleRunning = false;
        if (!particleRunning){
            particle.Play();
        }
        particleRunning = true;
        coRunning = true;
        yield return new WaitForSeconds(.3f);
        coRunning = false;
    }

    
    IEnumerator SpawnTimer(){
        spawnRunning = true;
        startWait = Random.Range (2f, 5f);
        yield return new WaitForSeconds(startWait);
        spawnRate = Random.Range(minSpawnWait, maxSpawnWait);
        Vector2 randomPos = transform.position + Random.insideUnitSphere * 2f;
        Vector3 spawnPos = new Vector3(randomPos.x, randomPos.y, -2f);
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        yield return new WaitForSeconds(spawnRate);
        spawnRunning = false;
    }


}
