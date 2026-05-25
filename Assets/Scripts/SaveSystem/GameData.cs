using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class GameData
{
    public List<Inventory_Item> itemList;
    public SerializableDictionary<string, int> inventory;//物品ID和该物品的数量
    public SerializableDictionary<string, ItemType> equipedItems;

    public SerializableDictionary<string, bool> skillTreeUI;

    public SerializableDictionary<SkillType, SkillUpgradeType> skillUpgrades;

    public SerializableDictionary<string, bool> unLockedCheckpoints;

    public int skillPoints = 10;
    public string lastScenePlayed;

    public Vector3 lastPlayerPosition;

    //普通字典类型无法序列化，即无法转化为json形式，需要用类的形式自定义字典来替代普通字典
    public GameData()
    {
        inventory = new SerializableDictionary<string, int>();
        equipedItems = new SerializableDictionary<string, ItemType>();
        skillTreeUI = new SerializableDictionary<string, bool>();
        skillUpgrades = new SerializableDictionary<SkillType, SkillUpgradeType>();
        unLockedCheckpoints = new SerializableDictionary<string, bool>();
    }
}
