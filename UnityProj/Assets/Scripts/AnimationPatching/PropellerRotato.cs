using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellerRotato : MonoBehaviour
{
    public static float ProperllerRotatoSpeed = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAroundLocal(Vector3.forward, ProperllerRotatoSpeed * Time.deltaTime);
    }
}
