using System.IO;
using UnityEngine;
using System;
using System.Text;
/// <summary>
/// 提供一个可全局调用的静态类
/// </summary>
public static class JsonProcess
{
    //设定默认的存档位置
    public const string DefaultSavePath = "save/gamedata.json";
    /// <summary>
    /// 提供一个路径处理的方法，依据不同的情况返回不同的路径,目前作为辅助方法
    /// </summary>
    public static string ResolvePath(string path)
    {
        //如果是空就用默认路径
        if (string.IsNullOrEmpty(path)) return Path.Combine(Application.persistentDataPath, DefaultSavePath);
        //如果是从根目录开始的绝对地址返回该地址
        if (Path.IsPathRooted(path)) return path;
        //如果都不是就返回系统自动寻找＋你提供的地址
        return Path.Combine(Application.persistentDataPath, path);
    }
    /// <summary>
    /// 提供一个将数据转换为JSON文件的通用泛形转换工具
    /// </summary>
    public static string ToJson<T>(T data) where T : class
    {
        //如果data空，直接抛出异常
        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }
        //没有问题转换为json文件并格式化
        return JsonUtility.ToJson(data, true);
    }
    /// <summary>
    /// 将Json文件转为对象的过程，使用泛型承接
    /// </summary>
    public static T FromJson<T>(string json) where T : class
    {
        //如果内容为空结束
        if (string.IsNullOrEmpty(json)) return null;
        //正常就正常转化
        return JsonUtility.FromJson<T>(json);
    }
    /// <summary>
    /// 提供一个文件写入的方法
    /// </summary>
    public static void SaveToFile<T>(T data, string Inpath) where T : class
    {
        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }
        string path = ResolvePath(Inpath);
        string directory = Path.GetDirectoryName(path);
        //确保目录被删除后还能正常生成文件，防呆处理
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        //存储转换后的文件内容
        string json = ToJson(data);
        //一个临时文件没有切换路径，只是在原文件路径后加了一个.tmp
        string tempPath = path + ".tmp";
        //写入的时候优先写入临时文件,用UTF8编码且不带BOM，防止出现乱码问题
        File.WriteAllText(tempPath, json, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        //等待写入成功后才去变成正式的Json文件，防止写入过程中出现问题导致原文件损坏
        File.Move(tempPath, path);
    }

    /// <summary>
    /// 反序列化回游戏对象
    /// </summary>
    public static bool TryLoadFromFile<T>(string Inpath, out T data) where T : class
    {
        data = null;
        string path = ResolvePath(Inpath);
        if (!File.Exists(path))
        {
            return false;
        }
        string json = File.ReadAllText(path, Encoding.UTF8);
        if (string.IsNullOrEmpty(json))
        {
            return false;
        }
        data = FromJson<T>(json);
        return data != null;
    }
}