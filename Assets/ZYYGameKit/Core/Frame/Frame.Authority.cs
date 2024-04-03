using System;
using UnityEngine;

namespace ZYYGameKit
{
    /// <summary>
    /// 初始化权限
    /// </summary>
    public interface ICanInit
    {
        bool Initialized { get; set; }
        void Init();
        void Deinit();
    }

    public interface IGetSetModuleManager
    {
        IModuleManager GetModuleManager();

        void SetModuleManager(IModuleManager moduleManager)
        {
            
        }
    }

    /// <summary>
    /// 获取数据层实例权限
    /// </summary>
    public interface IGetModel: IGetSetModuleManager
    {
        
    }
    
    public static class ModelExtensions
    {
        public static T GetModel<T>(this IGetModel slef) where T : class, IModel
        {
            return slef.GetModuleManager().GetModel<T>();
        }
    }

    /// <summary>
    /// 获取服务层实例权限
    /// </summary>
    public interface IGetSystem : IGetSetModuleManager
    {
       
    }
    
    public static class ServerExtensions
    {
        public static T GetServer<T>(this IGetSystem slef) where T : class, ISystem
        {
            return slef.GetModuleManager().GetSystem<T>();
        }
    }


    /// <summary>
    /// 获取工具层实例权限
    /// </summary>
    public interface IGetUtility : IGetSetModuleManager
    {
       
    }
    
    public static class UtilityExtensions
    {
        public static T GetUtility<T>(this IGetUtility slef) where T : class, IUtility
        {
            return slef.GetModuleManager().GetUtility<T>();
        }
    }

    /// <summary>
    /// 发送命令权限
    /// </summary>
    public interface ICanSendCommand : IGetSetModuleManager
    {
     
    }
    
    public static class CommandExtensions
    {
        public static void SendCommand<T>(this ICanSendCommand slef, T command) where T : ICommand
        {
            slef.GetModuleManager().SendCommand(command);
        }

        public static T SendCommand<T>(this ICanSendCommand slef, ICommand<T> command)
        {
            return slef.GetModuleManager().SendCommand(command);
        }
    }

    /// <summary>
    /// 发送查询权限
    /// </summary>
    public interface ICanSendSearch : IGetSetModuleManager
    {
       
    }
    
    public static class SearchExtensions
    {
        public static T SendSearch<T>(this ICanSendSearch slef, ISearch<T> search)
        {
            return slef.GetModuleManager().SendSearch(search);
        }
    }


    /// <summary>
    /// 发送事件权限
    /// </summary>
    public interface ICanSendEvent : IGetSetModuleManager
    {
      
    }
    
    public static class EventExtensions
    {
        public static void SendEvent<T>(this ICanSendEvent slef, T e) where T : IEvent
        {
            slef.GetModuleManager().SendEvent(e);
        }
        
        public static void SendEvent<T>(this ICanSendEvent slef) where T : IEvent, new()
        {
            slef.GetModuleManager().SendEvent<T>();
        }
    }

    /// <summary>
    /// 注册事件权限
    /// </summary>
    public interface ICanRegisterEvent : IGetSetModuleManager
    {
       
    }
    
    public static class EventRegisterExtensions
    {
        public static void RegisterEvent<T>(this ICanRegisterEvent slef, Action<T> onEvent) where T : IEvent
        {
            slef.GetModuleManager().RegisterEvent(onEvent);
        }
        
        public static void SmartRegisterEvent<T>(this ICanRegisterEvent slef, Action<T> onEvent, GameObject gameObject,
            UnRegisterType unRegisterType = UnRegisterType.Destroy) where T : IEvent
        {
            slef.GetModuleManager().SmartRegisterEvent(onEvent, gameObject, unRegisterType);
        }
        
        public static void RegisterEvent<T>(this ICanRegisterEvent slef, Action onEvent) where T : IEvent
        {
            slef.GetModuleManager().RegisterEvent<T>(onEvent);
        }
        
        public static void SmartRegisterEvent<T>(this ICanRegisterEvent slef, Action onEvent, GameObject gameObject,
            UnRegisterType unRegisterType = UnRegisterType.Destroy) where T : IEvent
        {
            slef.GetModuleManager().SmartRegisterEvent<T>(onEvent, gameObject, unRegisterType);
        }
        
        public static void UnRegisterEvent<T>(this ICanRegisterEvent slef, Action<T> onEvent) where T : IEvent
        {
            slef.GetModuleManager().UnRegisterEvent(onEvent);
        }
        
        public static void UnRegisterEvent<T>(this ICanRegisterEvent slef, Action onEvent) where T : IEvent
        {
            slef.GetModuleManager().UnRegisterEvent<T>(onEvent);
        }
    }
}