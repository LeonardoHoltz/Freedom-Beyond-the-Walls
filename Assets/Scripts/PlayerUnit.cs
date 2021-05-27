using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
using FBTW.InputManager;
using FBTW.Player;

namespace FBTW.Units.Player
{
    
    public class PlayerUnit : MonoBehaviour
    {
        public NavMeshAgent navAgent;

        public GameObject unit, respectiveCavalry;

        public LayerMask unitsLayers;

        public static float m_attackRange = 10.0f;

        public int currentAgility;

        public int modifier = 0;

        private int currentHealth;

        public int currentMaxHealth;

        public HealthBar healthBar;

        private bool m_isMovingToAttack, m_isMovingToEnemy, m_isAttacking, m_wantsToRideHorse;

        private Transform m_targetHorse;

        private float animationOffset;

        void Start()
        {
            currentAgility = PlayerManager.instance.m_agility;
            currentHealth = PlayerManager.instance.maxHealthHuman;
            currentMaxHealth = PlayerManager.instance.maxHealthHuman;
            healthBar.SetMaxHealth(PlayerManager.instance.maxHealthHuman);
            animationOffset = Random.Range(0.0f, 1.0f);
            unit.GetComponent<Animator>().SetFloat("offset", animationOffset);
        }

        private void Update()
        {
            // Animation:

            if (m_isAttacking)
            {
                unit.GetComponent<Animator>().SetBool("isAttacking", true);
            }
            else
            {
                unit.GetComponent<Animator>().SetBool("isAttacking", false);
                

                if (unit.GetComponent<Rigidbody>().IsSleeping())
                {
                    unit.GetComponent<Animator>().SetBool("isRunning", false);
                }
                else
                {
                    unit.GetComponent<Animator>().SetBool("isRunning", true);
                }
            }
            
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
            if (getHealth() <= 0)
            {
                // remove highlight if unit is selected
                if(unit.gameObject.GetComponent<Outline>() != null)
                {
                    Destroy(unit.gameObject.GetComponent<Outline>());
                }
                Destroy(unit, 2);
            }
        }

        public void RideHorse()
        {
            bool foundHorse = false;

            /* check if the horse is null, if it is null, then probably the horse was already mounted
             * and in the logic the horse and the human were deleted to generate the new cavalry.
             * Then we need to specify that the human doesn't want more to ride a horse */
            if(m_targetHorse == null)
            {
                setWantsToRideHorse(false);
                MoveUnit(unit.transform.position);
            }
            else
            {
                // check if the horse is near, if it is, ride:
                Collider[] hitUnits = Physics.OverlapSphere(unit.GetComponent<Transform>().position, 2.0f, unitsLayers);
                foreach (Collider otherUnit in hitUnits)
                {
                    if(otherUnit.tag == "HorseUnit")
                    {
                        if(otherUnit.transform == m_targetHorse)
                        {
                            otherUnit.GetComponent<HorseUnit>().setHumanMoutinginTheHorse(unit);
                            foundHorse = true;
                            Vector3 horsePosition = otherUnit.transform.position;
                            Quaternion horseRotation = otherUnit.transform.rotation;
                            
                            if(m_targetHorse.GetComponent<HorseUnit>().getHumanMoutinginTheHorse() == unit)
                            {
                                Destroy(otherUnit.gameObject);
                                Destroy(unit);
                                GameObject newCavalry = Instantiate(respectiveCavalry, horsePosition, horseRotation);
                                newCavalry.transform.SetParent(GameObject.Find("Survey Corps").transform);
                                InputHandler.listUnitsToBeselected.Add(newCavalry.transform);
                            }
                            else
                            {
                                foundHorse = false;
                            }
                            break;
                        }
                    }
                }
                if(!foundHorse)
                {
                    MoveUnit(m_targetHorse.position);
                }
            }
        }

        public void MoveUnit(Vector3 destination)
        {
            navAgent.SetDestination(destination);
        }
        public int getAgility()
        {
            return currentAgility;
        }
        public int getHealth()
        {
            return currentHealth;
        }
        public void setCurrentHealth(int hp)
        {
            currentHealth = hp;
            healthBar.SetHealth(currentHealth);
        }
        public void setMaximumHealth(int hp)
        {
            currentMaxHealth = hp;
            healthBar.SetMaxHealth(currentMaxHealth);
        }

        public void setAgility(int agility)
        {
            currentAgility = agility;
        }

        public void setMovingToAttack(bool movingToAttack)
        {
            m_isMovingToAttack = movingToAttack;
        }

        public bool getMovingToAttack()
        {
            return m_isMovingToAttack;
        }

        public void setAttacking(bool attacking)
        {
            m_isAttacking = attacking;
        }

        public bool getAttacking()
        {
            return m_isAttacking;
        }

        public float getAttackRange()
        {
            return m_attackRange;
        }

        public void setWantsToRideHorse(bool wantsToRide)
        {
            m_wantsToRideHorse = wantsToRide;
        }

        public bool getWantsToRideHorse()
        {
            return m_wantsToRideHorse;
        }

        public void setTargetHorse(Transform horse)
        {
            m_targetHorse = horse;
        }

    }

}


