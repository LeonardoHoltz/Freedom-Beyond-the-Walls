﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using FBTW.Units.Player;

namespace FBTW.Units.Titans
{
    public class TitanUnit : MonoBehaviour
    {
        public Transform attackPoint;
        public float attackRange = 3.0f;
        public LayerMask unitsLayers;

        public NavMeshAgent navAgent;


        // Update is called once per frame
        void Update()
        {
            // temporary key for attack
            if (Input.GetKeyDown(KeyCode.K))
            {
                TitanAttack();
            }
        }
        public void TitanAttack()
        {
            // Play animation

            // Detect Enemies in attack range
            Collider[] hitUnits = Physics.OverlapSphere(attackPoint.position, attackRange, unitsLayers);

            // Damage them
            foreach (Collider unit in hitUnits)
            {
                if(unit.tag == "HumanUnit")
                {
                    PlayerUnit pU = unit.transform.gameObject.GetComponent<PlayerUnit>();
                    pU.TakeDamage(5);
                }
                if (unit.tag == "HorseUnit")
                {
                    HorseUnit hU = unit.transform.gameObject.GetComponent<HorseUnit>();
                    hU.TakeDamage(5);
                }

            }
        }

        private void OnDrawGizmosSelected()
        {
            if (attackPoint == null)
                return;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
