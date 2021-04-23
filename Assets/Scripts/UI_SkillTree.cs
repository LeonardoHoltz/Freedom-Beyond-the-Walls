using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using FBTW.Skills;



namespace FBTW.SkillTree
{
    public class UI_SkillTree : MonoBehaviour
    {
        public static UI_SkillTree instance; // Singleton

        private PlayerSkills playerSkills;

        void Start()
        {
            instance = this;         
        }

        void Update()
        {



                transform.Find("Agility_1Btn").GetComponent<Button>().onClick.AddListener(() =>
                {
                    this.playerSkills.TryUnlockSkill(Skills.PlayerSkills.SkillType.Agility_1);
                });
                transform.Find("Agility_2Btn").GetComponent<Button>().onClick.AddListener(() =>
                {
                    this.playerSkills.TryUnlockSkill(Skills.PlayerSkills.SkillType.Agility_2);
                });
                transform.Find("MaxHealth_1Btn").GetComponent<Button>().onClick.AddListener(() =>
                {
                    this.playerSkills.TryUnlockSkill(Skills.PlayerSkills.SkillType.MaxHealth_1);
                });
                transform.Find("MaxHealth_2Btn").GetComponent<Button>().onClick.AddListener(() =>
                {
                    this.playerSkills.TryUnlockSkill(Skills.PlayerSkills.SkillType.MaxHealth_2);
                });
                transform.Find("HorseUnlockBtn").GetComponent<Button>().onClick.AddListener(() =>
                {
                    this.playerSkills.TryUnlockSkill(Skills.PlayerSkills.SkillType.HorseUnlock);
                });
        }

        public void SetPlayerSkills(Skills.PlayerSkills playerSkills)
        {
            this.playerSkills = playerSkills;
        }
    }
}
