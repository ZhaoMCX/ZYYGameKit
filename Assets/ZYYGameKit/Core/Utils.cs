using System;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using ZYYGameKit.Log;


namespace ZYYGameKit.Core.Utils
{
   public static class FileUtils
{
    // 保存 JSON 数据到文件
    public static bool SaveToJson<T>(T data, string fileName)
    {
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        return Save(json, fileName);
    }

    public static bool Save(string data, string fileName,string filePath = null)
    {
        try
        {
            filePath = FilePathCheck(filePath);
            string filePathName = Path.Combine(filePath, fileName);
            File.WriteAllText(filePathName, data);
            return true;
        }
        catch (Exception e)
        {
            ZyyLogger.Log(e.Message);
            return false;
        }
    }

    private static string FilePathCheck(string filePath)
    {
        if (filePath == null)
        {
            filePath = Application.streamingAssetsPath;
        }
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }

        return filePath;
    }

    public static string Load(string fileName, string filePath = null)
    {
        try
        {
            filePath = FilePathCheck(filePath);
            string filePathName = Path.Combine(filePath, fileName);
            if (File.Exists(filePathName))
            {
                string data = File.ReadAllText(filePathName);
                return data;
            }

            ZyyLogger.Log($"File not found:{filePathName}");
            return default;
        }
        catch (Exception e)
        {
            ZyyLogger.Log(e.Message);
            return default;
        }
    }

    // 从文件加载 JSON 数据
    public static T LoadFromJson<T>(string fileName)
    {
        string data = Load(fileName);
        if (data != default)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }
        return default;
    }
}


public static class AnimationUtils
{
    // 根据动画片段名称获取其长度
    public static float GetAnimationClipLength(string clipName,Animator animator)
    {
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
        foreach (AnimationClip clip in ac.animationClips)
        {
            if (clip.name == clipName)
            {
                return clip.length;
            }
        }
        return 0;
    }
} 
}
