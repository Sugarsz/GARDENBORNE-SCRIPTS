using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject healthSyringe, staminaSyringe, superDash;

    public void SpawnItem(int item){

        if (item == 1){
            Instantiate(healthSyringe, spawnPoint.position, Quaternion.identity);
        }
        if (item == 2){
            Instantiate(staminaSyringe, spawnPoint.position, Quaternion.identity);
        }
        if (item == 3){
            Instantiate(superDash, spawnPoint.position, Quaternion.identity);
        }
    }


}
