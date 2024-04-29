using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;


namespace ZZYGameKit.Core.Inspector
{
    
    [CustomEditor(typeof(MonoBehaviour),true)]
    [CanEditMultipleObjects]
    public class InspectorButtonMonoEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            // 获取当前所有选中对象的共有方法
            Dictionary<string, List<MonoBehaviour>> methodsToInvoke = new Dictionary<string, List<MonoBehaviour>>();

            foreach (Object obj in targets)
            {
                MonoBehaviour script = obj as MonoBehaviour;
                if (script)
                {
                    var methods = script.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    foreach (var method in methods)
                    {
                        var attributes = method.GetCustomAttributes(typeof(ButtonAttribute), true);
                        if (attributes.Length > 0)
                        {
                            if (!methodsToInvoke.ContainsKey(method.Name))
                            {
                                methodsToInvoke[method.Name] = new List<MonoBehaviour>();
                            }
                            methodsToInvoke[method.Name].Add(script);
                        }
                    }
                }
            }

            // 为每个方法渲染一个按钮
            foreach (var methodName in methodsToInvoke.Keys)
            {
                if (GUILayout.Button(methodName))
                {
                    foreach (MonoBehaviour script in methodsToInvoke[methodName])
                    {
                        // 获取具体的方法
                        MethodInfo method = script.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                        method?.Invoke(script, null);
                    }
                }
            }
        }
    }
    
    [CustomEditor(typeof(ScriptableObject),true)]
    public class InspectorButtonSOEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            // 获取当前所有选中对象的共有方法
            Dictionary<string, List<MonoBehaviour>> methodsToInvoke = new Dictionary<string, List<MonoBehaviour>>();

            foreach (Object obj in targets)
            {
                MonoBehaviour script = obj as MonoBehaviour;
                if (script)
                {
                    var methods = script.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    foreach (var method in methods)
                    {
                        var attributes = method.GetCustomAttributes(typeof(ButtonAttribute), true);
                        if (attributes.Length > 0)
                        {
                            if (!methodsToInvoke.ContainsKey(method.Name))
                            {
                                methodsToInvoke[method.Name] = new List<MonoBehaviour>();
                            }
                            methodsToInvoke[method.Name].Add(script);
                        }
                    }
                }
            }

            // 为每个方法渲染一个按钮
            foreach (var methodName in methodsToInvoke.Keys)
            {
                if (GUILayout.Button(methodName))
                {
                    foreach (MonoBehaviour script in methodsToInvoke[methodName])
                    {
                        // 获取具体的方法
                        MethodInfo method = script.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                        method?.Invoke(script, null);
                    }
                }
            }
        }
    }
    
}
