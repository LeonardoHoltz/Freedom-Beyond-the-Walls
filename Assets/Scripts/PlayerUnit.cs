using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
using FBTW.InputManager;

namespace FBTW.Units.Player
{
    
    public class PlayerUnit : MonoBehaviour
    {
        public NavMeshAgent navAgent;

        public GameObject unit;

        public static float m_attackRange = 10.0f;

        public int m_agility = 100;


        public int maxHealth = 5;
        private int currentHealth;
        public HealthBar healthBar;

        private bool m_isMovingToAttack, m_isMovingToEnemy, m_isAttacking;

        void Start()
        {
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
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
            return m_agility;
        }
        public int getHealth()
        {
            return currentHealth;
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


