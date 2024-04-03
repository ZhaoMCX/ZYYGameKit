namespace ZYYGameKit
{
    /// <summary>
    /// 数据层，数据层，提供共享的配置数据，静态数据
    /// 可获取工具
    /// 可发送事件
    /// </summary>
    public interface IModel : ICanInit ,IGetUtility, ICanSendEvent
    {
        
    }

    public abstract class AbstractModel : IModel
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