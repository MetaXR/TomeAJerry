using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 背包的数据脚本
/// </summary>
[CreateAssetMenu(fileName = "BagSO", menuName = "Bag/BagSO")]
public class BagSO : ScriptableObject
{
    //物品分类
    [SerializeField]
    private BagItemType bagType=default;
    //物品列表
    [SerializeField]
    private List<DropItem> bagItems = new List<DropItem>();

    [SerializeField]
    public BagItemType BagType
    {
        get { return bagType; }
    }
    [SerializeField]
    public List<DropItem> BagItems
    {
        get { return bagItems; }
        set { bagItems = value; }
    }
    /// <summary>
    /// 向背包的物品列表中插入一个物品
    /// </summary>
    /// <param name="dropItem">掉落的物品</param>
    public void InsertItem(DropItem dropItem)
    {
        bagItems.Add(dropItem);
    }
}
