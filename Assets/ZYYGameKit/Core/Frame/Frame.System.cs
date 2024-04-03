

namespace ZYYGameKit
{


    /// <summary>
    /// 服务层，控制层提供控制层共享的逻辑和共享的变量
    /// 可获取数据
    /// 可获取服务
    /// 可注册事件
    /// 可发送事件
    /// </summary>
    public interface ISystem : ICanInit, IGetModel, IGetSystem, ICanRegisterEvent, ICanSendEvent, IGetUtility, ICanSendSearch
    {

    }


    public abstract class AbstractSystem : ISystem
    {
        private IModuleManager moduleManager;

        public bool Initialized { get; set; }
        public abstract void Init();

        public abstract void Deinit();

        IModuleManager IGetSetModuleManager.GetModuleManager()
        {
            return moduleManager;
        }

        void IGetSetModuleManager.SetModuleManager(IModuleManager moduleManager)
        {
            this.moduleManager = moduleManager;
        }
    }
}