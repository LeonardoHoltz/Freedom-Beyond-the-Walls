using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FBTW.Game;


namespace FBTW.City
{

    public class CityManager : MonoBehaviour
    {
        public static CityManager instance; // Singleton
        public GameObject cityWalls;

        public LayerMask unitsLayers;

        public int currentHealth = 75;

        public int currentMaxHealth = 75;

        // Start is called before the first frame update
        void Start()
        {
            instance = this;
        }


        public int getHealth()
        {
            return currentHealth;
        }

        public int getMaxHealth()
        {
            return currentMaxHealth;
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            if (getHealth() <= 0)
            {
                GameManager.instance.EndGame();
            }
        }
    }

}
