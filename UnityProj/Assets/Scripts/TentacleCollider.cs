using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleCollider : MonoBehaviour
{
    [SerializeField] private TentacleController tentacleController;

    public delegate void TentacleCollided(Collider other);

    public event TentacleCollided tentacleCollided;

    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log("Collided with: "+other.name);
        OnTentacleCollided(other);
    }

    void OnTentacleCollided(Collider other)
    {
        tentacleCollided?.Invoke(other);
    }
}
