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
        private GameObject humanMounting;

        Vector3 last_position = Vector3.zero, position;

        void Start()
        {
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
            humanMounting = null;
        }

        private void Update()
        {
            position = unit.GetComponent<Transform>().position;
            if (unit.GetComponent<Transform>().position == last_position)
            {
                unit.GetComponent<Animator>().speed = 0;
            }
            else
            {
                unit.GetComponent<Animator>().speed = 1;
            }
            last_position = position;
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

        public GameObject getHumanMoutinginTheHorse()
        {
            return humanMounting;
        }

        public void setHumanMoutinginTheHorse(GameObject human)
        {
            humanMounting = human;
        }

    }

}


