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
        [SerializeField] private float forcePerFrame = 50f;
        private Vector2 _move;
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (playerInput)
                SetInputs();
        }

        private void FixedUpdate()
        {
            var force = new Vector3(_move.x, 0f, _move.y);
            rigidbody.AddForce(force*forcePerFrame);
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