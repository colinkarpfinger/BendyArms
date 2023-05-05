using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

namespace BendyArms
{
    

    public class CharacterControllerPhysics : MonoBehaviour
    {
        [SerializeField] private PlayerInputControllerRewired playerInput;
        [SerializeField] private Rigidbody rigidbody;

        [SerializeField] private Transform transform;
         [SerializeField] private Transform motorTransform;
        [SerializeField] private float forceMultForward = 200f;
        [SerializeField] private float forceMultTurn = 80f;
        private Vector2 _move;

        [SerializeField] private Transform cubeVert00;
        [SerializeField] private Transform cubeVert10;
        [SerializeField] private Transform cubeVert11;
        [SerializeField] private Transform cubeVert01;

        [SerializeField] EventReference propellerRef;
        private FMOD.Studio.EventInstance propellerSound;
        
        private Transform motor;
        //private Transform[] buoyancyVerts;

        void Start()
        {
            //rigidbody.useGravity = false;
            //buoyancyVerts = new Transform[]{cubeVert00, cubeVert01, cubeVert11, cubeVert10};
            propellerSound = FMODUnity.RuntimeManager.CreateInstance(propellerRef);
            motor = transform.Find("Motor");
        }

        // Update is called once per frame
        void Update()
        {
            if (playerInput)
                SetInputs();

            propellerSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(motor.gameObject));
        }

        private void FixedUpdate()
        {
            var forwardsForce = rigidbody.transform.forward * _move.y * forceMultForward;
            forwardsForce.Scale(new Vector3(1, 0, 1));
            rigidbody.AddForceAtPosition(forwardsForce, motorTransform.position);

            var rotationalForce = rigidbody.transform.right * _move.x * forceMultTurn;
            rigidbody.AddForceAtPosition(rotationalForce, motorTransform.position);

            /*foreach (Transform vert in this.buoyancyVerts) {
                float waterDiff = Mathf.Max(Water.ComputeWaterHeight(vert.position.x, vert.position.z, Water.WaterTime) - vert.position.y, 0.0f);
                rigidbody.AddForceAtPosition(Vector3.up * waterDiff * 50, vert.position);
            }*/
            float fwdProjection = Vector3.Dot(rigidbody.transform.forward, rigidbody.velocity);
            WaterRotato.RotatoSpeed = fwdProjection * 0.2f;
            PropellerRotato.ProperllerRotatoSpeed = fwdProjection * 0.3f;

            FMOD.Studio.PLAYBACK_STATE state;   
	        propellerSound.getPlaybackState(out state);
            if(_move.y > 0) {
                //Debug.Log(_move.y);
	            if(state == FMOD.Studio.PLAYBACK_STATE.STOPPED){
                    //Debug.Log("Start sound!");
                    propellerSound.start();
                }
            } else {
                if(state != FMOD.Studio.PLAYBACK_STATE.STOPPED){
                   // Debug.Log("Stop sound!");
                    propellerSound.keyOff();
                }
            }
        }

        void SetInputs()
        {
            SetDirectionals(playerInput.move);
            var shootInput = playerInput.firePrimary.value;
        }

        void SetDirectionals(Vector2 values)
        {
            _move = values;
            _move.x *= -1;
        }
    }

}