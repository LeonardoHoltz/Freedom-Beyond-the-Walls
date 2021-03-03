using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FBTW.InputManager;

namespace FBTW.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager instance; // Singleton

        public Transform playerUnits;

        // Start is called before the first frame update
        void Start()
        {
            instance = this;
        }

        // Update is called once per frame
        void Update()
        {
            InputHandler.instance.HandleUnitMovement();
        }
    }
}


