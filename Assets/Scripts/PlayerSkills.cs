using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FBTW.Resources;

namespace FBTW.Skills
{
    public class PlayerSkills
    {
        public event EventHandler<OnSkillUnlockedEventArgs> OnSkillUnlocked;
        public class OnSkillUnlockedEventArgs : EventArgs
        {
            public SkillType skillType;
        }
        public enum SkillType
        {
            None,
            HorseUnlock,
            Agility_1,
            Agility_2,
            MaxHealth_1,
            MaxHealth_2
        }

        private List<SkillType> unlockedSkillTypeList;

        public PlayerSkills()
        {
            unlockedSkillTypeList = new List<SkillType>();
        }

        private void UnlockSkill(SkillType skillType)
        {
            if(!IsSkillUnlocked(skillType))
            {
                unlockedSkillTypeList.Add(skillType);
                OnSkillUnlocked?.Invoke(this, new OnSkillUnlockedEventArgs { skillType = skillType });
                ResourceManagement.DecreaseLevel(1);
            }
                
        }

        public bool IsSkillUnlocked(SkillType skillType)
        {
            return unlockedSkillTypeList.Contains(skillType);
        }

        public SkillType GetSkillRequirements(SkillType skilltype)
        {
            switch(skilltype)
            {
                case SkillType.Agility_2: return SkillType.Agility_1;
                case SkillType.MaxHealth_2: return SkillType.MaxHealth_1;
            }
            return SkillType.None;
        }

        public void TryUnlockSkill(SkillType skilltype)
        {
            SkillType skillRequirement = GetSkillRequirements(skilltype);
            if(ResourceManagement.getLevel() > 0)
            {
                if (skillRequirement != SkillType.None)
                {
                    if (IsSkillUnlocked(skillRequirement))
                    {
                        UnlockSkill(skilltype);
                        
                    }
                }
                else
                {
                    UnlockSkill(skilltype);
                }
            }

        }
    }

}