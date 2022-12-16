using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 单例类：事件机制
/// </summary>
public class EventManager:Singleton<EventManager>
{
    /// <summary>
    /// 事件：点击物品
    /// </summary>
    /// <param name="item"></param>
    public delegate void OnItemClickDelegate(DropItem item);
    public event OnItemClickDelegate onItemClick;

    /// <summary>
    /// 定义捡起物品事件
    /// </summary>
    public delegate void OnDropItemPick(DropItem dropItem);
    public event OnDropItemPick onDropItemPick;

    /// <summary>
    /// 使用物品事件
    /// </summary>
    /// <param name="dropItem">被使用的物品</param>
    public delegate void UseItem(DropItem dropItem);
    public event UseItem useItemEvent;

    /// <summary>
    /// 敌人死亡事件
    /// </summary>
    /// <param name="type">敌人种类(1:蜗牛；2:食人花)</param>
    public delegate void CritterDead(int type);
    public event CritterDead critterDeadEvent;

    /// <summary>
    /// 事件的调用函数
    /// </summary>
    /// <param name="item"></param>
    public void OnItemClickEvent(DropItem item)
    {
        if (onItemClick != null)
        {
            onItemClick(item);
        }
    }

    /// <summary>
    /// 捡起物品事件的调用函数
    /// </summary>
    /// <param name="dropItem">被捡起的物品</param>
    public void OnDropItemPickEvent(DropItem dropItem)
    {
        if (onDropItemPick != null)
        {
            onDropItemPick(dropItem);
        }
    }
    /// <summary>
    /// 调用-使用物品事件
    /// </summary>
    /// <param name="dropItem"></param>
    public void OnUseItemEvent(DropItem dropItem)
    {
        if (useItemEvent != null)
        {
            useItemEvent(dropItem);
        }
    }
    /// <summary>
    /// 敌人死亡事件的调用函数
    /// </summary>
    /// <param name="type"></param>
    public void OnCritterDead(int type)
    {
        if (critterDeadEvent != null)
        {
            critterDeadEvent(type);
        }
    }
}