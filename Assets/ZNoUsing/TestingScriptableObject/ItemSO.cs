using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Data", menuName = "Inventory/Item", order = 2)]
public class ItemSO : ScriptableObject 
{
    public string objectName;    
    public string Desc;
    public bool canUse;
}