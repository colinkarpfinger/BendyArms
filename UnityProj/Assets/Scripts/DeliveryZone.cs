using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class DeliveryZone : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private StudioEventEmitter audioSuccess;

    [SerializeField] private Transform player;

    public float spawnRadius = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.attachedRigidbody)   //might want to add a flag to see if these are containers and not other stuff. 
            return;

        if (other.attachedRigidbody.isKinematic)    //don't count it when still attached to tentacle 
            return;

        if (!other.gameObject.CompareTag("Containers"))
            return; 
        
        audioSuccess.Play();
        gameManager.IncrementContainerCount();
        Destroy(other.gameObject);
        float angle = (float)UnityEngine.Random.value * (float)Math.PI * 2.0f;
        Vector3 spawnPos = new Vector3(spawnRadius * (float)Math.Cos(angle) + player.transform.position.x, 0, spawnRadius * (float)Math.Sin(angle) + player.transform.position.z);
        SpawnCrate.SpawnNewCrate(spawnPos);
        //other.attachedArticulationBody.velocity = new Vector3(0, 0, 0);
    }
}
