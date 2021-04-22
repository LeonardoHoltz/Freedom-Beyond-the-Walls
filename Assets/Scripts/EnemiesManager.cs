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
            if (tU.attackInProgression)
            {
                tU.TitanAttack();
            }
            else
            {
                // Detect Humans nearby
                Collider[] unitsFound = Physics.OverlapSphere(tU.GetComponent<Transform>().position, tU.getVisionRange(), unitsLayers);
                if (unitsFound.Length != 0)
                {
                    // if humans are in the vision range, the titan is not anymore searching for humans
                    tU.setSearchingForHumans(false);
                }
                else
                {
                    // If the titan don't see anything, it is searching for humans
                    tU.setSearchingForHumans(true);
                }

                // Move randomly if no humans nearby
                if (tU.isSearchingForHumans())
                {
                    if (tU.GetComponent<Rigidbody>().IsSleeping())
                    {
                        WalksRandomly(tU);
                    }
                }
                else
                {
                    // Human is near and in the attack range
                    if(tU.IsEnemyInAttackRange())
                    {
                        tU.TitanAttack();
                    }
                    // Human is near but not in the attack range -> chase
                    else
                    {
                        tU.attackTime = 0f;
                        Transform nearestHuman = CheckNearestHuman(unitsFound, tU);
                        ApproachHuman(nearestHuman, tU);
                    }
                } 
            }

           
            
            
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

        private Transform CheckNearestHuman(Collider[] unitsFound, TitanUnit tU)
        {
            float minDist = 50f;
            float humanDistance;
            Transform nearestHuman = unitsFound[0].transform;
            foreach (Collider unit in unitsFound)
            {
                humanDistance = Vector3.Distance(unit.transform.position, tU.transform.position);
                if (humanDistance < minDist)
                {
                    minDist = humanDistance;
                    nearestHuman = unit.transform;
                }
            }
            return nearestHuman;
        }

        private void ApproachHuman(Transform human, TitanUnit tU)
        {
            Vector3 attackDistance = tU.transform.position - tU.attackPoint.position;
            tU.MoveTitan(new Vector3(human.position.x + attackDistance.x, 0, human.position.z + attackDistance.z));
        }

    }
}


