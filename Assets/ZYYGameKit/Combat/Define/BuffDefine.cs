using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
namespace ZYYGameKit.Combat
{
    public class BuffInfo
    {
        class PeriodBuffEffect
        {
            public float TriggerTime;
            public BuffEffect Effect;
            public PeriodBuffEffect(float triggerTime, BuffEffect effect)
            {
                TriggerTime = triggerTime;
                Effect = effect;
            }
        }
        
        public BuffConfig BuffConfig;
        public GameObject Creator;
        public GameObject Target;
        public float CurDuration;
        public int CurStack;
        List<PeriodBuffEffect> periodBuffEffects;
        List<BuffEffect> createBuffEffects;
        List<BuffEffect> removeBuffEffects;
        List<BuffEffect> stackChangeBuffEffects;


        public BuffInfo(BuffConfig buffConfig, GameObject target, GameObject creator = null)
        {
            BuffConfig = buffConfig;
            Target = target;
            Creator = creator;
            foreach (var buffConfigBuffEffect in buffConfig.BuffEffects)
            {
                if ((buffConfigBuffEffect.TriggerTiming & TriggerTimingEnum.Create) == TriggerTimingEnum.Create)
                {
                    if (createBuffEffects == null) createBuffEffects = new List<BuffEffect>();
                    createBuffEffects.Add(buffConfigBuffEffect);
                }
                if ((buffConfigBuffEffect.TriggerTiming & TriggerTimingEnum.Remove) == TriggerTimingEnum.Remove)
                {
                    if (removeBuffEffects == null) removeBuffEffects = new List<BuffEffect>();
                    removeBuffEffects.Add(buffConfigBuffEffect);
                }
                if ((buffConfigBuffEffect.TriggerTiming & TriggerTimingEnum.Period) == TriggerTimingEnum.Period)
                {
                    if (periodBuffEffects == null) periodBuffEffects = new List<PeriodBuffEffect>();
                    periodBuffEffects.Add(new PeriodBuffEffect(0, buffConfigBuffEffect));
                }
                if ((buffConfigBuffEffect.TriggerTiming & TriggerTimingEnum.StackChange) == TriggerTimingEnum.StackChange)
                {
                    if (stackChangeBuffEffects == null) stackChangeBuffEffects = new List<BuffEffect>();
                    stackChangeBuffEffects.Add(buffConfigBuffEffect);
                }
            }
            CurDuration = buffConfig.Duration;
            CurStack = 1;
        }

        public bool OnUpdate(float elapseTime)
        {
            HandlePeriodEffect(elapseTime);
            if (!BuffConfig.IsPermanent)
            {
                if (CurDuration <= 0) return false;
                CurDuration -= Time.deltaTime;
            }
            return true;
        }

        public void HandlePeriodEffect(float elapseTime)
        {
            if (periodBuffEffects == null) return;
            foreach (var periodBuffEffect in periodBuffEffects)
            {
                if (elapseTime <= periodBuffEffect.TriggerTime) continue;
                foreach (var abstractEffect in periodBuffEffect.Effect.Effects)
                {
                    abstractEffect.Apply(this);
                }
                periodBuffEffect.TriggerTime = elapseTime + periodBuffEffect.Effect.PeriodTime;
            }
        }

        public void HandleCreateEffect()
        {
            if (createBuffEffects == null) return;
            foreach (var buffEffect in createBuffEffects)
            {
                foreach (var effect in buffEffect.Effects)
                {
                    effect.Apply(this);
                }
            }
        }

        public void HandleRemoveEffect()
        {
            if (removeBuffEffects == null) return;
            foreach (var buffEffect in removeBuffEffects)
            {
                foreach (var effect in buffEffect.Effects)
                {
                    effect.Apply(this);
                }
            }
        }
        public void HandleStackChangeEffect()
        {
            if (stackChangeBuffEffects == null) return;
            foreach (var buffEffect in stackChangeBuffEffects)
            {
                foreach (var effect in buffEffect.Effects)
                {
                    effect.Apply(this);
                }
            }
        }
    }

    /// <summary>
    /// buff负责短暂的，单一性的触发条件，指定触发时机，不监听输入，不处理消耗
    /// </summary>
    public class BuffConfig
    {
        public int BuffId;
        public int Priority;
        public bool IsPermanent;
        public float Duration;
        public bool IsStack;
        public int MaxStack;
        public AddTimeChangeEnum AddTimeChange;
        public TimeOverStackChangeEnum TimeOverStackChange;
        public BuffEffect[] BuffEffects;
    }

    public class BuffEffect
    {
        //触发时机
        public TriggerTimingEnum TriggerTiming;
        //周期时间
        public float PeriodTime;
        //触发效果
        public AbstractEffect[] Effects;
    }

    public enum AddTimeChangeEnum
    {
        [LabelText("刷新时间")]
        Refresh,
        [LabelText("叠加时间")]
        Add,
    }

    public enum TimeOverStackChangeEnum
    {
        [LabelText("清除层数")]
        Clear,
        [LabelText("递减层数")]
        Reduce
    }

    [System.Flags]
    public enum TriggerTimingEnum
    {
        [LabelText("周期")]
        Period = 1 << 1,
        [LabelText("创建时")]
        Create = 1 << 2,
        [LabelText("移除时")]
        Remove = 1 << 3,
        [LabelText("层数变化时")]
        StackChange = 1 << 4,
        [LabelText("全选")]
        All = Period | Create | Remove | StackChange
    }
}