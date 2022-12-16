using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 武器脚本
/// </summary>
public class Weapon:MonoBehaviour
{
    [SerializeField]
    private int attackStrength = default;

    [SerializeField]
    private bool enable = default;
    //是否启动
    [SerializeField]
    public bool Enable { get=>enable;set { enable = value; } }
    //攻击值
    public int AttackStrength
    {
        get => attackStrength;
    }

    void Awake()
    {
        //代码启动时，开始监听-使用物品事件，事件发生时执行响应函数ReceiveAnItem
        EventManager.Instance.useItemEvent += ReceiveAnItem;
    }

    void OnDestroy()
    {
        //代码销毁后，不再监听-使用物品事件
        EventManager.Instance.useItemEvent -= ReceiveAnItem;
    }

    /// <summary>
    /// 使用物品之后的效果函数
    /// </summary>
    /// <param name="dropItem"></param>
    private void ReceiveAnItem(DropItem dropItem)
    {        
        //佩戴匕首，战力爆表
        attackStrength = dropItem.ItemType == BagItemType.weapon ? attackStrength * System.Convert.ToInt32(1 + 0.2) : attackStrength;
    }

}
