using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRotato : MonoBehaviour
{
    public static float RotatoSpeed = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation *= Quaternion.EulerRotation(0, RotatoSpeed * Time.deltaTime, 0);
    }
}
