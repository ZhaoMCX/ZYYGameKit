using DG.Tweening;
using UnityEngine;

namespace ZYYGameKit.UI
{
    public class BaseUIAnim : MonoBehaviour
    {
        protected Vector3 startPosition;

        protected virtual void Awake()
        {
            startPosition = transform.localPosition;
        }

        public virtual Sequence PlayAnim()
        {
            return DOTween.Sequence();
        }
        public virtual Sequence PlayRewindAnim()
        {
            return DOTween.Sequence();
        }
        public virtual void ResetAnim()
        {
           
        }
    }
}