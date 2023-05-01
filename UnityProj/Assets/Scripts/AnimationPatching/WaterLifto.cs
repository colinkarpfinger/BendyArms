using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLifto : MonoBehaviour
{
    [SerializeField] private float minHeight = -10f;
    [SerializeField] private float maxHeight = -2f;
    [SerializeField] private float RotatoDivider = 4f;
    [SerializeField] private float smoothTime = 0.2f;
    private Vector3 initalPosition;

    private Vector3 moveVelocity; 
    // Start is called before the first frame update
    void Start()
    {
        initalPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float upAmount = Mathf.Clamp(WaterRotato.RotatoSpeed / RotatoDivider, minHeight, maxHeight);
        var targetPos = initalPosition + Vector3.up * upAmount;
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, targetPos, ref moveVelocity, smoothTime);
        
        //transform.localPosition = initalPosition + new Vector3(0, WaterRotato.RotatoSpeed / 3.0f - 0.2f, 0);
    }
}
