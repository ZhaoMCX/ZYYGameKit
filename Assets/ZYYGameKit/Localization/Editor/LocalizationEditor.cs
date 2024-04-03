
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace ZYYGameKit.Localization
{
    public class LocalizationEditor : Editor
    {
    
        [MenuItem("CONTEXT/TMP_Text/Localization")]
        static void AddNewComponentAndSetReference(MenuCommand command)
        {
            // 获取选中的TextMeshProUGUI组件
            TextMeshProUGUI textMeshProUGUI = (TextMeshProUGUI)command.context;

            if (textMeshProUGUI != null)
            {
                // 获取包含TextMeshProUGUI组件的游戏对象
                GameObject selectedObject = textMeshProUGUI.gameObject;

                // 添加新组件
                LocalString newComponent = selectedObject.AddComponent<LocalString>();

                // 获取或添加ExistingComponentScript组件
                TextMeshProUGUI existingComponent = selectedObject.GetComponent<TextMeshProUGUI>();

                // 设置引用
                newComponent.ShowText = existingComponent;

                // 刷新Inspector以显示更新后的引用
                EditorUtility.SetDirty(existingComponent);
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }
    }
}