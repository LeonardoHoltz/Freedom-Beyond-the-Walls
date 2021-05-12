using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FBTW.InputManager;
using FBTW.HUD;
using FBTW.Units.Player;
using FBTW.Skills;
using FBTW.SkillTree;

namespace FBTW.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager instance; // Singleton

        public Transform playerUnits;

        public int m_agility = 100;

        public int maxHealth = 5;
        private int m_playerUnitCount;

        private Skills.PlayerSkills playerSkills;


        // Start is called before the first frame update
        void Start()
        {
            instance = this;
            this.playerSkills = new Skills.PlayerSkills();
            playerSkills.OnSkillUnlocked += PlayerSkills_OnSkillUnlocked;

        }
        private void PlayerSkills_OnSkillUnlocked(object sender, PlayerSkills.OnSkillUnlockedEventArgs e)
        {          

            switch (e.skillType)
            {
                case PlayerSkills.SkillType.Agility_1:
                    {
                        m_agility = 125;
                        break;
                    }
                case PlayerSkills.SkillType.Agility_2:
                    {
                        m_agility = 150;
                        break;
                    }
                case PlayerSkills.SkillType.MaxHealth_1:
                    {
                        maxHealth = 10;
                        break;
                    }
                case PlayerSkills.SkillType.MaxHealth_2:
                    {
                        maxHealth = 15;
                        break;
                    }
            }                            
            
        }
        // Update is called once per frame
        void Update()
        {
            InputHandler.instance.HandleUnitMovement();
            foreach(Transform child in playerUnits)
            {
                if(child.name == "Survey Corps")
                {
                    m_playerUnitCount = child.childCount;
                    foreach(Transform unit in child)
                    {
                        if (unit.gameObject.tag == "HumanUnit")
                        {
                            PlayerUnit pU = unit.gameObject.GetComponent<PlayerUnit>();
                            if (pU.getMovingToAttack())
                            {
                                InputHandler.instance.BeginAttack(unit);
                            }
                            if (pU.currentMaxHealth != maxHealth)
                            {
                                pU.setCurrentHealth(maxHealth);
                                pU.setMaximumHealth(maxHealth);
                            }
                            if(pU.getAgility() != m_agility)
                            {
                                pU.setAgility(m_agility);
                            }
                        }
                    }
                    
                    
                }
            }
            HUD.HUD.instance.SetUnitCount(m_playerUnitCount);
            UI_SkillTree.instance.SetPlayerSkills(GetPlayerSkills());
        }


        public Skills.PlayerSkills GetPlayerSkills()
        {
            return playerSkills;
        }

        public bool CanSpawnHorseUnit()
        {
            return playerSkills.IsSkillUnlocked(Skills.PlayerSkills.SkillType.HorseUnlock);
        }
    }
}


