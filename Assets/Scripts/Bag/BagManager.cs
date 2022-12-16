using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 背包物品类型
/// </summary>
public enum BagItemType 
{
    food=1,//食物
    weapon=2,//武器
    skin=3//皮肤
}
/// <summary>
/// 物品
/// </summary>
public class Item
{
    public int id;
    public BagItemType type;
    public string name;
    public int num;
    public string icon;
    public string introduction;

    public Item(int Id, BagItemType Type, string Name, int Num, string Icon, string Intro)
    {
        id = Id;
        type = Type;
        name = Name;
        num = Num;
        icon = Icon;
        introduction = Intro;
    }
}
/// <summary>
/// 背包管理脚本
/// </summary>
/// <typeparam name="BagManager"></typeparam>
public class BagManager: Singleton<BagManager>
{
  
}
