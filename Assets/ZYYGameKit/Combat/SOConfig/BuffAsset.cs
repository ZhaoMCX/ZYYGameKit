using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ZYYGameKit.Combat.SOConfig
{
    [CreateAssetMenu(fileName = "_BuffAsset", menuName = "Combat/BuffAsset")]
    public class BuffAsset : SerializedScriptableObject
    {
        [LabelText("Id")]
        public int BuffId;
        [LabelText("优先级")]
        public int Priority;
        [LabelText("是否永久")]
        public bool IsPermanent;
        [LabelText("持续时间")]
        [HideIf("IsPermanent")]
        public float Duration;
        [LabelText("添加时冲突时间修改方式")]
        [EnumToggleButtons]
        public AddTimeChangeEnum AddTimeChange;
        [LabelText("是否可叠加")]
        public bool IsStack;
        [ShowIf("IsStack")]
        [LabelText("最大层数")]
        public int MaxStack;
        [ShowIf("IsStack")]
        [LabelText("时间耗尽后层数修改方式")]
        [EnumToggleButtons]
        public TimeOverStackChangeEnum TimeOverStackChange;
        [LabelText("Buff效果列表")]
        [TypeFilter("GetFilteredTypeList")]
        public BuffEffectAsset[] BuffEffects;

        public IEnumerable<Type> GetFilteredTypeList()
        {
            var q = typeof(AbstractEffect).Assembly.GetTypes()
                .Where(x => typeof(BuffEffectAsset)==x); // Excludes classes not inheriting from BaseClass// 排除泛型定义
            return q;
        }
    }
}