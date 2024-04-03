using System.IO;
using Luban;
using SimpleJSON;
using UnityEngine;

namespace ZYYGameKit.DataConfig
{
    public interface IGetConfig : IUtility
    {
        public Tables GetTables();
    }

    public class LuBanConfig : IGetConfig
    {

        public Tables GetTables()
        {
            var tablesCtor = typeof(Tables).GetConstructors()[0];
            var loaderReturnType = tablesCtor.GetParameters()[0].ParameterType.GetGenericArguments()[1];
            // 根据cfg.Tables的构造函数的Loader的返回值类型决定使用json还是ByteBuf Loader
            System.Delegate loader = loaderReturnType == typeof(ByteBuf) ?
                new System.Func<string, ByteBuf>(LoadByteBuf)
                : (System.Delegate)new System.Func<string, JSONNode>(LoadJson);
            return (Tables)tablesCtor.Invoke(new object[]
            {
                loader
            });
        }

        static JSONNode LoadJson(string file)
        {

            return JSON.Parse(File.ReadAllText($"{Application.dataPath}/ZYFramework/LuBan/GenData/{file}.json", System.Text.Encoding.UTF8));
        }

        static ByteBuf LoadByteBuf(string file)
        {
            return new ByteBuf(File.ReadAllBytes($"{Application.dataPath}/ZYFramework/LuBan/GenData/{file}.bytes"));
        }
    }
}