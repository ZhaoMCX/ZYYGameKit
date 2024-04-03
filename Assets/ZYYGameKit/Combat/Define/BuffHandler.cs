using System.Collections.Generic;
using UnityEngine;
namespace ZYYGameKit.Combat
{
    
    /// <summary>
    /// buff处理类
    /// </summary>
    public class BuffHandler : MonoBehaviour
    {
        // 正在有效的buff
        List<BuffInfo> buffs = new List<BuffInfo>();
        // 即将移除的buff
        List<BuffInfo> removeBuffs = new List<BuffInfo>();
        // 即将添加的buff
        List<BuffInfo> addBuffs = new List<BuffInfo>();

        float elapseTime;

        public void AddBuff(BuffInfo buffInfo)
        {
            addBuffs.Add(buffInfo);
        }

        public void RemoveBuff(BuffInfo buffInfo)
        {
            removeBuffs.Add(buffInfo);
        }


        void Update()
        {
            elapseTime += Time.deltaTime;
            HandleAddBuffs();
            HandleBuffs();
            HandleRemoveBuffs();
        }

        void HandleBuffs()
        {
            if (buffs.Count == 0) return;
            foreach (var buffInfo in buffs)
            {
                if (!buffInfo.OnUpdate(elapseTime))
                {
                    RemoveBuff(buffInfo);
                }
            }
        }
        void HandleRemoveBuffs()
        {
            if (removeBuffs.Count == 0) return;
            foreach (var buffInfo in removeBuffs)
            {
                if (!buffInfo.BuffConfig.IsStack || buffInfo.CurStack == 1)
                {
                    buffInfo.HandleRemoveEffect();
                    buffs.Remove(buffInfo);
                }
                else
                {
                    switch (buffInfo.BuffConfig.TimeOverStackChange)
                    {
                        case TimeOverStackChangeEnum.Clear:
                            buffInfo.HandleRemoveEffect();
                            buffs.Remove(buffInfo);
                            break;
                        case TimeOverStackChangeEnum.Reduce:
                            buffInfo.CurStack--;
                            buffInfo.HandleStackChangeEffect();
                            buffInfo.CurDuration = buffInfo.BuffConfig.Duration;
                            break;
                    }
                }
            }
            removeBuffs.Clear();
        }
        
        /// <summary>
        /// 处理待添加的buff
        /// </summary>
        void HandleAddBuffs()
        {
            if (addBuffs.Count == 0) return;
            foreach (var buffInfo in addBuffs)
            {
                //查重
                var find = buffs.Find(x => x.BuffConfig.BuffId == buffInfo.BuffConfig.BuffId);
                if (find == null)
                {
                    buffInfo.HandleCreateEffect();
                    buffs.Add(buffInfo);
                }
                else
                {
                    //根据添加的时间类型修改buff时间
                    switch (find.BuffConfig.AddTimeChange)
                    {
                        case AddTimeChangeEnum.Add:
                            find.CurDuration += buffInfo.CurDuration;
                            break;
                        case AddTimeChangeEnum.Refresh:
                            find.CurDuration = buffInfo.CurDuration;
                            break;
                    }
                    //如果层数不可堆叠或者超过最大层数，则不处理
                    if (!find.BuffConfig.IsStack || find.CurStack >= find.BuffConfig.MaxStack) continue;
                    find.CurStack++;
                    find.HandleStackChangeEffect();
                }
            }
            addBuffs.Clear();
            SortBuffs();
        }

        void SortBuffs()
        {
            //优先级从大到小排序
            buffs.Sort((x, y) => y.BuffConfig.Priority - x.BuffConfig.Priority);
        }
    }
}