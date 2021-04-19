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

        public Transform titanUnits;
        public LayerMask unitsLayers;

        // Start is called before the first frame update
        void Start()
        {
            instance = this;
        }

        private void Update()
        {
            foreach (Transform titan in titanUnits)
            {
                TitanUnit tU = titan.gameObject.GetComponent<TitanUnit>();
                HandleTitanActions(tU);
            }
        }

        public void HandleTitanActions(TitanUnit tU)
        {
            // Move towards the wall
            if (tU.isSearchingForHumans())
            {   
                if(tU.GetComponent<Rigidbody>().IsSleeping())
                {
                    WalksRandomly(tU);
                }
            }
            // Detect Humans nearby at the same time
            Collider[] unitsFound = Physics.OverlapSphere(tU.GetComponent<Transform>().position, tU.getVisionRange(), unitsLayers);
            if(unitsFound.Length != 0)
            {
                // if humans are in the vision range, the titan is not anymore searching for humans
                tU.setSearchingForHumans(false);
            }
            else
            {
                tU.setSearchingForHumans(true);
            }

            // If an human is near -> chase
            if (!tU.isSearchingForHumans())
            {

            }
            // When an human is in attack range -> attack
        }

        public void WalksRandomly(TitanUnit tU)
        {
            Vector3 titanPosition;
            float deltaX = 0f;
            float deltaZ = 0f;
            titanPosition = tU.GetComponent<Transform>().position;
            deltaX += Random.Range(-30f, 30f);
            deltaZ += Random.Range(-30f, 30f);
            tU.MoveTitan(new Vector3(titanPosition.x + deltaX, 0, titanPosition.z + deltaZ));
        }

    }
}


