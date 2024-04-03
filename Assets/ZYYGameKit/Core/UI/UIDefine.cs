using System.Collections.Generic;
using UnityEngine;
using ZYYGameKit.Resource;

namespace ZYYGameKit.UI
{

    public enum Dir
    {
        Up,
        Down,
        Left,
        Right
    }
    
    public class UISystem : AbstractSystem
    {
        Dictionary<string,BasePanel> panelDict = new Dictionary<string, BasePanel>();
        GameObject uiRoot;
        IResourceModel resourceModel;
        Stack<BasePanel> openPanelStack = new Stack<BasePanel>();

        public void RegisterPanel<T>(BasePanel panel)
        {
            panelDict.Add(typeof(T).Name,panel);
        }
        
        public void BackPanel()
        {
            if (openPanelStack.Count > 0)
            {
                openPanelStack.Pop().Close();
            }
        }

        public void OpenPanel<T>(string assetKey, IUIData uiData, bool isPushStack = false)
        {
            var name = typeof(T).Name;
            if (panelDict.TryGetValue(name, out var value))
            {
                value.Open(uiData);
            }
            else
            {
                if (uiRoot != null)
                {
                    GameObject panelPrefab = resourceModel.LoadAsset<GameObject>(assetKey);
                    var panel = GameObject.Instantiate(panelPrefab, uiRoot.transform);
                    panel.name = name;
                    panelDict.Add(name, panel.GetComponent<BasePanel>());
                    panelDict[name].Open(uiData);
                }
            }
            if (isPushStack&&!openPanelStack.Contains(value))
            {
                openPanelStack.Push(value);
            }
        }

        public void OpenPanel<T>(IUIData uiData, bool isPushStack = false)
        {
            var name = typeof(T).Name;
            if (panelDict.TryGetValue(name, out var value))
            {
                value.Open(uiData);
            }
            if (isPushStack&&!openPanelStack.Contains(value))
            {
                openPanelStack.Push(value);
            }
        }
        
        
        public void HidePanel<T>()
        {
            var name = typeof(T).Name;
            if (panelDict.TryGetValue(name, out var value))
            {
                value.Hide();
            }
        }
        
        public void ClosePanel<T>()
        {
            var name = typeof(T).Name;
            if (panelDict.TryGetValue(name, out var value))
            {
                value.Close();
                panelDict.Remove(name);
            }
        }

        public override void Init()
        {
            uiRoot = GameObject.FindGameObjectWithTag("UIRoot");
            resourceModel = this.GetModel<IResourceModel>();
        }

        public override void Deinit()
        {
            uiRoot = null;
            resourceModel = null;
        }
    }
}