using TMPro;
using UnityEngine;


namespace ZYYGameKit.Localization
{
    public class LocalString : MonoBehaviour,IController
    {
        LocalizationModel localizationModel;
        public TMP_Text ShowText;
        string key;
        string curLanguage;
        bool isSetKey;
        
        public string Key
        {
            get
            {
                return key;
            }
            set
            {
                key = value;
                isSetKey = true;
                GetBind();
                ShowText.text = localizationModel.GetCurLanguageValue(key, curLanguage);
            }
        }
        
         void Start()
        {
            GetBind();
            if(!isSetKey) key = ShowText.text;
            this.SmartRegisterEvent<LanguageChangeEvent>(RefreshShowTest,gameObject);
        }
        
         
        

        private void RefreshShowTest(LanguageChangeEvent obj)
        {
            curLanguage = obj.ChangeLanguage;
            ShowText.text = localizationModel.GetCurLanguageValue(key, curLanguage);
        }

        public IModuleManager GetModuleManager()
        {
            return ModuleManager.Instance;
        }

        public void GetBind()
        {
            localizationModel ??= this.GetModel<LocalizationModel>();
        }
    }

}