using System.Collections.Generic;
using UnityEngine;
namespace ZYYGameKit.Combat
{
    /// <summary>
    /// 技能定义部分，定义了技能的数据结构，和处理逻辑。根据触发条件分类，技能负责持久的，流程性的触发条件，监听输入，处理消耗
    /// </summary>


    public enum SkillStateEnum
    {
        Ready,
        Charging,
        Casting,
        Cooling
    }

    /// <summary>
    /// 技能运行数据类
    /// </summary>
    public class SkillInfo
    {
        public GameObject Owner;
        public SkillConfig SkillConfig;
        public SkillStateEnum SkillState = SkillStateEnum.Ready;
        List<SkillEffect> skillEffects = new List<SkillEffect>();
        float curChargeTimer;
        float curTimer;
        float curCooldownTimer;
        //最后一次激活的技能效果，用于终止判断
        SkillEffect lastSkillEffect;
        //蓄力状态的监听器，监听蓄力取消事件
        SmartEvent lister;

        public void OnUpdate(float deltaTime)
        {
            if (SkillState == SkillStateEnum.Ready) return;
            //根据时间缩放计算当前的时间
            float curDeltaTime = deltaTime * SkillConfig.TimeScale;
            curTimer += curDeltaTime;
            //根据状态进行逻辑处理
            switch (SkillState)
            {
                //蓄力状态
                case SkillStateEnum.Charging:
                    curChargeTimer += curDeltaTime;
                    curTimer += curDeltaTime;
                    //如果当前时间大于循环结束时间，则回到循环开始时间
                    if (curTimer >= SkillConfig.ChargeLoopEndTime)
                    {
                        curTimer = SkillConfig.ChargeLoopStartTime;
                    }
                    //如果当前时间大于最大蓄力时间，则取消技能
                    if (curChargeTimer >= SkillConfig.ChargeMaxTime)
                    {
                        CancelSkill();
                        return;
                    }
                    //找到当前时间对应的技能效果
                    var skillEffect = skillEffects.FindLast(x => x.TriggerTime <= curTimer);
                    //如果没有找到，则返回
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
                //释放状态
                case SkillStateEnum.Casting:
                    curTimer += curDeltaTime;
                    //如果当前时间大于总时间，则结束技能
                    if (curTimer >= SkillConfig.TotalTime)
                    {
                        EndSkill();
                        return;
                    }
                    //找到当前时间对应的技能效果
                    skillEffect = skillEffects.FindLast(x => x.TriggerTime <= curTimer);
                    //如果当前技能效果和上次技能效果相同，则返回
                    if (lastSkillEffect == skillEffect)
                    {
                        return;
                    }
                    //如果有技能效果，则执行
                    if (skillEffect != null)
                    {
                        foreach (var effect in skillEffect.Effects)
                        {
                            effect.Apply(this);
                        }
                    }
                    lastSkillEffect = skillEffect;
                    break;
                //冷却状态
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
            CommitCost();
            SkillState = SkillStateEnum.Casting;
            Debug.Log("技能激活");
        }
        
        /// <summary>
        /// 进入蓄力状态
        /// </summary>
        /// <param name="smartEvent">蓄力监听的事件</param>
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


        /// <summary>
        /// 当蓄力操作释放时
        /// </summary>
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
        
        /// <summary>
        /// 是否能够释放技能，可以在这里加入能量消耗条件之类的
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 提交能量消耗，例如扣除MP之类的
        /// </summary>
        void CommitCost()
        {
            
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