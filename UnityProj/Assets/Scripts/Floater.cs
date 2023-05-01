using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    [SerializeField] private Rigidbody affectedBody;
    // Start is called before the first frame update

    [SerializeField] private float force = 150.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float waterDiff = Mathf.Max(Water.ComputeWaterHeight(transform.position.x, transform.position.z, Water.WaterTime) - transform.position.y, 0.0f);
        affectedBody.AddForceAtPosition(Vector3.up * waterDiff * force, transform.position);
    }
}
