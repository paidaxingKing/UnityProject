using System;
using UnityEngine;

[Serializable]
public class Inventory_Item 
{
    public string itemId;

    public Item_DataSO itemData;
    public int stackSize = 1;//物品的当前堆叠数量

    public ItemModifier[] modifiers { get; private set; }

    public ItemEffect_DataSO itemEffect;

    public Inventory_Item(Item_DataSO itemData)
    {
        this.itemData = itemData;

        modifiers = GetEquipmentData()?.modifiers;

        itemId = itemData.itemName +" - " + Guid.NewGuid();
        itemEffect = itemData.effect;
        
    }

    public void AddModifiersTo(Entity_Stats playerStats)
    {
        foreach (var mod in modifiers)
        {
            Stat statToModify = playerStats.GetStatByType(mod.statType);
            statToModify.AddModifier(mod.value, itemId);
        }
    }

    public void RemoveModifiersFrom(Entity_Stats playerStats)
    {
        foreach (var mod in modifiers)
        {
            Stat statToModify = playerStats.GetStatByType(mod.statType);

            statToModify.RemoveModifier(itemId);
        }
    }

    public void AddItemEffect(Player player)
    {
        itemEffect?.Subscribe(player);
    }

    public void RemoveItemEffect()
    {
        itemEffect?.Unsubscribe();
    }

    private Equipment_DataSO GetEquipmentData()
    {
        if (itemData is Equipment_DataSO equipment)
        {
            return equipment;
        }
        return null;
    }

    public bool CanAddStack()
    {
        return stackSize < itemData.maxStackSize;
    }

    public void AddStack()
    {
        stackSize++;
    }

    public void RemoveStack()
    {
        stackSize--;
    }
}
