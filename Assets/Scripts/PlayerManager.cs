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
            foreach (Transform child in playerUnits)
            {
                if (child.name == "Survey Corps")
                {
                    foreach (Transform unit in child)
                    {
                        if (unit.gameObject.tag == "HumanUnit")
                        {
                            PlayerUnit pU = unit.gameObject.GetComponent<PlayerUnit>();
                            switch (e.skillType)
                            {
                                case PlayerSkills.SkillType.Agility_1:
                                    {
                                        pU.setAgility(125);
                                        break;
                                    }
                                case PlayerSkills.SkillType.Agility_2:
                                    {                                       
                                        pU.setAgility(150);
                                        break;
                                    }
                                case PlayerSkills.SkillType.MaxHealth_1:
                                    {                                     
                                        pU.setMaximumHealth(10);
                                        pU.setCurrentHealth(10);
                                        break;
                                    }
                                case PlayerSkills.SkillType.MaxHealth_2:
                                    {
                                        Debug.Log("hello");
                                        pU.setMaximumHealth(15);
                                        pU.setCurrentHealth(15);
                                        break;
                                    }
                            }                            
                        }
                    }
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


