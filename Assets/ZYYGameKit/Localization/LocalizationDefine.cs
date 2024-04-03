using System.Collections.Generic;
using ZYYGameKit.DataConfig;

namespace ZYYGameKit.Localization
{
    public readonly struct ChangeLanguageCommand : ICommand
    {
        readonly string changeLanguage;
    
        public ChangeLanguageCommand(string changeLanguage)
        {
            this.changeLanguage = changeLanguage;
        }

        public void Execute(ICommandAuthority authority)
        {
            authority.SendEvent(new LanguageChangeEvent(changeLanguage));
        }
    }
    
    public struct LanguageChangeEvent : IEvent
    {
        public string ChangeLanguage;

        public LanguageChangeEvent(string changeLanguage)
        {
            ChangeLanguage = changeLanguage;
        }
    }

    public class Language
    {
        public readonly string Key;
        public readonly string ChineseSimplified;
        public readonly string English;

        public Language(string key, string chineseSimplified, string english)
        {
            Key = key;
            ChineseSimplified = chineseSimplified;
            English = english;
        }
    }
    
    
    public class LocalizationModel : AbstractModel
    {
        Dictionary<string,Language> languageDict;

        public string GetCurLanguageValue(string key,string language = null)
        {
            var showName = languageDict["showName"];
            if (showName.ChineseSimplified.Equals(language))
            {
                return languageDict[key].ChineseSimplified;
            }
            if (showName.English.Equals(language))
            {
                return languageDict[key].English;
            }
            return key;
        }
        
        public override void Init()
        {
            languageDict = new Dictionary<string, Language>();
            var genTbLanguage = this.GetUtility<IGetConfig>().GetTables().TbLanguage;
            foreach (var language in genTbLanguage.DataList)
            {
                languageDict.Add(language.Key,new Language(language.Key,language.ChineseSimplified,language.English));
            }
        }
        
        
        public override void Deinit()
        {
            languageDict = null;
        }
    }
}