using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using FBTW.Units.Player;
using FBTW.Enemies;
using FBTW.Resources;

namespace FBTW.Units.Titans
{
    public class TitanUnit : MonoBehaviour
    {
        public Transform attackPoint;
        public float attackRange = 3.0f;
        public LayerMask unitsLayers;

        public NavMeshAgent navAgent;

        public GameObject titan;

        public int maxHealth;
        private int currentHealth;
        public HealthBar healthBar;

        private bool searchingForHumans = true;
        private float titanVisionRange = 20f;

        public float attackDelay = 4.0f;
        public float attackTime = 0.0f;
        public bool attackInProgression = false;

        void Start()
        {
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
            gameObject.GetComponent<AudioSource>().Play();
        }

        // Update is called once per frame
        void Update()
        {
            // temporary key for attack
            if (Input.GetKeyDown(KeyCode.K))
            {
                TitanAttack();
            }


        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
            if (getHealth() <= 0)
            {
                Destroy(titan, 2);
                ResourceManagement.IncreaseXP(500);
            }
        }

        public int getHealth()
        {
            return currentHealth;
        }

        public float getVisionRange()
        {
            return titanVisionRange;
        }

        public bool isSearchingForHumans()
        {
            return searchingForHumans;
        }

        public void setSearchingForHumans(bool value)
        {
            searchingForHumans = value;
        }

        public void MoveTitan(Vector3 destination)
        {
            navAgent.SetDestination(destination);
        }

        public bool IsEnemyInAttackRange()
        {
            Collider[] hitUnits = Physics.OverlapSphere(attackPoint.position, attackRange, unitsLayers);
            if(hitUnits.Length > 0)
            {
                return true;
            }
            return false;
        }

        public void TitanAttack()
        {
            // Play animation
            //titan.GetComponent<Animator>().SetBool("isAttacking", true);

            AgentNotMoving(true);
            attackInProgression = true;

            attackTime += Time.deltaTime;

            if(attackTime >= attackDelay)
            {
                // Detect Enemies in attack range
                Collider[] hitUnits = Physics.OverlapSphere(attackPoint.position, attackRange, unitsLayers);

                // Damage them
                foreach (Collider unit in hitUnits)
                {
                    if (unit.tag == "HumanUnit")
                    {
                        PlayerUnit pU = unit.transform.gameObject.GetComponent<PlayerUnit>();
                        pU.TakeDamage(5);
                    }
                    if (unit.tag == "HorseUnit")
                    {
                        HorseUnit hU = unit.transform.gameObject.GetComponent<HorseUnit>();
                        hU.TakeDamage(5);
                    }
                    if (unit.tag == "CavalryUnit")
                    {
                        CavalryUnit cU = unit.transform.gameObject.GetComponent<CavalryUnit>();
                        cU.TakeDamage(5);
                    }

                }
                attackTime = 0f;
                attackInProgression = false;
                
            }
        }

        public void AgentNotMoving(bool value)
        {
            navAgent.isStopped = value;
        }

        private void OnDrawGizmosSelected()
        {
            if (attackPoint == null)
                return;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }


    }
}
