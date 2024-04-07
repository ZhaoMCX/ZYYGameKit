using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
namespace ZYYGameKit.Log
{
    public class LogMonitor : MonoBehaviour
    {
        public GUIStyle logStyle;
        private string logText = "";
        private string[] logs;

        private void Start()
        {
            // 设置文本样式
            logStyle.fontSize = 14;
            logStyle.normal.textColor = Color.white;
        }

        private void OnEnable()
        {
            // 注册Unity的Application.logMessageReceived事件，以捕获日志消息
            Application.logMessageReceived += HandleLog;
        }

        private void OnDisable()
        {
            // 在脚本销毁时取消日志事件的注册
            Application.logMessageReceived -= HandleLog;
        }

        private void OnGUI()
        {
            // 在屏幕上显示按行排列的日志文本
            GUI.Label(new Rect(10, 10, Screen.width - 20, Screen.height - 20), logText, logStyle);
        }

        private void HandleLog(string logText, string stackTrace, LogType logType)
        {
            logStyle.normal.textColor = Color.white;
            // 将新的日志消息添加到日志文本中，每行之间用换行符分隔
            this.logText += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + logText + "\n";
            logs = this.logText.Split('\n');

            // 限制日志文本的行数，以防止它变得过长
            int maxLines = 15;
            if (logs.Length > maxLines)
            {
                // 移除旧的日志行，保持最新的maxLines行
                this.logText = string.Join("\n", logs, logs.Length - maxLines, maxLines);
            }

            StartColorFade();
        }

        private async void StartColorFade()
        {
            float elapsedTime = 0f;
            Color initialColor = Color.white;
            Color targetColor = new Color(1f, 1f, 1f, 0);
            while (elapsedTime < 10f)
            {
                if (elapsedTime <= 5f)
                {
                    // 确保最终颜色与目标颜色匹配
                    logStyle.normal.textColor = initialColor;
                }
                else
                {
                    // 计算插值颜色
                    float t = elapsedTime / 10f;
                    Color lerpedColor = Color.Lerp(initialColor, targetColor, t);
                    logStyle.normal.textColor = lerpedColor;
                }

                // 等待一帧
                await UniTask.Yield();

                // 增加已经过去的时间
                elapsedTime += Time.deltaTime;
            }

            // 确保最终颜色与目标颜色匹配
            logStyle.normal.textColor = targetColor;
        }

        
        public void Test()
        {
            ZyyLogger.Log(outText);
        }

        public string outText;
    }
}