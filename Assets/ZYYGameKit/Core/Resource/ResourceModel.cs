using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace ZYYGameKit.Resource
{
    public interface IResourceModel : IModel
    {
        public T LoadAsset<T>(string key) where T : Object;
        
        public Task<T> LoadAssetAsync<T>(string key) where T : Object;

        public void Release(string key);
        
        
        public Task LoadSceneAsync(string key, LoadSceneMode mode = LoadSceneMode.Single, bool suspendLoad = false);

        public Task UnLoadSceneAsync(string key);
    }


    public struct LoadResourceCompleteEvent : IEvent
    {
        
    }
    
    public class DefaultResourceModel : AbstractModel, IResourceModel
    {

        Dictionary<string, Object> assetDict = new Dictionary<string, Object>();

        EventProperty<int> count = new EventProperty<int>();
        
        

        public T LoadAsset<T>(string key) where T : Object
        {
            if(assetDict.TryGetValue(key,out Object value))
            {
                return value as T;
            }
            return default;
        }
        
        public async Task<T> LoadAssetAsync<T>(string key) where T : Object
        {
            if (assetDict.TryGetValue(key,out Object value))
            {
                return value as T;
            }
            count.Value++;
            var resourceRequest = Resources.LoadAsync<T>(key);
            await resourceRequest.ToUniTask();
            assetDict.Add(key,resourceRequest.asset);
            count.Value--;
            return resourceRequest.asset as T;
        }
        
        public void Release(string key)
        {
            if(assetDict.TryGetValue(key,out Object value))
            {
                Resources.UnloadAsset(value);
                assetDict.Remove(key);
            }
        }

        public async Task LoadSceneAsync(string key, LoadSceneMode mode = LoadSceneMode.Single, bool suspendLoad = false)
        {
            var loadSceneAsync = SceneManager.LoadSceneAsync(key, mode);
            await loadSceneAsync.ToUniTask();
        }
        
        public async Task UnLoadSceneAsync(string key)
        {
            var unloadSceneAsync = SceneManager.UnloadSceneAsync(key);
            await unloadSceneAsync.ToUniTask();
        }

        public override void Init()
        {
            count.Register(e =>
            {
                if (e == 0)
                {
                    this.SendEvent<LoadResourceCompleteEvent>();
                }
            });
        }

        public override void Deinit()
        {
            count.UnRegisterAll();
        }
    }
}