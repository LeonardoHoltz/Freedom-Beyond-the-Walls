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

        public int maxHealth = 5;
        private int currentHealth;
        public HealthBar healthBar;

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


    }
    
}


