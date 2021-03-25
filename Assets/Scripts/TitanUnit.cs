using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FBTW.Units.Player;

namespace FBTW.Units.Titans
{
    public class TitanUnit : MonoBehaviour
    {
        public Transform attackPoint;
        public float attackRange = 3.0f;
        public LayerMask unitsLayers;


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
                PlayerUnit pU = unit.transform.gameObject.GetComponent<PlayerUnit>();
                pU.TakeDamage(5);
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
