using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 物品脚本
/// </summary>
public class ItemController : MonoBehaviour
{
    /// <summary>
    /// 当前物品---类Item替换为物品的数据脚本类DropItem
    /// </summary>
    public DropItem CurItem;
    /// <summary>
    /// 物品图标
    /// </summary>
    public Image itemIcon;
    /// <summary>
    /// 物品数量
    /// </summary>
    public Text itemNum;

    /// <summary>
    /// 初始化物品---1.修改函数变量
    /// </summary>
    /// <param name="item">类型DropItem</param>
    public void Init(DropItem item)
    {
        //保存物品数据到当前物品
        CurItem = item;
        //把图片加载进工程，并将其赋值给物品图标
        itemIcon.sprite = Resources.Load<Sprite>(item.PreviewImage);
        //itemIcon.SetNativeSize();
        //将物品数量显示到物品UI上。
        itemNum.text = item.Id.ToString();
    }
    
    /// <summary>
    /// 点击物品将调用的响应函数
    /// </summary>
    public void OnItemClick()
    {
        //发送事件：将当前物品信息传递给物品信息展示窗口
        EventManager.Instance.OnItemClickEvent(CurItem);
    }
}
