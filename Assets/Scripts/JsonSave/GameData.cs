using NUnit.Framework;
using System;
using UnityEngine;
using System.Collections.Generic;
[Serializable]
///<summary>
///负责数据存储的类，包含所有游戏数据类型  
///</summary>
public class GameData
{
    public string sceneName;
    public InventoryDto inventory=new InventoryDto();
    public PlayerSnapshotDto player = new PlayerSnapshotDto();
}
[Serializable]
///<summary>
///背包数据传输对象 
///</summary>
public class InventoryDto
{
    public int slotCount;
    public List<InventorySlotDto> slotId=new List<InventorySlotDto>();
}
[Serializable]
///<summary>
///单个格子数据
///</summary>
public class InventorySlotDto
{
    public string id = "";
    public int amount;
}

[Serializable]
public class PlayerSnapshotDto
{
    public int currentHealth;
    public float posX;
    public float posY;
    public float facingX = 1f;
}

