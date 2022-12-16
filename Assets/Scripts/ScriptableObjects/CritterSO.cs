using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 蜗牛数据
/// </summary>
[CreateAssetMenu(fileName = "CritterSO", menuName = "Critter/CritterSO")]//创建选项
public class CritterSO : ScriptableObject//基类
{
    [SerializeField]
    private string _name;
    [SerializeField]
    private int _maxHealth;
    [SerializeField]
    private int _damage;
    [SerializeField]
    private float _waitTime = default;
    [SerializeField]
    private float _roamingSpeed = default;
    [SerializeField]
    private float _chasingSpeed = default;
    [SerializeField]
    private float _roamingDistance = default;
    [SerializeField]
    private int _maxDroppedItemsNum = 1;
    [SerializeField]
    private List<DropItem> _dropItems = new List<DropItem>();

    [SerializeField]
    public string Name => _name;//蜗牛的名字
    [SerializeField]
    public int MaxHealth //最大的生命值
    {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }
    [SerializeField]
    public int Damage//蜗牛的伤害值，是指对别人造成的伤害
    {
        get { return _damage; }
        set { _damage = value; }
    }
    [SerializeField]
    public float WaitTime => _waitTime;//等待时间
    [SerializeField]
    public float RoamingSpeed => _roamingSpeed;//巡逻速度
    [SerializeField]
    public float ChasingSpeed => _chasingSpeed;//追逐速度
    [SerializeField]
    public float RoamingDistance => _roamingDistance; //漫游距离
    [SerializeField]
    public int MaxDroppedNum => _maxDroppedItemsNum;//最多能掉落多少个物品
    [SerializeField]
    public List<DropItem> DroppedItems=> _dropItems;//掉落物品列表

    /// <summary>
    /// 获取掉落物品的数量
    /// </summary>
    /// <returns></returns>
    public int GetDroppedItemsNum()
    {
        return Mathf.CeilToInt(Random.Range(0.0f,_maxDroppedItemsNum));
    }
    /// <summary>
    /// 随机获取一个掉落物品
    /// </summary>
    /// <returns></returns>
    public DropItem GetRandomItem()
    {
        int index = Mathf.CeilToInt(Random.Range(0,_maxDroppedItemsNum-1));
        return _dropItems[index];

    }
}
