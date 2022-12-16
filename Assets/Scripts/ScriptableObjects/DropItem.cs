using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 掉落物品的信息
/// </summary>
[CreateAssetMenu(fileName = "DroppItem", menuName = "Critter/DropItem", order = 2)]
public class DropItem:ScriptableObject
{
    [SerializeField]
    private int _id;
    [SerializeField]
    private string _name;
    [SerializeField]
    private string _previewImage;
    [SerializeField]
    private string _description;
    [SerializeField]
    private BagItemType _itemType = default;
    [SerializeField]
    private GameObject _prefab;

    [SerializeField]
    public int Id => _id;
    [SerializeField]
    public string Name => _name;
    [SerializeField]
    public string PreviewImage => _previewImage;
    [SerializeField]
    public string Description => _description;
    [SerializeField]
    public BagItemType ItemType => _itemType;
    [SerializeField]
    public GameObject Prefab => _prefab;
}
