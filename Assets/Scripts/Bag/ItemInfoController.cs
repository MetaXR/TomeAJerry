using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 物品信息窗口控制脚本
/// </summary>
public class ItemInfoController : MonoBehaviour
{
    /// <summary>
    /// 窗口标题
    /// </summary>
    public Text title;
    /// <summary>
    /// 物品图片
    /// </summary>
    public Image png;
    /// <summary>
    /// 物品介绍
    /// </summary>
    public Text intro;
    /// <summary>
    /// 掉落的物品
    /// </summary>
    public DropItem currentItem;

    /// <summary>
    /// 窗口初始化函数 1.Item类型替换成DropItem
    /// </summary>
    /// <param name="item">要展示的物品数据</param>
    public void Init(DropItem item)
    {
        currentItem = item;
        //物品名字展示在窗口标题上
        title.text = item.Name;
        //物品图片展示在窗口图片位置
        png = Resources.Load<Image>(item.PreviewImage);
        //物品介绍展示在窗口底部文字内容位置
        intro.text = item.Description;
        
    }
    /// <summary>
    /// 窗口刷新函数 2.Item类型替换成DropItem
    /// </summary>
    /// <param name="item">要展示的物品数据</param>
    public void RefreshUI(DropItem item)
    {
        currentItem = item;
        //物品名字展示在窗口标题上
        title.text = item.Name;
        //物品图片展示在窗口图片位置
        png = Resources.Load<Image>(item.PreviewImage);
        //物品介绍展示在窗口底部文字内容位置
        intro.text = item.Description;
    }
    void Awake()
    {
        //代码启动时，开始监听物品被点击的事件onItemClick
        //若监听到这个事件，则信息窗口执行窗口刷新函数RefreshUI()
        EventManager.Instance.onItemClick += RefreshUI;
    }

    void OnDestroy()
    {
        //代码销毁后，信息窗口将不再监听物品被点击事件
        EventManager.Instance.onItemClick -= RefreshUI;
    }
    /// <summary>
    /// 点击使用按钮，广播使用物品事件
    /// </summary>
    public void OnButtonUseClick()
    {
        EventManager.Instance.OnUseItemEvent(currentItem);
    }
}
