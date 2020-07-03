using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{
    public int currentXp;
    public int nextLevelXp;
    public int currentLevel;
    public int total_xp;
    public int consecutiveLevel;
    public Text xpText, levelText, nextLevelText;
    public Image xpbarImage;
    public GameObject warningText;
    public GameObject staminaText;
    public static LevelSystem instance;
    public float currentLevelstring;

    void Awake(){
        if (instance == null){
            instance = this;
        }
        else{
            Destroy(gameObject);
        }
    }

    public void Start(){
        SetXp();
        currentLevel = 1;
        currentLevelstring = 1;
        warningText.SetActive(false);
        UpdateText();
    }
    bool hasSpawned1;
    bool hasSpawned2;
    bool hasSpawned3;
    public void AddXp(int amount){
        currentXp += amount;

        if(currentXp >= nextLevelXp){

            currentXp = currentXp - nextLevelXp;
            currentLevel++;
            currentLevelstring = currentLevel;
            consecutiveLevel ++;
            nextLevelXp += Mathf.CeilToInt((nextLevelXp * 10)/100);
            GameObject.Find("Player").GetComponent<PlayerScripts>().LevelUp();
            StartCoroutine("StaminaText");
        }
        if (consecutiveLevel >= 3){
            Debug.Log("Milestone reached");
            GameObject.Find("Bonfire").GetComponent<bonfire>().SpawnPotion();
            consecutiveLevel = 0;
        }
        if (currentLevel == 10 && !hasSpawned1){
            SpawnItem(1);
            StartCoroutine("WarnText");
            hasSpawned1 = true;
        }
        if (currentLevel == 15 && !hasSpawned2){
            SpawnItem(2);
            StartCoroutine("WarnText");
            hasSpawned2 = true;
        }
        if (currentLevel == 20 && !hasSpawned3){
            SpawnItem(3);
            StartCoroutine("WarnText");
            hasSpawned3 = true;
        }
        SetXp();
        UpdateText();
    }  
    int nextNormal;
    
    public Transform spawnPoint;
    public GameObject healthSyringe, staminaSyringe, superDash;

    public void SpawnItem(int item){

        if (item == 1){
            
            GameObject.Find("Bonfire").GetComponent<bonfire>().SpawnItem();
            Instantiate(staminaSyringe, spawnPoint.position, Quaternion.identity);
        }
        if (item == 2){
            GameObject.Find("Bonfire").GetComponent<bonfire>().SpawnItem();
            Instantiate(healthSyringe, spawnPoint.position, Quaternion.identity);
        }
        if (item == 3){
            GameObject.Find("Bonfire").GetComponent<bonfire>().SpawnItem();
            Instantiate(superDash, spawnPoint.position, Quaternion.identity);
        }
    }

    void UpdateText(){
        Debug.Log("current Level:" + currentLevel);
        levelText.text = currentLevel.ToString();
        int nextLevelLevel = Mathf.FloorToInt(currentLevelstring +1);
        nextLevelText.text = nextLevelLevel.ToString();
    }
    IEnumerator WarnText(){
        warningText.SetActive(true);
        yield return new WaitForSeconds(5f);
        warningText.SetActive(false);
    }
    IEnumerator StaminaText(){
        staminaText.SetActive(true);
        yield return new WaitForSeconds(2f);
        staminaText.SetActive(false);
    }
    public void SetXp(){
        float xpNormalized = (float)currentXp/nextLevelXp;
        Debug.Log(xpNormalized);
        xpbarImage.fillAmount = xpNormalized;
    }


}
