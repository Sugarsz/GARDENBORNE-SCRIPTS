using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public int Type;
    GameObject str,htr,dtr,devilblabla;
    UIScripts uiScripts;
    void Start(){

    }
    void Awake(){
        uiScripts = GameObject.Find("StatusBars").GetComponent<UIScripts>();
    }
    void OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.CompareTag("Player")){
            if (Type ==1 ){
                GameObject.Find("Player").GetComponent<PlayerScripts>().seeds1 += 1;

            }
            if (Type ==2 ){
                GameObject.Find("Player").GetComponent<PlayerScripts>().seeds2 += 1;

            }
            if (Type ==3 ){
                GameObject.Find("Player").GetComponent<PlayerScripts>().seeds3 += 1;

            }
            if (Type ==9 ){
                GameObject.Find("Player").GetComponent<PlayerScripts>().staminaRegen += 35;
                uiScripts.PlayAnimation(1);

            }
            if (Type ==5 ){
                GameObject.Find("Player").GetComponent<PlayerScripts>().maxHealth += 50;
                GameObject.Find("Player").GetComponent<PlayerScripts>().Heal(50);
                uiScripts.PlayAnimation(2);

            }
            if (Type ==6 ){
                GameObject.Find("Player").GetComponent<PlayerScripts>().hasSuperDash = true;
                GameObject.Find("Player").GetComponent<PlayerScripts>().attackDamage += 1;
                uiScripts.PlayAnimation(3);

            }
            uiScripts.UpdateText();
            Destroy(gameObject);
        }
    }

    public void PrintEvent(){
        Debug.Log(gameObject.name);
    }

    
}
