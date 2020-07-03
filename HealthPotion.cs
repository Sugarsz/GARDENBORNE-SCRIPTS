using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.CompareTag("Player")){
            AudioManager.instance.Play("healthpotion", Random.Range(.6f, 1f), .3f);
            GameObject.Find("Player").GetComponent<PlayerScripts>().Heal(100f);
            Destroy(gameObject);
        }
    }
}
