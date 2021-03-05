using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

namespace FBTW.Units.Player
{
    
    public class PlayerUnit : MonoBehaviour
    {
        public NavMeshAgent navAgent;
        Vector3 oldPos, newPos;

        public int foodCost;
        public int gasCost;
        public int bladeCost;

        
        public void MoveUnit(Vector3 _destination)
        {
            navAgent.SetDestination(_destination);
        }
    }
    
}


