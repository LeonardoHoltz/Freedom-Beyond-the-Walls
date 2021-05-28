using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

using FBTW.InputManager;
using FBTW.Units.Titans;
using FBTW.HUD;
using FBTW.Game;

namespace FBTW.Enemies
{
    public class EnemiesManager : MonoBehaviour
    {
        public static EnemiesManager instance; // Singleton

        public Transform titanUnits;
        public LayerMask unitsLayers;
        public LayerMask obsLayers;

        public Transform Scenario;

        private bool hasWaited = false;

        private float timer = 1f;

        private int titanCount;

        // Start is called before the first frame update
        void Start()
        {
            instance = this;
        }

        private void Update()
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else if (!hasWaited && timer <= 0)
            {
                hasWaited = true;
            }
            titanCount = titanUnits.childCount;
            if (titanCount <= 0 && hasWaited)
            {
                GameManager.instance.CompleteLevel();
            }
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
                tU.gameObject.GetComponent<Animator>().SetBool("isAttacking", false);
                if(!tU.gameObject.GetComponent<AudioSource>().isPlaying)
                {
                    tU.gameObject.GetComponent<AudioSource>().Play();
                }
                
                tU.AgentNotMoving(false);

                // Detect Humans nearby
                Collider[] unitsFound = Physics.OverlapSphere(tU.GetComponent<Transform>().position, tU.getVisionRange(), unitsLayers);
                Collider[] obsFound = Physics.OverlapSphere(tU.GetComponent<Transform>().position, tU.getVisionRange(), obsLayers);

                foreach (Collider obs in obsFound)
                {
                    if (obs.gameObject.tag == "Wall")
                    {
                        tU.setSearchingForHumans(false);
                        break;
                    }
                }

                if (tU.isSearchingForHumans())
                {
                    if (unitsFound.Length != 0)
                    {
                        // if humans are in the vision range, the titan is not anymore searching for humans
                        tU.setSearchingForHumans(false);
                    }
                    else
                    {
                        // If the titan don't see anything, it is searching for humans or the city
                        tU.setSearchingForHumans(true);
                    }
                }
                // Move randomly if no humans nearby
                if (tU.isSearchingForHumans())
                {
                    if (tU.GetComponent<Rigidbody>().IsSleeping())
                    {
                        WalksRandomly(tU);
                    }
                }
                // Human or Wall nearby
                else
                {
                    // Wall is near and in the attack range
                    if (tU.IsWallInAttackRange())
                    {
                        tU.gameObject.GetComponent<Animator>().SetBool("isAttacking", true);
                        tU.gameObject.GetComponent<AudioSource>().Stop();
                        tU.TitanAttack();
                    }
                    else
                    {
                        // Human is near and in the attack range
                        if (tU.IsEnemyInAttackRange())
                        {
                            tU.gameObject.GetComponent<Animator>().SetBool("isAttacking", true);
                            tU.gameObject.GetComponent<AudioSource>().Stop();
                            tU.TitanAttack();
                        }
                        // Human or Wall is near but not in the attack range -> Approach
                        else
                        {
                            tU.AgentNotMoving(false);
                            tU.gameObject.GetComponent<Animator>().SetBool("isAttacking", false);
                            if (!tU.gameObject.GetComponent<AudioSource>().isPlaying)
                            {
                                tU.gameObject.GetComponent<AudioSource>().Play();
                            }
                            tU.attackTime = 0f;
                            Transform nearestTarget = CheckNearestTarget(unitsFound, obsFound, tU);
                            ApproachTarget(nearestTarget, tU);
                        }
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

        private Transform CheckNearestTarget(Collider[] unitsFound, Collider[] obsFound, TitanUnit tU)
        {
            float minDist = 50f;
            float humanDistance, wallDistance;
            Transform nearestTarget = null;
            Transform nearestHuman;
            if(unitsFound.Length != 0)
            {
                nearestHuman = unitsFound[0].transform;
            }
            else
            {
                nearestHuman = null;
            }
            foreach (Collider obs in obsFound)
            {
                if(obs.gameObject.tag == "Wall")
                {
                    wallDistance = Vector3.Distance(obs.transform.position, tU.transform.position);
                    if (wallDistance < minDist)
                    {
                        minDist = wallDistance;
                        nearestTarget = obs.transform;
                    }
                }
            }
            if(nearestTarget == null)
            {
                minDist = 50f;
                foreach (Collider unit in unitsFound)
                {
                    humanDistance = Vector3.Distance(unit.transform.position, tU.transform.position);
                    if (humanDistance < minDist)
                    {
                        minDist = humanDistance;
                        nearestHuman = unit.transform;
                    }
                }
                nearestTarget = nearestHuman;
            }
            return nearestTarget;
        }

        private Transform CheckNearestWall(List<Collider> wallFound, TitanUnit tU)
        {
            float minDist = 50f;
            float wallDistance;
            Transform nearestWall = wallFound[0].transform;
            foreach (Collider unit in wallFound)
            {
                wallDistance = Vector3.Distance(unit.transform.position, tU.transform.position);
                if (wallDistance < minDist)
                {
                    minDist = wallDistance;
                    nearestWall = unit.transform;
                }
            }
            return nearestWall;
        }

        private void ApproachTarget(Transform human, TitanUnit tU)
        {
            Vector3 attackDistance = tU.transform.position - tU.attackPoint.position;
            tU.MoveTitan(new Vector3(human.position.x + attackDistance.x, 0, human.position.z + attackDistance.z));
        }

    }
}


