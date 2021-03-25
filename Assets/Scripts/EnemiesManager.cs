using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FBTW.InputManager;
using FBTW.Units.Titans;
using FBTW.HUD;

namespace FBTW.Enemies
{
    public class EnemiesManager : MonoBehaviour
    {
        public static EnemiesManager instance; // Singleton

        // Start is called before the first frame update
        void Start()
        {
            instance = this;
        }

        // Update is called once per frame
        void Update()
        {
            HandleEnemiesActions();
        }

        public void HandleEnemiesActions()
        {
            // Move towards the wall
            // Detect Humans nearby in the same time
            // If an human is near -> chase
            // When an human is in attack range -> attack
        }

    }
}


