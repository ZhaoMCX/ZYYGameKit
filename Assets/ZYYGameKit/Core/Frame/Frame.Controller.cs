

namespace ZYYGameKit
{

    /// <summary>
    /// 控制层，接收输入，显示数据变化，计算逻辑
    /// 可获取数据
    /// 可获取服务
    /// 可发送命令
    /// 可发送事件
    /// </summary>
    public interface IController : IGetModel, IGetSystem, ICanSendCommand, ICanRegisterEvent, IGetUtility,ICanSendSearch
    {
        public void GetBind(){}
    }
}