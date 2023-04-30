using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleCursorDamped : MonoBehaviour
{
    [SerializeField] private Transform cursorTransform;
    [Range(0f,1f)]
    [SerializeField] private float smoothTime = 0.2f;

    private Vector3 velocity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetCursorTransform(Transform newTransform)
    {
        cursorTransform = newTransform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, cursorTransform.position, ref velocity, smoothTime);
    } 
}
