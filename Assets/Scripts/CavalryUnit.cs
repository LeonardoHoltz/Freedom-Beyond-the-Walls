﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
using FBTW.InputManager;
using FBTW.Player;

namespace FBTW.Units.Player
{

    public class CavalryUnit : MonoBehaviour
    {
        public NavMeshAgent navAgent;

        public GameObject unit, horse, respectiveHuman;

        public static float m_attackRange = 10.0f;

        public int currentAgility;
        public int modifier = 0;
        public int initAgility = 150;

        private int currentHealth;
        public int maxHealth = 10;

        public int currentMaxHealth;

        public HealthBar healthBar;

        private bool m_isMovingToAttack, m_isMovingToEnemy, m_isAttacking;

        Vector3 last_position = Vector3.zero, position;

        void Start()
        {

            currentAgility = PlayerManager.instance.m_agility;
            currentHealth = PlayerManager.instance.maxHealthCavalry;
            currentMaxHealth = PlayerManager.instance.maxHealthCavalry;
            healthBar.SetMaxHealth(PlayerManager.instance.maxHealthCavalry);
        }

        private void Update()
        {
            // Animation:
            Animator[] animators;
            position = unit.GetComponent<Transform>().position;
            if (unit.GetComponent<Transform>().position == last_position)
            {
                animators = unit.GetComponentsInChildren<Animator>();
                foreach(Animator animator in animators)
                {
                    animator.speed = 0;
                }
            }
            else
            {
                animators = unit.GetComponentsInChildren<Animator>();
                foreach (Animator animator in animators)
                {
                    animator.speed = 1;
                }
            }
            last_position = position;
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

        public GameObject getHuman()
        {
            return respectiveHuman;
        }

        public GameObject getHorse()
        {
            return horse;
        }

    }

}


