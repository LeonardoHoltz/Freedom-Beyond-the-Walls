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

        public int maxHealthHuman = 5;
        public int maxHealthCavalry = 10;
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
                        maxHealthHuman = 10;
                        maxHealthCavalry = 15;
                        break;
                    }
                case PlayerSkills.SkillType.MaxHealth_2:
                    {
                        maxHealthHuman = 15;
                        maxHealthCavalry = 20;
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
                            Collider[] hitColliders = Physics.OverlapSphere(unit.position, 5f);
                            pU.modifier = 0;
                            foreach (var hitCollider in hitColliders)
                            {
                                
                                if(hitCollider.gameObject.name.Contains("Tree"))
                                {
                                    pU.modifier = 50;
                                }
                            }
                            if (pU.getMovingToAttack())
                            {
                                InputHandler.instance.BeginAttack(unit);
                            }
                            if (pU.getWantsToRideHorse())
                            {
                                pU.RideHorse();
                            }
                            if (pU.currentMaxHealth != maxHealthHuman)
                            {
                                pU.setCurrentHealth(maxHealthHuman);
                                pU.setMaximumHealth(maxHealthHuman);
                            }
                            if(pU.getAgility() != m_agility + pU.modifier)
                            {
                                pU.setAgility(m_agility + pU.modifier);
                            }
                        }
                        if (unit.gameObject.tag == "CavalryUnit")
                        {
                            CavalryUnit cU = unit.gameObject.GetComponent<CavalryUnit>();
                            Collider[] hitColliders = Physics.OverlapSphere(unit.position, 5f);
                            cU.modifier = 50;
                            
                            foreach (var hitCollider in hitColliders)
                            {
                                if (hitCollider.gameObject.name.Contains("Tree"))
                                {
                                    cU.modifier = 0;
                                }
                            }
                            if (cU.getMovingToAttack())
                            {
                                InputHandler.instance.BeginAttack(unit);
                            }
                            if (cU.currentMaxHealth != maxHealthCavalry)
                            {
                                cU.setCurrentHealth(maxHealthCavalry);
                                cU.setMaximumHealth(maxHealthCavalry);
                            }
                            if (cU.getAgility() != m_agility + cU.modifier)
                            {
                                cU.setAgility(m_agility + cU.modifier);
                            }
                        }
                    }
                    
                    
                }
            }
            HUD.HUD.instance.SetUnitCount(m_playerUnitCount);
            if(UI_SkillTree.instance)
            {
                UI_SkillTree.instance.SetPlayerSkills(GetPlayerSkills());
            }

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


