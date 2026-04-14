using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
/// <summary>
/// 提供一个可全局调用的静态类
/// </summary>
public static class JsonProcess
{
    //设定默认的存档位置
    public const string DefaultSavePath = "save/gamedata.json";
    public const string DefaultSlotDirectory = "save/slots";
    public const string DefaultSlotFilePrefix = "slot_";
    public const string SlotFileExtension = ".json";

    [Serializable]
    public struct SaveSlotMeta
    {
        public int slotIndex;
        public string fullPath;
        public long lastWriteUtcTicks;
    }
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
    /// 动态文件路径生成，只需要获取当前存档下标
    /// </summary>
    public static string BuildSlotRelativePath(int slotIndex, string slotDirectory = DefaultSlotDirectory, string slotPrefix = DefaultSlotFilePrefix)
    {
        if (slotIndex <= 0) throw new ArgumentOutOfRangeException(nameof(slotIndex), "slotIndex 必须大于 0。");
        string fileName = slotPrefix + slotIndex + SlotFileExtension;
        return Path.Combine(slotDirectory, fileName);
    }
    /// <summary>
    /// 提供排序后的路径
    /// </summary>
    public static string ResolveSlotPath(int slotIndex, string slotDirectory = DefaultSlotDirectory, string slotPrefix = DefaultSlotFilePrefix)
    {
        return ResolvePath(BuildSlotRelativePath(slotIndex, slotDirectory, slotPrefix));
    }
    /// <summary>
    /// 转下标存在就删除
    /// </summary>
    /// <returns>返回值判断是否成功删除</returns>
    public static bool DeleteSlotFile(int slotIndex, string slotDirectory = DefaultSlotDirectory, string slotPrefix = DefaultSlotFilePrefix)
    {
        string fullPath = ResolveSlotPath(slotIndex, slotDirectory, slotPrefix);
        if (!File.Exists(fullPath))
            return false;

        File.Delete(fullPath);
        return true;
    }
    /// <summary>
    /// 依旧是传入存档下标查找该位置是否存在存档
    /// </summary>
    public static bool SlotExists(int slotIndex, string slotDirectory = DefaultSlotDirectory, string slotPrefix = DefaultSlotFilePrefix)
    {
        string fullPath = ResolveSlotPath(slotIndex, slotDirectory, slotPrefix);
        return File.Exists(fullPath);
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

    /// <summary>
    /// 该方法供ui调用，主要是提供操作时间和返回存档名
    /// </summary>
    public static List<SaveSlotMeta> ListSlots(string slotDirectory = DefaultSlotDirectory, string slotPrefix = DefaultSlotFilePrefix)
    {
        var result = new List<SaveSlotMeta>();
        string fullDir = ResolvePath(slotDirectory);
        if (!Directory.Exists(fullDir))
            return result;

        string pattern = slotPrefix + "*" + SlotFileExtension;
        //获取该文件下的所有文件，pattern是过滤文件，前缀和后缀格式slot_*.json，最后一个是搜索模式只在当前文件夹
        string[] files = Directory.GetFiles(fullDir, pattern, SearchOption.TopDirectoryOnly);
        foreach (string file in files)
        {
            //该操作是去掉后缀
            string fileName = Path.GetFileNameWithoutExtension(file);
            //再次对比其后缀并按字母二进制编码比对
            if (!fileName.StartsWith(slotPrefix, StringComparison.Ordinal))
                continue;
            //这里又是截取字符串的操作看砍掉了前缀
            string indexPart = fileName.Substring(slotPrefix.Length);
            //尝试获取int类型的数字
            if (!int.TryParse(indexPart, out int slotIndex) || slotIndex <= 0)
                continue;
            //获取游戏时间
            DateTime utc = File.GetLastWriteTimeUtc(file);
            result.Add(new SaveSlotMeta
            {
                slotIndex = slotIndex,
                fullPath = file,
                lastWriteUtcTicks = utc.Ticks
            });
        }
        //lambda表达式做排序操作最后返回一个list给UI
        result.Sort((a, b) => a.slotIndex.CompareTo(b.slotIndex));
        return result;
    }
}