using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

namespace FBTW.Units.Player
{
    
    public class PlayerUnit : MonoBehaviour
    {
        public NavMeshAgent navAgent;

        public int foodCost;
        public int gasCost;
        public int bladeCost;

        public static float m_attackRange = 10.0f;

        public static int m_agility = 100;

        public static int m_evasion = 30;

        public int maxHealth = 5;
        private int currentHealth;
        public HealthBar healthBar;

        private bool m_isAttacking, m_isMovingToEnemy;

        void Start()
        {
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
        }
        
        public void MoveUnit(Vector3 _destination)
        {
            navAgent.SetDestination(_destination);
        }
        public static int getAgility()
        {
            return m_agility;
        }
        public static int getEvasion()
        {
            return m_evasion;
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


