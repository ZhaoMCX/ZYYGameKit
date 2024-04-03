// using System.Collections.Generic;
// using System.Threading.Tasks;
// using Cysharp.Threading.Tasks;
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using YooAsset;
// using ZYYGameKit;
// using ZYYGameKit.Resource;
//
//
// public class YooAssetResourceModel : AbstractModel, IResourceModel
// {
//     readonly Dictionary<string, AssetHandle> assetHandleDict = new Dictionary<string, AssetHandle>();
//     readonly Dictionary<string, SceneHandle> sceneHandleDict = new Dictionary<string, SceneHandle>();
//     
//
//     public T LoadAsset<T>(string key) where T : Object
//     {
//         if (assetHandleDict.TryGetValue(key, out AssetHandle assetHandle))
//         {
//             if (assetHandle.IsDone)
//             {
//                 return assetHandle.AssetObject as T;
//             }
//         }
//         return default;
//     }
//
//     public async Task<T> LoadAssetAsync<T>(string key) where T : Object
//     {
//         if (assetHandleDict.TryGetValue(key, out AssetHandle assetHandle))
//         {
//             if (assetHandle.IsDone)
//             {
//                 return assetHandle.AssetObject as T;
//             }
//             await assetHandle;
//             return assetHandle.AssetObject as T;
//         }
//         var loadAssetAsync = YooAssets.LoadAssetAsync<T>(key);
//         assetHandleDict.Add(key, loadAssetAsync);
//         await loadAssetAsync.Task;
//         return loadAssetAsync.AssetObject as T;
//
//     }
//
//     public void Release(string key)
//     {
//         if (assetHandleDict.TryGetValue(key, out AssetHandle assetHandle))
//         {
//             assetHandle.Release();
//             assetHandleDict.Remove(key);
//         }
//     }
//     public async Task LoadSceneAsync(string key, LoadSceneMode mode = LoadSceneMode.Single, bool suspendLoad = false)
//     {
//         if (sceneHandleDict.ContainsKey(key)) return;
//         var loadSceneAsync = YooAssets.LoadSceneAsync(key, mode, suspendLoad);
//         sceneHandleDict.Add(key, loadSceneAsync);
//         await loadSceneAsync.Task;
//     }
//
//     public async Task UnLoadSceneAsync(string key)
//     {
//         if (sceneHandleDict.TryGetValue(key, out SceneHandle sceneHandle))
//         {
//             await sceneHandle.UnloadAsync();
//             sceneHandleDict.Remove(key);
//         }
//     }
//     public override void Init()
//     {
//         YooAssets.Initialize();
//     }
//     public override void Deinit()
//     {
//         YooAssets.Destroy();
//     }
// }