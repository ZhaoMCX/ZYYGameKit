namespace ZYYGameKit
{

    public interface ISearchAuthority
    {
        public T GetModel<T>() where T : class, IModel;
        public T GetSystem<T>() where T : class, ISystem;
        public T GetUtility<T>() where T : class, IUtility; 
    }

    public interface ISearch<T>
    {
        public T Execute(ISearchAuthority authority);
    }
}