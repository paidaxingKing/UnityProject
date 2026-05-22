using System;
using UnityEngine;

[Serializable]
public class Inventory_EquipmentSlot
{
    public ItemType slotType;
    public Inventory_Item equipedItem;

    public bool HasItem()
    {
        return equipedItem != null && equipedItem.itemData != null;
        //只要创建了卡槽，equipedItem就肯定不为Null，所以只能通过没有分配data数据来辨别是否装备了这个物品
    }

}
