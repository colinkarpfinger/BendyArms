using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.Serialization;

namespace BendyArms
{
    public class ButtonInput
    {
        private string actionName;
        
        public bool value;

        public delegate void Down();
        public event Down down;

        public delegate void Up();
        public event Up up;

        public ButtonInput(string actionName)
        {
            this.actionName = actionName;
        }

        public void UpdateButton(Player player)
        {
            value = player.GetButton(actionName);
            SetDown(player.GetButtonDown(actionName));
            SetUp (player.GetButtonUp(actionName));
        }

        public void SetDown(bool value)
        {
            if (value)
                OnDown();
        }

        public void SetUp(bool value)
        {
            if (value)
                OnUp();
        }
        
        private void OnUp()
        {
            up?.Invoke();
        }

        private void OnDown()
        {
            down?.Invoke();
        }
    }
    public class PlayerInputControllerRewired : MonoBehaviour
    {
        public int playerId = 0;
        private Rewired.Player rwPlayer;
        public Vector2 move;

        public ButtonInput firePrimary = new ButtonInput("ShootPrimary");
        public ButtonInput fireSecondary = new ButtonInput("ShootSecondary");
        public ButtonInput action0 = new ButtonInput("Action0");
        public ButtonInput action1 = new ButtonInput("Action1");
        public ButtonInput pauseMenu = new ButtonInput("Pause");

        private void Awake()
        {
            rwPlayer = ReInput.players.GetPlayer(playerId);
        }

        void Update()
        {
            GetInput();
        }

        void GetInput()
        {
            move.x = rwPlayer.GetAxis("Move Side");
            move.y = rwPlayer.GetAxis("Move Forward");
            
            firePrimary.UpdateButton(rwPlayer);
            fireSecondary.UpdateButton(rwPlayer);
            action0.UpdateButton(rwPlayer);
            action1.UpdateButton(rwPlayer);
            pauseMenu.UpdateButton(rwPlayer);
        }
    }
}

