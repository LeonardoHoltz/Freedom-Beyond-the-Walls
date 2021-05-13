using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;



namespace FBTW.Units.Player
{

    public class HorseUnit : MonoBehaviour
    {
        public NavMeshAgent navAgent;

        public GameObject unit;

        public int maxHealth = 5;
        private int currentHealth;
        public HealthBar healthBar;

        void Start()
        {
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
        }

        public int getHealth()
        {
            return currentHealth;
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
            if (getHealth() <= 0)
            {
                // remove highlight if unit is selected
                if (unit.gameObject.GetComponent<Outline>() != null)
                {
                    Destroy(unit.gameObject.GetComponent<Outline>());
                }
                Destroy(unit, 2);
            }
        }

        public void MoveUnit(Vector3 _destination)
        {
            navAgent.SetDestination(_destination);
        }
    }

}


