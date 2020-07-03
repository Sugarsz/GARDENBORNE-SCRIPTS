using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSplat : MonoBehaviour
{
    public Sprite[] sprites;

    public float minSize;
    public float maxSize;
    private SpriteRenderer sr;
    private float particleLife = 100;
    public float totalParticleLife = 50;
    public float decreaseFactor = 250;
    void Awake(){
        sr = GetComponent<SpriteRenderer>();
        particleLife = totalParticleLife;

    }

    public void Initialize(){
        SetSprite();
        SetRotation();
        SetSize();

    }

    void Update(){
        particleLife -= decreaseFactor/2 * Time.deltaTime;
        if (particleLife <= 0){
            Destroy(gameObject);
        }
        Color color = sr.color;
        color.a = particleLife/totalParticleLife ;
        sr.color = color;
    }
    void SetSprite(){
        int randomIndex = Random.Range(0, sprites.Length);

        sr.sprite = sprites[randomIndex];
    }
    void SetSize(){
        float randomSize = Random.Range(minSize, maxSize);

        transform.localScale *= randomSize;
    }

    void SetRotation(){
        float randomRotation = Random.Range(-360f, 360f);

        transform.rotation = Quaternion.Euler(0f, 0f, randomRotation);
    }


}
