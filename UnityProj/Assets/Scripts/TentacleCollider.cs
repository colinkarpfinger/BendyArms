using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleCollider : MonoBehaviour
{
    [SerializeField] private TentacleController tentacleController;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with: "+other.name);
        tentacleController.TentacleCollided(other);
    }
    
}
