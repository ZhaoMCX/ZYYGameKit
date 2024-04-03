// using System.Reflection;
// using UnityEditor;
// using UnityEngine;
//
//
// namespace ZYFramework.Core.Inspector
// {
//     
//     [CustomEditor(typeof(MonoBehaviour),true)]
//     [CanEditMultipleObjects]
//     public class InspectorButtonMonoEditor : Editor
//     {
//         public override void OnInspectorGUI()
//         {
//             base.OnInspectorGUI();
//
//             MonoBehaviour script = (MonoBehaviour)target;
//
//             var methods = script.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
//             foreach (var method in methods)
//             {
//                 var attributes = method.GetCustomAttributes(typeof(ButtonAttribute), true);
//                 if (attributes.Length > 0)
//                 {
//                     if (GUILayout.Button(method.Name))
//                     {
//                         method.Invoke(script, null);
//                     }
//                 }
//             }
//         }
//     }
//     
//     [CustomEditor(typeof(ScriptableObject),true)]
//     public class InspectorButtonSOEditor : Editor
//     {
//         public override void OnInspectorGUI()
//         {
//             base.OnInspectorGUI();
//
//             ScriptableObject script = (ScriptableObject)target;
//
//             var methods = script.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
//             foreach (var method in methods)
//             {
//                 var attributes = method.GetCustomAttributes(typeof(ButtonAttribute), true);
//                 if (attributes.Length > 0)
//                 {
//                     if (GUILayout.Button(method.Name))
//                     {
//                         method.Invoke(script, null);
//                     }
//                 }
//             }
//         }
//     }
//     
// }
