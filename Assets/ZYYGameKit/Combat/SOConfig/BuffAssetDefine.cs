using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
namespace ZYYGameKit.Combat.SOConfig
{
    public class BuffEffectAsset
    {
        [LabelText("是否启用")]
        public bool Enable  = true;
        [LabelText("触发时机")]
        [EnumToggleButtons]
        public TriggerTimingEnum TriggerTiming;
        [LabelText("周期时间")]
        [ShowIf("@(TriggerTiming & TriggerTimingEnum.Period) == TriggerTimingEnum.Period")]
        public float PeriodTime;
        [LabelText("触发效果列表")]
        [TypeFilter("GetFilteredTypeList")]
        public AbstractEffect[] Effects;

        public IEnumerable<Type> GetFilteredTypeList()
        {
            var q = typeof(AbstractEffect).Assembly.GetTypes()
                .Where(x => !x.IsAbstract) // Excludes BaseClass
                .Where(x => !x.IsGenericTypeDefinition) // Excludes T
                .Where(x => typeof(AbstractEffect).IsAssignableFrom(x)); // Excludes classes not inheriting from BaseClass
            return q;
        }
    }
    
    
}