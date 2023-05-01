using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryZone : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
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
        
        gameManager.IncrementContainerCount();
        Destroy(other.gameObject);
    }
}
