using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLifto : MonoBehaviour
{
    private Vector3 initalPosition;

    // Start is called before the first frame update
    void Start()
    {
        initalPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = initalPosition + new Vector3(0, WaterRotato.RotatoSpeed - 0.3f, 0);
    }
}
