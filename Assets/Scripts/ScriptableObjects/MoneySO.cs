using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 金币数据
/// </summary>
[CreateAssetMenu(fileName = "Bag", menuName = "Bag/MoneySO")]
public class MoneySO : ScriptableObject
{
    [SerializeField]
    private int _coin;
    [SerializeField]
    private int _diamond;

    [SerializeField]
    public int Coin //金币
    {
        get { return _coin; }
        set { _coin = value; }
    }

    [SerializeField]
    public int Diamond //蓝钻
    {
        get { return _diamond; }
        set { _diamond = value; }
    }
}
