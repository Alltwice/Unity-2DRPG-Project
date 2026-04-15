using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SavePanelUI : BasePanel
{
    private const string DefaultLoadSceneName = "MainScene";

    [Header("需求组件")]
    [SerializeField] private GameDataSaveLoad saveLoad;

    [Header("UI")]
    [Tooltip("索引 0 对应 1 号槽，索引 1 对应 2 号槽，以此类推。")]
    //槽位序号和时间戳
    [SerializeField] private TextMeshProUGUI[] slotLabels;
    //提供槽位状态描述
    [SerializeField] private TextMeshProUGUI statusText;
    //槽位信息包体
    private readonly Dictionary<int, JsonProcess.SaveSlotMeta> slotMetaMap = new Dictionary<int, JsonProcess.SaveSlotMeta>();

    /// <summary>
    /// 开始时注册并刷新UI
    /// </summary>
    private void Start()
    {
        UIManager.Instance.RegisterPanel(PanelType.savePanel, this);
        RefreshSlotsUI();
    }
    /// <summary>
    /// 解绑
    /// </summary>
    private void OnDisable()
    {
        if (UIManager.Instance != null)
            UIManager.Instance.UnregisterPanel(PanelType.savePanel);
    }
    /// <summary>
    /// 打开就要刷新UI界面
    /// </summary>
    public override void Open()
    {
        base.Open();
        RefreshSlotsUI();
    }
    /// <summary>
    /// 点击存档按钮方法改变槽位状态显示，同时提供一个保护防止越界
    /// 此后逻辑按此操作
    /// </summary>
    /// <param name="slotIndex">存档下标</param>
    public void OnClickSaveSlot(int slotIndex)
    {
        if (!ValidateSlotIndex(slotIndex))
            return;

        if (saveLoad == null)
        {
            SetStatus("未绑定 GameDataSaveLoad，无法保存。");
            return;
        }

        saveLoad.SaveSlot(slotIndex);
        RefreshSlotsUI();
        SetStatus($"槽位 {slotIndex} 保存成功。");
    }
    /// <summary>
    /// 点击加载按钮方法
    /// </summary>
    /// <param name="slotIndex"></param>
    public void OnClickLoadSlot(int slotIndex)
    {
        if (!ValidateSlotIndex(slotIndex))
            return;

        if (saveLoad == null)
        {
            SetStatus("未绑定 GameDataSaveLoad，无法读档。");
            return;
        }

        if (LoadManager.Instance == null)
        {
            SetStatus("未找到 LoadManager，无法开始场景加载。");
            return;
        }

        if (!saveLoad.TryReadSlotRawData(slotIndex, out GameData loadedData))
        {
            SetStatus($"槽位 {slotIndex} 不存在或读取失败。");
            return;
        }

        string targetScene = loadedData != null ? loadedData.sceneName : null;
        if (string.IsNullOrEmpty(targetScene))
        {
            targetScene = DefaultLoadSceneName;
            SetStatus($"槽位 {slotIndex} 为旧存档，未记录场景，已回退到 {targetScene}。");
        }

        if (!Application.CanStreamedLevelBeLoaded(targetScene))
        {
            SetStatus($"目标场景 {targetScene} 未加入 Build Settings，无法读档。");
            return;
        }
        //跨场景一定会中断，选择将下标存入静态变量当中
        GameDataSaveLoad.SetPendingLoadSlot(slotIndex);
        SetStatus($"正在加载场景 {targetScene}，加载完成后将还原槽位 {slotIndex}。");
        LoadManager.Instance.StartLoading(targetScene, true);
    }
    /// <summary>
    /// 点击删除按钮方法
    /// </summary>
    /// <param name="slotIndex"></param>
    public void OnClickDeleteSlot(int slotIndex)
    {
        if (!ValidateSlotIndex(slotIndex))
            return;

        if (saveLoad == null)
        {
            SetStatus("未绑定 GameDataSaveLoad，无法删除。");
            return;
        }

        bool deleted = saveLoad.DeleteSlot(slotIndex);
        RefreshSlotsUI();
        SetStatus(deleted ? $"槽位 {slotIndex} 删除成功。" : $"槽位 {slotIndex} 不存在。");
    }
    /// <summary>
    /// 点击关闭呗
    /// </summary>
    public void OnClickClose()
    {
        if (UIManager.Instance == null)
            return;

        UIManager.Instance.PopOut();
    }
    /// <summary>
    /// 刷新UI方法
    /// </summary>
    public void RefreshSlotsUI()
    {
        slotMetaMap.Clear();

        if (saveLoad != null)
        {
            //调用JSON保存工具里的获取信息方法
            List<JsonProcess.SaveSlotMeta> metas = saveLoad.GetSlotsMeta();
            //将List拆解到字典内方便后续查找
                for (int i = 0; i < metas.Count; i++)
                    slotMetaMap[metas[i].slotIndex] = metas[i];
        }

        if (slotLabels == null)
            return;

        for (int i = 0; i < slotLabels.Length; i++)
        {
            if (slotLabels[i] == null)
                continue;

            int slotIndex = i + 1;
            //这里的过程是通过字典查找到的信息拼接到原来的字符串之上的做法
            if (!slotMetaMap.TryGetValue(slotIndex, out JsonProcess.SaveSlotMeta meta))
            {
                slotLabels[i].text = $"存档 {slotIndex}: 空";
                continue;
            }
            DateTime localTime = new DateTime(meta.lastWriteUtcTicks, DateTimeKind.Utc).ToLocalTime();
            slotLabels[i].text = $"存档 {slotIndex}: {localTime:yyyy年MM月dd日 HH时mm分}";
        }
    }
    /// <summary>
    /// 提供保护判断
    /// </summary>
    /// <param name="slotIndex"></param>
    /// <returns></returns>
    private bool ValidateSlotIndex(int slotIndex)
    {
        if (slotIndex <= 0)
        {
            SetStatus("槽位编号必须大于 0。");
            return false;
        }

        return true;
    }
    /// <summary>
    /// 设置文本信息
    /// </summary>
    /// <param name="message"></param>
    private void SetStatus(string message)
    {
        if (statusText != null)
            statusText.text = message;

        Debug.Log($"SavePanelUI: {message}");
    }
}
