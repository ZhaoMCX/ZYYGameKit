
using System.Collections.Generic;
using UnityEngine;
namespace ZYYGameKit.Combat
{



    public class BuffHandler : MonoBehaviour
    {
        List<BuffInfo> buffs = new List<BuffInfo>();
        List<BuffInfo> removeBuffs = new List<BuffInfo>();
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
        void HandleAddBuffs()
        {
            if (addBuffs.Count == 0) return;
            foreach (var buffInfo in addBuffs)
            {
                var find = buffs.Find(x => x.BuffConfig.BuffId == buffInfo.BuffConfig.BuffId);
                if (find == null)
                {
                    buffInfo.HandleCreateEffect();
                    buffs.Add(buffInfo);
                }
                else
                {
                    switch (find.BuffConfig.AddTimeChange)
                    {
                        case AddTimeChangeEnum.Add:
                            find.CurDuration += buffInfo.CurDuration;
                            break;
                        case AddTimeChangeEnum.Refresh:
                            find.CurDuration = buffInfo.CurDuration;
                            break;
                    }
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