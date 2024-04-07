using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ZYYGameKit.Log;

namespace ZYYGameKit
{
    public class IOCContanier
    {
        //存储所有实例，每种类型一个实例
        Dictionary<Type, object> instanceDict = new Dictionary<Type, object>();

        public void Register<T>(T instance)
        {
            var key = typeof(T);

            if (instanceDict.ContainsKey(key))
            {
                ZyyLogger.Log($"注册实例{key}失败,已存在");
            }
            else
            {
                instanceDict.Add(key, instance);
            }
        }

        public T Get<T>() where T : class
        {
            var key = typeof(T);

            if (instanceDict.TryGetValue(key, out var retInstance))
            {
                return retInstance as T;
            }

            return default;
        }

        public IEnumerable<T> GetInstancesByType<T>()
        {
            var type = typeof(T);
            return instanceDict.Values.Where(instance => type.IsInstanceOfType(instance)).Cast<T>();
        }

        public void Clear() => instanceDict.Clear();
    }


    public interface IModuleManager
    {
        void RegisterSystem<T>(T system) where T : ISystem;

        void RegisterModel<T>(T model) where T : IModel;

        void RegisterUtility<T>(T utility) where T : IUtility;

        void OnInit();

        public T GetSystem<T>() where T : class, ISystem;
        public T GetModel<T>() where T : class, IModel;
        public T GetUtility<T>() where T : class, IUtility; 
        public void SendCommand<T>(T command) where T : ICommand;
        public T SendCommand<T>(ICommand<T> command);
        public T SendSearch<T>(ISearch<T> search);
        public void SendEvent<T>(T e) where T : IEvent;
        public void SendEvent<T>() where T : IEvent, new();
        public void RegisterEvent<T>(Action<T> onEvent) where T : IEvent;

        public void SmartRegisterEvent<T>(Action<T> onEvent, GameObject gameObject,
            UnRegisterType unRegisterType = UnRegisterType.Destroy) where T : IEvent;

        public void SmartRegisterEvent<T>(Action onEvent, GameObject gameObject,
            UnRegisterType unRegisterType = UnRegisterType.Destroy) where T : IEvent;

        public void UnRegisterEvent<T>(Action<T> onEvent) where T : IEvent;

        public void RegisterEvent<T>(Action onEvent) where T : IEvent;
        public void UnRegisterEvent<T>(Action onEvent) where T : IEvent;
    }


    /// <summary>
    /// 提供一种模块管理器的实现
    /// </summary>
    public abstract class AbstractModuleManager : ICanInit, IModuleManager, ICommandAuthority,ISearchAuthority
    {
        //模块容器
        IOCContanier iocContanier = new IOCContanier();

        Dictionary<Type, ISmartEvent> eventDict = new Dictionary<Type, ISmartEvent>();

        public bool Initialized { get; set; }

        public void Init()
        {
            OnInit();
            if (Initialized)
            {
                return;
            }

            foreach (var model in iocContanier.GetInstancesByType<IModel>().Where(m => !m.Initialized))
            {
                model.Init();
                model.Initialized = true;
            }

            foreach (var system in iocContanier.GetInstancesByType<ISystem>().Where(m => !m.Initialized))
            {
                system.Init();
                system.Initialized = true;
            }

            Initialized = true;
        }

        public void Deinit()
        {
            foreach (var system in iocContanier.GetInstancesByType<ISystem>().Where(s => s.Initialized))
                system.Deinit();
            foreach (var model in iocContanier.GetInstancesByType<IModel>().Where(m => m.Initialized)) model.Deinit();
            iocContanier.Clear();
            iocContanier = null;
            Initialized = false;
        }

        public void RegisterSystem<T>(T system) where T : ISystem
        {
            iocContanier.Register(system);
            system.SetModuleManager(this);
        }
        
        public void RegisterModel<T>(T model) where T : IModel
        {
            iocContanier.Register(model);
            model.SetModuleManager(this);
        }

        public void RegisterUtility<T>(T utility) where T : IUtility
        {
            iocContanier.Register(utility);
        }

        public abstract void OnInit();


        public T GetSystem<T>() where T : class, ISystem
        {
            return iocContanier.Get<T>();
        }

        public T GetModel<T>() where T : class, IModel
        {
            return iocContanier.Get<T>();
        }

        public T GetUtility<T>() where T : class, IUtility
        {
            return iocContanier.Get<T>();
        }

        public void SendCommand<T>(T command) where T : ICommand
        {
            command.Execute(this);
        }

        public T SendCommand<T>(ICommand<T> command)
        {
            return command.Execute(this);
        }

        public T SendSearch<T>(ISearch<T> search)
        {
            return search.Execute(this);
        }

        public void SendEvent<T>(T e) where T : IEvent
        {
            if (eventDict.TryGetValue(typeof(SmartEvent<T>), out var smartEvent))
            {
                if (smartEvent is SmartEvent<T> value) value.Trigger(e);
            }
        }

        public void SendEvent<T>() where T : IEvent, new()
        {
            if (eventDict.TryGetValue(typeof(SmartEvent), out var smartEvent))
            {
                if (smartEvent is SmartEvent value) value.Trigger();
            }
        }

        public void RegisterEvent<T>(Action<T> onEvent) where T : IEvent
        {
            if (!eventDict.TryGetValue(typeof(SmartEvent<T>), out var smartEvent))
            {
                smartEvent = new SmartEvent<T>();
                eventDict.Add(typeof(SmartEvent<T>), smartEvent);
            }

            if (smartEvent is SmartEvent<T> value) value.Register(onEvent);
        }

        public void SmartRegisterEvent<T>(Action<T> onEvent, GameObject gameObject,
            UnRegisterType unRegisterType = UnRegisterType.Destroy) where T : IEvent
        {
            if (!eventDict.TryGetValue(typeof(SmartEvent<T>), out var smartEvent))
            {
                smartEvent = new SmartEvent<T>();
                eventDict.Add(typeof(SmartEvent<T>), smartEvent);
            }

            if (smartEvent is SmartEvent<T> value) value.SmartRegister(onEvent, gameObject, unRegisterType);
        }

        public void RegisterEvent<T>(Action onEvent) where T : IEvent
        {
            if (!eventDict.TryGetValue(typeof(SmartEvent), out var smartEvent))
            {
                smartEvent = new SmartEvent();
                eventDict.Add(typeof(SmartEvent), smartEvent);
            }

            if (smartEvent is SmartEvent value) value.Register(onEvent);
        }

        public void SmartRegisterEvent<T>(Action onEvent, GameObject gameObject,
            UnRegisterType unRegisterType = UnRegisterType.Destroy) where T : IEvent
        {
            if (!eventDict.TryGetValue(typeof(SmartEvent), out var smartEvent))
            {
                smartEvent = new SmartEvent();
                eventDict.Add(typeof(SmartEvent), smartEvent);
            }

            if (smartEvent is SmartEvent value) value.SmartRegister(onEvent, gameObject, unRegisterType);
        }

        public void UnRegisterEvent<T>(Action<T> onEvent) where T : IEvent
        {
            if (eventDict.TryGetValue(typeof(SmartEvent<T>), out var smartEvent))
            {
                if (smartEvent is SmartEvent<T> value) value.UnRegister(onEvent);
            }
        }

        public void UnRegisterEvent<T>(Action onEvent) where T : IEvent
        {
            if (eventDict.TryGetValue(typeof(SmartEvent), out var smartEvent))
            {
                if (smartEvent is SmartEvent value) value.UnRegister(onEvent);
            }
        }
    }
}