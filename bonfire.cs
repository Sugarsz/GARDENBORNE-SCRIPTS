using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bonfire : MonoBehaviour
{   
    public Sprite[] sprites;
    SpriteRenderer spriteRenderer;
    bool lit;
    public GameObject lightss;
    public GameObject particle;
    float waterHealth = 0;
    public GameObject potionPrefab;
    public ParticleSystem explosionWarning;
    public ParticleSystem explosionWarning2;

    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update(){
        if (lit){
            spriteRenderer.sprite = sprites[0];
            lightss.SetActive(true);
            particle.SetActive(true);
        }
    }
    public void unfire(){
        
        waterHealth += 100 * Time.deltaTime;
        if (waterHealth > 50){
            spriteRenderer.sprite = sprites[1];
            lightss.SetActive(false);
            particle.SetActive(false);

        }
        if (!running){
        StartCoroutine("returnfire");

        }else{
            StopCoroutine("returnfire");
            StartCoroutine("returnfire");
        }
    }
    bool running;
    IEnumerator returnfire(){
        lit = false;
        running = true;
        yield return new WaitForSeconds(5f);
        lit = true;
        waterHealth = 0;
        running = false;
    }

    public void SpawnPotion(){
        explosionWarning.Play();
        AudioManager.instance.Play("bonfirelit", 1f, .6f);
        Vector3 randomPos = transform.position + Random.insideUnitSphere * 3;
        Vector2 spawnPos = new Vector2(randomPos.x, randomPos.y);
        Instantiate(potionPrefab, spawnPos , Quaternion.identity);
    }
    public void SpawnItem(){
        explosionWarning2.Play();
        AudioManager.instance.Play("bonfirelit", 1f, .6f);
    }
}

