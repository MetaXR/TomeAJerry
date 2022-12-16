using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 背包控制脚本
/// </summary>
public class BagController : MonoBehaviour
{
    /// <summary>
    /// 全部物品
    /// </summary>    
    public List<DropItem> ItemList = new List<DropItem>();
    public BagSO bagSO;//---背包的数据文件
    /// <summary>
    /// 物品预制体
    /// </summary>
    public GameObject ItemPrefab;
    /// <summary>
    /// 物品父物体
    /// </summary>
    public Transform ItemParent;
    /// <summary>
    /// 金币数据文件
    /// </summary>
    public MoneySO pigMoney=default;
    //金币数量文本
    public Text coin;
    //蓝钻数量文本
    public Text diamond;

    void Awake()
    {
        //代码启动时，开始监听-物品被点击的事件onItemClick，若监听到这个事件则信息窗口执行窗口刷新函数RefreshUI.
        EventManager.Instance.onDropItemPick += AddAnItem;
    }

    void OnDestroy()
    {
        //代码销毁后，信息展示窗口将不再监听-物品被点击事件
        EventManager.Instance.onDropItemPick -= AddAnItem;
    }

    /// <summary>
    /// 初始化背包物品
    /// </summary>
    void Init()
    {        
        //判断物品列表是否为空，若为空则打印日志并不在继续执行
        if (bagSO.BagItems == null)
        {
            Debug.Log("Item is null");
            return;
        }
        int j = 0;
        //遍历背包物品列表---2.替换为使用背包的数据文件
        foreach (DropItem item in bagSO.BagItems)
        {
            //若取出的物品的类型是皮肤，则继续执行，背包将展示皮肤类的物品
            if (item.ItemType == BagItemType.skin)
            {
                //实例化一个物品图标
                GameObject obj = Instantiate<GameObject>(ItemPrefab);
                //放到物品父物体下
                obj.transform.SetParent(ItemParent);
                //依据j排列物品图标，意思是把物品排列成水平的等间距的一排
                obj.transform.localPosition = new Vector3(-690 + j * 140, 340, 0);
                //初始化物品UI上的信息，有2个：物品图标和物品数量
                obj.GetComponent<ItemController>().Init(item);
                j++;
            }
        }
        j = 0;

        //把金币数据文件中的金币数量，赋值给金币数量文本
        coin.text = pigMoney.Coin.ToString();
        //把金币数据文件中的蓝钻数量，赋值给蓝钻数量文本
        diamond.text = pigMoney.Diamond.ToString();
    }

    /// <summary>
    /// 刷新背包页面,或者切换种类。
    /// </summary>
     void RefreshUI(BagItemType type)
    {
        for(int i=0;i<ItemParent.childCount; i++)
        {
            Destroy(ItemParent.GetChild(i).gameObject);
        }
        int j = 0;
        //遍历背包物品列表---3.替换为使用背包的数据文件
        foreach (DropItem item in bagSO.BagItems)
        {
            //找到玩家选择的物品种类，则继续执行，背包将展示该类物品
            if (item.ItemType == type)
            {
                //实例化一个物品图标
                GameObject obj = Instantiate<GameObject>(ItemPrefab);
                //放到物品父物体下
                obj.transform.SetParent(ItemParent);
                //依据j排列物品图标，意思是把物品排列成水平的等间距的一排
                obj.transform.localPosition = new Vector3(-690 + j * 140, 340, 0);
                //初始化物品UI上的信息，有2个：物品图标和物品数量
                obj.GetComponent<ItemController>().Init(item);
                j++;
            }
        }
        j = 0;

        //把金币数据文件中的金币数量，赋值给金币数量文本
        coin.text = pigMoney.Coin.ToString();
        //把金币数据文件中的蓝钻数量，赋值给蓝钻数量文本
        diamond.text = pigMoney.Diamond.ToString();
    }    
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 当用户点击关闭按钮时，背包页面关闭。
    /// </summary>
    public void OnCloseBtnClick()
    {
        this.gameObject.SetActive(false);
    }
    /// <summary>
    /// 点击类型，切换背包物品
    /// </summary>
    /// <param name="type">玩家点击的是哪一个分类</param>
    public void OnTypeBtnClick(int type)
    {
        //调用背包刷新函数，传入参数Type
        RefreshUI((BagItemType)type);
    }
    /// <summary>
    /// 向背包中添加一个物品
    /// </summary>
    /// <param name="dropItem"></param>
    private void AddAnItem(DropItem dropItem)
    {
        bagSO.InsertItem(dropItem);
    }    
}
