using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FBTW.InputManager;
using FBTW.HUD;

namespace FBTW.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager instance; // Singleton

        public Transform playerUnits;

        private int m_playerUnitCount;

        // Start is called before the first frame update
        void Start()
        {
            instance = this;
        }

        // Update is called once per frame
        void Update()
        {
            InputHandler.instance.HandleUnitMovement();
            foreach(Transform child in playerUnits)
            {
                if(child.name == "Survey Corps")
                {
                    m_playerUnitCount = child.childCount;
                }
            }
            HUD.HUD.instance.SetUnitCount(m_playerUnitCount);
        }

    }
}


