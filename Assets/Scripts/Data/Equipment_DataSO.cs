using System;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG SetUp/Item Data/Equipment item", fileName = "Equipment Data - ")]
public class Equipment_DataSO : Item_DataSO
{
    [Header("Item Modifiers")]
    public ItemModifier[] modifiers;//表示这个物件可以修改哪些属性
}

[Serializable]
public class ItemModifier//表示能修改的属性
{
    public StatType statType;
    public float value;
}
