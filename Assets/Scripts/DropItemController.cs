using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 物品的控制脚本
/// </summary>
public class DropItemController : MonoBehaviour
{
    [SerializeField]
    private DropItem currentItem=default;
    /// <summary>
    /// 物品数据
    /// </summary>
    public DropItem CurrentItem
    {
        set => currentItem = value;
    }

    public DropItem GetItem()
    {
        return currentItem;
    }

    void Awake()
    {
        EventManager.Instance.onDropItemPick += OnPicked;//为物品的捡起事件，添加-响应函数OnPicked
    }

    void OnDestroy()
    {
        EventManager.Instance.onDropItemPick -= OnPicked;//为物品的捡起事件，去掉-响应函数OnPicked

    }

    /// <summary>
    /// 触发器函数
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //发送事件-物品被捡起事件-。
            EventManager.Instance.OnDropItemPickEvent(currentItem);
        }
    }
    /// <summary>
    /// 物品被捡起来之后，立刻销毁物体
    /// </summary>
    /// <param name="dropItem"></param>
    void OnPicked(DropItem dropItem)
    {
        if (dropItem.Id == currentItem.Id && dropItem.Name==dropItem.Name)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
