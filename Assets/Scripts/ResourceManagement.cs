using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FBTW.Resources
{
    public static class ResourceManagement
    {
        private static int m_Food;
        private static int m_Gas;
        private static int m_Blades;
        private static int m_Horse;

        // Getters
        public static int getFood()
        {
            return m_Food;
        }

        public static int getGas()
        {
            return m_Gas;
        }

        public static int getBlades()
        {
            return m_Blades;
        }

        public static int getHorse()
        {
            return m_Horse;
        }

        // Setters
        public static void setFood(int food)
        {
            m_Food = food;
        }

        public static void setGas(int gas)
        {
            m_Gas = gas;
        }

        public static void setBlades(int blades)
        {
            m_Blades = blades;
        }
        public static void setHorse(int horse)
        {
            m_Horse = horse;
        }

        // Increasing Methods
        public static void IncreaseFood(int food)
        {
            m_Food += food;
        }

        public static void IncreaseGas(int gas)
        {
            m_Gas += gas;
        }

        public static void IncreaseBlade(int blades)
        {
            m_Blades += blades;
        }

        public static void IncreaseHorse(int horse)
        {
            m_Horse += horse;
        }

        // Decreasing Methods
        public static void DecreaseFood(int food)
        {
            m_Food -= food;
        }

        public static void DecreaseGas(int gas)
        {
            m_Gas -= gas;
        }

        public static void DecreaseBlade(int blades)
        {
            m_Blades -= blades;
        }
        public static void DecreaseHorse(int horse)
        {
            m_Horse -= horse;
        }

    }

}
