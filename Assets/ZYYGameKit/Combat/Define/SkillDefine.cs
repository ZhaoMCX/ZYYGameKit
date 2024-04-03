using System.Collections.Generic;
using UnityEngine;
namespace ZYYGameKit.Combat
{

    public enum SkillStateEnum
    {
        Ready,
        Charging,
        Casting,
        Cooling
    }

    public class SkillInfo
    {
        public GameObject Owner;
        public SkillConfig SkillConfig;
        public SkillStateEnum SkillState = SkillStateEnum.Ready;
        List<SkillEffect> skillEffects = new List<SkillEffect>();
        float curChargeTimer;
        float curTimer;
        float curCooldownTimer;
        SkillEffect lastSkillEffect;
        SmartEvent lister;

        public void OnUpdate(float deltaTime)
        {
            if (SkillState == SkillStateEnum.Ready) return;
            float curDeltaTime = deltaTime * SkillConfig.TimeScale;
            curTimer += curDeltaTime;
            switch (SkillState)
            {
                case SkillStateEnum.Charging:
                    curChargeTimer += curDeltaTime;
                    curTimer += curDeltaTime;
                    if (curTimer >= SkillConfig.ChargeLoopEndTime)
                    {
                        curTimer = SkillConfig.ChargeLoopStartTime;
                    }
                    if (curChargeTimer >= SkillConfig.ChargeMaxTime)
                    {
                        CancelSkill();
                        return;
                    }
                    var skillEffect = skillEffects.FindLast(x => x.TriggerTime <= curTimer);
                    if (skillEffect == null || skillEffect.TriggerTime < SkillConfig.ChargeLoopStartTime)
                    {
                        return;
                    }

                    foreach (var effect in skillEffect.Effects)
                    {
                        effect.Apply(this);
                    }
                    lastSkillEffect = skillEffect;
                    break;
                case SkillStateEnum.Casting:
                    curTimer += curDeltaTime;
                    if (curTimer >= SkillConfig.TotalTime)
                    {
                        EndSkill();
                        return;
                    }
                    skillEffect = skillEffects.FindLast(x => x.TriggerTime <= curTimer);
                    if (lastSkillEffect == skillEffect)
                    {
                        return;
                    }
                    if (skillEffect != null)
                    {
                        foreach (var effect in skillEffect.Effects)
                        {
                            effect.Apply(this);
                        }
                    }
                    lastSkillEffect = skillEffect;
                    break;
                case SkillStateEnum.Cooling:
                    curCooldownTimer -= curDeltaTime;
                    if (curCooldownTimer <= 0)
                    {
                        curCooldownTimer = 0;
                        SkillState = SkillStateEnum.Ready;
                    }
                    break;
            }

        }

        public void EnterCasting()
        {
            if (SkillConfig.IsCharge)
            {
                curTimer = SkillConfig.ChargeLoopEndTime;
            }
            SkillState = SkillStateEnum.Casting;
            Debug.Log("技能激活");
        }
        
        public void EnterCharging(SmartEvent smartEvent)
        {
            lister = smartEvent;
            lister.Register(OnChargeCancel);
            SkillState = SkillStateEnum.Charging;
        }
        
        public void EnterCooling()
        {
            curCooldownTimer = SkillConfig.Cooldown;
            SkillState = SkillStateEnum.Cooling;
            lastSkillEffect = null;
            curChargeTimer = 0;
            curTimer = 0;
            if (lister != null)
            {
                lister.UnRegister(OnChargeCancel);
                lister = null;
            }
        }

        public void ActiveSkill(SmartEvent smartEvent)
        {
            if (SkillConfig.IsCharge)
            {
                EnterCharging(smartEvent);
            }
            else
            {
                if (CanCastSkill())
                {
                   EnterCasting();
                }
            }
        }

        public void EndSkill()
        {
           EnterCooling();
           Debug.Log("技能结束");
        }

        public void CancelSkill()
        {
            EnterCooling();
            Debug.Log("技能取消");
        }


        public void OnChargeCancel()
        {
            if (CanCastSkill())
            {
                EnterCasting();
            }
            else
            {
                CancelSkill();
            }
        }

        bool CanCastSkill()
        {
            if (SkillConfig.IsCharge)
            {
                if (curChargeTimer >= SkillConfig.ChargeMinTime && curChargeTimer <= SkillConfig.ChargeMaxTime)
                {
                    return true;
                }
                return false;
            }
            return true;
        }


        public SkillInfo(SkillConfig skillConfig, GameObject owner)
        {
            Owner = owner;
            this.SkillConfig = skillConfig;
            foreach (var skillEffect in skillConfig.SkillEffects)
            {
                skillEffects.Add(skillEffect);
            }
            SkillState = SkillStateEnum.Ready;
        }
    }


    /// <summary>
    /// 根据触发条件分类，技能负责持久的，流程性的触发条件，监听输入，处理消耗
    /// </summary>
    public class SkillConfig
    {
        public int SkillId;
        public float Cooldown;
        public float TotalTime;
        public float TimeScale;
        public bool IsCharge;
        public float ChargeMinTime;
        public float ChargeMaxTime;
        public float ChargeLoopStartTime;
        public float ChargeLoopEndTime;
        public SkillEffect[] SkillEffects;
    }

    public class SkillEffect
    {
        //触发时间点
        public float TriggerTime;
        //触发效果
        public AbstractEffect[] Effects;
    }

}