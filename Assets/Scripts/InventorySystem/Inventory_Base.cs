using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Base : MonoBehaviour,ISaveable
{

    public event Action OnInventoryChange;

    public int maxInventorySize = 10;//仓库中最多可以存放的物品种类数
    public List<Inventory_Item> itemList = new List<Inventory_Item>();//仓库的物品槽位
    
    protected virtual void Awake()
    {

    }

    public void TriggerUpdateUI()
    {
        OnInventoryChange?.Invoke();
    }

    public void TryUseItem(Inventory_Item itemToUse)
    {
        Inventory_Item consumable = itemList.Find(item => item == itemToUse);

        if (consumable == null) return;

        consumable.itemEffect.ExecuteEffect();

        if (consumable.stackSize > 1)
        {
            consumable.RemoveStack();
        }
        else
        {
            RemoveItem(consumable);
        }

        OnInventoryChange?.Invoke();
    }

    public bool CanAddItem()
    {
        return itemList.Count < maxInventorySize;
    }

    public Inventory_Item FoundStack(Inventory_Item itemToAdd)
    {
        return itemList.Find(item => item.itemData == itemToAdd.itemData && item.CanAddStack());        
    }

    public void AddItem(Inventory_Item item)
    {
        Inventory_Item itemInInventory = FoundStack(item);

        if (itemInInventory != null)
        {
            itemInInventory.AddStack();
        }
        else
        {
            itemList.Add(item);
        }

        OnInventoryChange?.Invoke();
    }

    public void RemoveItem(Inventory_Item item)
    {
        itemList.Remove(item);
        OnInventoryChange?.Invoke();
    }

    public Inventory_Item FindItem(Item_DataSO itemData)
    {
        return itemList.Find(item => item.itemData == itemData);
    }

    public virtual void LoadData(GameData data)
    {
        
    }

    public virtual void SaveData(ref GameData data)
    {
       
    }
}
