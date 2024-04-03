using System;
using UnityEngine;

namespace ZYYGameKit
{
    public interface IEvent
    {
        
    }

    /// <summary>
    /// 事件对象接口
    /// </summary>
    public interface ISmartEvent
    {
        
    }

    /// <summary>
    /// 智能事件，可实现自动注销
    /// </summary>
    public class SmartEvent : ISmartEvent
    {
        Action onEvent;

        public void Register(Action callback)
        {
            onEvent += callback;
        }


        /// <summary>
        /// 注册，并在指定的gameObject销毁或者disable时自动注销
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="gameObject"></param>
        public void SmartRegister(Action callback, GameObject gameObject,
            UnRegisterType unRegisterType = UnRegisterType.Destroy)
        {
            onEvent += callback;
            var autoUnRegister = gameObject.GetComponent<AutoUnRegister>();
            if (!autoUnRegister) autoUnRegister = gameObject.AddComponent<AutoUnRegister>();
            switch (unRegisterType)
            {
                case UnRegisterType.Destroy:
                    autoUnRegister.RegisterOnDestroy(() => { onEvent -= callback; });
                    break;
                case UnRegisterType.Disable:
                    autoUnRegister.RegisterOnDisable(() => { onEvent -= callback; });
                    break;
            }
        }

        public void UnRegister(Action callback)
        {
            onEvent -= callback;
        }

        public void Trigger()
        {
            onEvent?.Invoke();
        }
    }

    /// <summary>
    /// 自动注销时机
    /// </summary>
    public enum UnRegisterType
    {
        Destroy,
        Disable
    }

    /// <summary>
    /// 带参数的智能事件
    /// </summary>
    /// <typeparam name="T">参数类型</typeparam>
    public class SmartEvent<T> : ISmartEvent
    {
        Action<T> onEvent;

        /// <summary>
        /// 普通注册
        /// </summary>
        /// <param name="callback"></param>
        public void Register(Action<T> callback)
        {
            onEvent += callback;
        }


        /// <summary>
        /// 注册，并在指定的gameObject销毁或者disable时自动注销
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="gameObject"></param>
        public void SmartRegister(Action<T> callback, GameObject gameObject,
            UnRegisterType unRegisterType = UnRegisterType.Destroy)
        {
            onEvent += callback;
            var autoUnRegister = gameObject.GetComponent<AutoUnRegister>();
            if (!autoUnRegister) autoUnRegister = gameObject.AddComponent<AutoUnRegister>();
            switch (unRegisterType)
            {
                case UnRegisterType.Destroy:
                    autoUnRegister.RegisterOnDestroy(() => { onEvent -= callback; });
                    break;
                case UnRegisterType.Disable:
                    autoUnRegister.RegisterOnDisable(() => { onEvent -= callback; });
                    break;
            }
        }

        public void UnRegister(Action<T> callback)
        {
            onEvent -= callback;
        }

        public void UnRegisterAll()
        {
            onEvent = null;
        }

        public void Trigger(T e)
        {
            onEvent?.Invoke(e);
        }
    }

    /// <summary>
    /// 自动注销功能类
    /// </summary>
    public class AutoUnRegister : MonoBehaviour
    {
        Action destroyEvent;
        Action disableEvent;

        public void RegisterOnDestroy(Action callback)
        {
            destroyEvent += callback;
        }

        public void RegisterOnDisable(Action callback)
        {
            disableEvent += callback;
        }

        void OnDestroy()
        {
            destroyEvent?.Invoke();
            destroyEvent = null;
            disableEvent = null;
        }

        void OnDisable()
        {
            disableEvent?.Invoke();
        }
    }

    /// <summary>
    /// 带事件的属性，属性改变时触发
    /// </summary>
    /// <typeparam name="T">参数</typeparam>
    public class EventProperty<T> where T : struct
    {
        T mValue;
        SmartEvent<T> smartEvent = new SmartEvent<T>();

        public EventProperty(T mValue)
        {
            this.mValue = mValue;
            smartEvent.Trigger(mValue);
        }

        public EventProperty()
        {
        }

        public T Value
        {
            get { return mValue; }
            set
            {
                if (mValue.Equals(value)) return;
                mValue = value;
                smartEvent.Trigger(value);
            }
        }

        public void Register(Action<T> callback)
        {
            smartEvent.Register(callback);
        }

        public void SmartRegister(Action<T> callback, GameObject gameObject,
            UnRegisterType unRegisterType = UnRegisterType.Destroy)
        {
            smartEvent.SmartRegister(callback, gameObject, unRegisterType);
        }

        public void UnRegister(Action<T> callback)
        {
            smartEvent.UnRegister(callback);
        }

        public void UnRegisterAll()
        {
            smartEvent.UnRegisterAll();
        }
    }
}