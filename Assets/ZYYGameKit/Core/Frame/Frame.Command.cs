namespace ZYYGameKit
{
    public interface ICommandAuthority
    {
        public T GetSystem<T>() where T : class, ISystem;
        public T GetModel<T>() where T : class, IModel;
        public T GetUtility<T>() where T : class, IUtility; 
        public void SendCommand<T>(T command) where T : ICommand;
        public T SendCommand<T>(ICommand<T> command);
        public T SendSearch<T>(ISearch<T> search);
        public void SendEvent<T>(T e) where T : IEvent;
        public void SendEvent<T>() where T : IEvent, new();
    }

    public interface ICommand
    {
        public void Execute(ICommandAuthority authority);
    }

    public interface ICommand<T>
    {
        public T Execute(ICommandAuthority authority);
    }
}