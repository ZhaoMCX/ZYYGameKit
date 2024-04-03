using System.Collections.Generic;
using UnityEngine;

namespace ZYYGameKit.Combat
{
    /// <summary>
    /// 技能处理类
    /// </summary>
    public class SkillHandler : MonoBehaviour
    {
        //当前拥有的技能
        List<SkillInfo> skills = new List<SkillInfo>();

        public void AddSkill(SkillInfo skillInfo)
        {
            if (skills.Find(x => x.SkillConfig.SkillId == skillInfo.SkillConfig.SkillId) != null) return;
            skills.Add(skillInfo);
        }
        
        public void RemoveSkill(SkillInfo skillInfo)
        {
            skills.Remove(skillInfo);
        }

        public void UseSkill(SmartEvent smartEvent)
        {
            var skillInfo = skills[0];
            UseSkill(skillInfo,smartEvent);
        }
        
        public void UseSkill(SkillInfo skillInfo,SmartEvent smartEvent)
        {
            skillInfo.ActiveSkill(smartEvent);
        }
        
        void Update()
        {
            foreach (var skill in skills)
            {
                skill.OnUpdate(Time.deltaTime);
            }
        }
    }
}