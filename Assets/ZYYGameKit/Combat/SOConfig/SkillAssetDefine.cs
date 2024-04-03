using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
namespace ZYYGameKit.Combat
{
    public class SkillEffectAsset
    {
        [LabelText("是否启用")]
        public bool Enable = true;
        //触发时间点
        [LabelText("触发时间点")]
        public float TriggerTime;
        //触发效果
        [LabelText("触发效果列表")]
        [TypeFilter("GetFilteredTypeList")]
        public AbstractEffect[] Effects;
        
        public IEnumerable<Type> GetFilteredTypeList()
        {
            var q = typeof(AbstractEffect).Assembly.GetTypes()
                .Where(x => !x.IsAbstract) // Excludes BaseClass
                .Where(x => !x.IsGenericTypeDefinition) // Excludes T
                .Where(x => !x.IsInterface)
                .Where(x => typeof(AbstractEffect).IsAssignableFrom(x)); // Excludes classes not inheriting from BaseClass
            return q;
        }
    }
}