using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
namespace ZYYGameKit.Combat.SOConfig
{


    [CreateAssetMenu(fileName = "_SkillAsset", menuName = "Combat/SkillAsset")]
    public class SkillAsset : SerializedScriptableObject
    {
        [LabelText("Id")]
        public int SkillId;
        [LabelText("冷却时间")]
        public float Cooldown = 0.1f;
        [LabelText("总时长")]
        public float TotalTime;
        [LabelText("时间缩放")]
        public float TimeScale = 1f;
        [LabelText("是否蓄力")]
        public bool IsCharge;
        [ShowIf("IsCharge")]
        [LabelText("最小蓄力时间")]
        public float ChargeMinTime;
        [ShowIf("IsCharge")]
        [LabelText("最大蓄力时间")]
        public float ChargeMaxTime;
        [ShowIf("IsCharge")]
        [LabelText("蓄力循环开始点")]
        public float ChargeLoopStartTime;
        [ShowIf("IsCharge")]
        [LabelText("蓄力循环结束点")]
        public float ChargeLoopEndTime;
        [LabelText("技能效果列表")]
        [TypeFilter("GetFilteredTypeList")]
        public SkillEffectAsset[] SkillEffects;

        public IEnumerable<Type> GetFilteredTypeList()
        {
            var q = typeof(SkillEffectAsset).Assembly.GetTypes()
                .Where(x => typeof(SkillEffectAsset) == x); // Excludes classes not inheriting from BaseClass// 排除泛型定义
            return q;
        }
    }
}