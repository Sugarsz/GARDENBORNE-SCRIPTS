using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    public ParticleSystem particleSplat;
    public GameObject splatPrefab;
    private List <ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    void OnParticleCollision(GameObject other){
        ParticlePhysicsExtensions.GetCollisionEvents(particleSplat, other, collisionEvents);

        int count = collisionEvents.Count;

        for (int i = 0; i < count; i++){
            GameObject splat = Instantiate(splatPrefab, collisionEvents[i].intersection, Quaternion.identity);
            ParticleSplat ps = splat.GetComponent<ParticleSplat>();
            ps.Initialize();
        }
    }
}
