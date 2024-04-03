namespace ZYYGameKit
{
    public class ModuleManager : AbstractModuleManager
    {
        static ModuleManager instance;
        ModuleManager()
        {
        
        }
    
        public static ModuleManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ModuleManager();
                    instance.Init();
                }
                return instance;
            }
        }

        public override void OnInit()
        {
            
        }
    }
}