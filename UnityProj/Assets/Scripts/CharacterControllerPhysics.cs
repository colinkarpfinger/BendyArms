using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BendyArms
{
    

    public class CharacterControllerPhysics : MonoBehaviour
    {
        [SerializeField] private PlayerInputControllerRewired playerInput;
        [SerializeField] private Rigidbody rigidbody;

        [SerializeField] private Transform transform;
         [SerializeField] private Transform motorTransform;
        [SerializeField] private float forcePerFrame = 50f;
        private Vector2 _move;

        [SerializeField] private Transform cubeVert00;
        [SerializeField] private Transform cubeVert10;
        [SerializeField] private Transform cubeVert11;
        [SerializeField] private Transform cubeVert01;

        //private Transform[] buoyancyVerts;

        void Start()
        {
            //rigidbody.useGravity = false;
            //buoyancyVerts = new Transform[]{cubeVert00, cubeVert01, cubeVert11, cubeVert10};
        }

        // Update is called once per frame
        void Update()
        {
            if (playerInput)
                SetInputs();
        }

        private void FixedUpdate()
        {
            var forwardsForce = rigidbody.transform.forward * _move.y * 200;
            forwardsForce.Scale(new Vector3(1, 0, 1));
            rigidbody.AddForceAtPosition(forwardsForce, motorTransform.position);

            var rotationalForce = rigidbody.transform.right * _move.x * 50;
            rigidbody.AddForceAtPosition(rotationalForce, motorTransform.position);

            /*foreach (Transform vert in this.buoyancyVerts) {
                float waterDiff = Mathf.Max(Water.ComputeWaterHeight(vert.position.x, vert.position.z, Water.WaterTime) - vert.position.y, 0.0f);
                rigidbody.AddForceAtPosition(Vector3.up * waterDiff * 50, vert.position);
            }*/
        }

        void SetInputs()
        {
            SetDirectionals(playerInput.move);
            var shootInput = playerInput.firePrimary.value;
        }

        void SetDirectionals(Vector2 values)
        {
            _move = values;
        }
    }

}