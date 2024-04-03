using UnityEngine;
namespace ZYYGameKit.Combat
{
    /// <summary>
    /// 效果，技能或buff触发效果后的逻辑
    /// </summary>
    public interface IEffect
    {
        public void Apply(object target);
    }

    public abstract class AbstractEffect : IEffect
    {
        public abstract void Apply(object target);
    }
    
  
    public class PrintEffect : AbstractEffect
    {
        public string Message;
        public override void Apply(object target)
        {
            Debug.Log(Message);
        }
    }
}