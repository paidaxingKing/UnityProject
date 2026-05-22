
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Player : Inventory_Base
{
    private Player player;
    public List<Inventory_EquipmentSlot> equipList;//装备物品槽位
    [SerializeField] private ItemList_DataSO itemDataBase;


    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<Player>();
    }

    public void UnequipItem(Inventory_Item itemToUnequip,bool replacingItem = false)
    {
        if (!CanAddItem() && !replacingItem) return;

        float savedHealthPercent = player.entity_Health.GetHealthPercent();

        foreach (var slot in equipList)
        {
            if (slot.equipedItem == itemToUnequip)
            {
                slot.equipedItem.RemoveModifiersFrom(player.entity_Stats);
                slot.equipedItem = null;
                AddItem(itemToUnequip);
                break;
            }
        }

        itemToUnequip.RemoveItemEffect();
        player.entity_Health.SetHealthToPercent(savedHealthPercent);
    }

    public void TryEquipItem(Inventory_Item item)//尝试装备物品
    {
        //var inventoryItem = FindItem(item.itemData);//是否有这个物品

        var machingSlots = equipList.FindAll(slot => slot.slotType == item.itemData.itemType);//是否有匹配的槽位,返回的可能是数组

        foreach (var slot in machingSlots)
        {
            if (!slot.HasItem())//如果槽位没有装备物品
            {
                EquipItem(item, slot);
                return;
            }
        }

        var slotToReplace = machingSlots[0];
        var itemToUnequip = slotToReplace.equipedItem;

        UnequipItem(itemToUnequip,slotToReplace != null);
        EquipItem(item, slotToReplace);

    }

    public void EquipItem(Inventory_Item item,Inventory_EquipmentSlot slot)//在某个槽位装备某物品
    {
        float savedHealthPercent = player.entity_Health.GetHealthPercent();

        slot.equipedItem = item;
        slot.equipedItem.AddModifiersTo(player.entity_Stats);
        slot.equipedItem.AddItemEffect(player);

        player.entity_Health.SetHealthToPercent(savedHealthPercent);

        RemoveItem(item);
    }

    public override void SaveData(ref GameData data)
    {
        data.inventory.Clear();
        data.equipedItems.Clear();

        foreach (var item in itemList)
        {
            if (item != null && item.itemData != null)
            {
                string saveID = item.itemData.saveID;
                int stack = item.stackSize;

                if (!data.inventory.ContainsKey(saveID))
                {
                    data.inventory[saveID] = 0;//自动创建键值对并赋值为0                
                }
                data.inventory[saveID] += stack;
            }
        }

        foreach (var slot in equipList)
        {
            if (slot.HasItem())
            {
                data.equipedItems[slot.equipedItem.itemData.saveID] = slot.slotType;
            }
        }


    }

    public override void LoadData(GameData data)
    {
        foreach (var item in data.inventory)//遍历保存的所有物品数据，然后重新加入到物品背包中
        {
            string saveID = item.Key;
            int stackSize = item.Value;

            Item_DataSO itemData = itemDataBase.GetItemData(saveID);

            if (itemData == null)
            {
                Debug.Log("没找到物品：" + saveID);
                continue;
            }

            for (int i = 0; i < stackSize; i++)
            {
                Inventory_Item itemToLoad = new Inventory_Item(itemData);
                AddItem(itemToLoad);
            }
        }

        foreach (var entry in data.equipedItems)//遍历所有装备槽，从保存的游戏数据中找是否存有该装备槽的装备数据
        {
            string saveID = entry.Key;
            ItemType itemType = entry.Value;

            Item_DataSO itemData = itemDataBase.GetItemData(saveID);
            Inventory_Item itemToLoad = new Inventory_Item(itemData);

            var slot = equipList.Find(slot => slot.slotType == itemType && !slot.HasItem());
            slot.equipedItem = itemToLoad;
            slot.equipedItem.AddModifiersTo(player.entity_Stats);
            slot.equipedItem.AddItemEffect(player);
        }

        TriggerUpdateUI();
    }
}
