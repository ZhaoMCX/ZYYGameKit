using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using ZYYGameKit;
using ZYYGameKit.Combat;
using ZYYGameKit.Combat.SOConfig;

public class Test : MonoBehaviour
{
    public BuffAsset buffAsset;
    public BuffHandler BuffHandler;
    public SkillAsset skillAsset;
    public SkillHandler SkillHandler;
    SmartEvent holdCancel = new SmartEvent();

    [Button]
    public void AddBuff()
    {
        var buffConfig = new BuffConfig();
        buffConfig.BuffId = buffAsset.BuffId;
        buffConfig.Duration = buffAsset.Duration;
        buffConfig.IsStack = buffAsset.IsStack;
        buffConfig.MaxStack = buffAsset.MaxStack;
        buffConfig.AddTimeChange = buffAsset.AddTimeChange;
        buffConfig.TimeOverStackChange = buffAsset.TimeOverStackChange;
        List<BuffEffect> buffEffects = new List<BuffEffect>();
        foreach (var buffAssetBuffEffect in buffAsset.BuffEffects)
        {
            if (buffAssetBuffEffect.Enable)
            {
                var buffEffect = new BuffEffect();
                buffEffect.Effects = buffAssetBuffEffect.Effects;
                buffEffect.PeriodTime = buffAssetBuffEffect.PeriodTime;
                buffEffect.TriggerTiming = buffAssetBuffEffect.TriggerTiming;
                buffEffects.Add(buffEffect);
            }
        }
        buffConfig.BuffEffects = buffEffects.ToArray();
        BuffHandler.AddBuff(new BuffInfo(buffConfig, gameObject));
    }
    
    [Button]
    public void AddSKill()
    {
        var skillConfig = new SkillConfig();
        skillConfig.SkillId = skillAsset.SkillId;
        skillConfig.Cooldown = skillAsset.Cooldown;
        skillConfig.IsCharge = skillAsset.IsCharge;
        skillConfig.ChargeMaxTime = skillAsset.ChargeMaxTime;
        skillConfig.ChargeMinTime = skillAsset.ChargeMinTime;
        skillConfig.ChargeLoopEndTime = skillAsset.ChargeLoopEndTime;
        skillConfig.ChargeLoopStartTime = skillAsset.ChargeLoopStartTime;
        skillConfig.TimeScale = skillAsset.TimeScale;
        skillConfig.TotalTime = skillAsset.TotalTime;
        List<SkillEffect> skillEffects = new List<SkillEffect>();
        foreach (var skillAssetSkillEffect in skillAsset.SkillEffects)
        {
            if (skillAssetSkillEffect.Enable)
            {
                var skillEffect = new SkillEffect();
                skillEffect.Effects = skillAssetSkillEffect.Effects;
                skillEffect.TriggerTime = skillAssetSkillEffect.TriggerTime;
                skillEffects.Add(skillEffect);
            }
        }
        skillEffects.Sort((x, y) => x.TriggerTime.CompareTo(y.TriggerTime));
        skillConfig.SkillEffects = skillEffects.ToArray();
        SkillHandler.AddSkill(new SkillInfo(skillConfig, gameObject));
    }

    [Button]
    public void UseSkill()
    {
        SkillHandler.UseSkill(holdCancel);
    }

    [Button]
    public void HoldCancelSkill()
    {
        holdCancel.Trigger();
    }
    
}
