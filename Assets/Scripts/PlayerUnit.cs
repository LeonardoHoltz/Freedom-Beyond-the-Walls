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

        public GameObject unit;

        public static float m_attackRange = 10.0f;

        public int currentAgility;

        private int currentHealth;

        public int currentMaxHealth;

        public HealthBar healthBar;

        private bool m_isMovingToAttack, m_isMovingToEnemy, m_isAttacking;

        private float animationOffset;

        void Start()
        {
            currentAgility = PlayerManager.instance.m_agility;
            currentHealth = PlayerManager.instance.maxHealth;
            currentMaxHealth = PlayerManager.instance.maxHealth;
            healthBar.SetMaxHealth(PlayerManager.instance.maxHealth);
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

    }

}


