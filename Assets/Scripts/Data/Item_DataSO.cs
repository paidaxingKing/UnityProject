using UnityEngine;

using UnityEditor;

[CreateAssetMenu(menuName = "RPG SetUp/Item Data/Material item", fileName = "Material Data - ")]

public class Item_DataSO : ScriptableObject
{
    public string saveID { get; private set; }

    [Header("Drop Details")]
    [Range(0, 1000)]
    public int itemRarity = 100;
    [Range(0, 100)]
    public float dropChance;

    [Range(0,100)]
    public float maxDropChance = 65f;


    [Header("Item Details")]
    public string itemName;
    public Sprite itemIcon;
    public ItemType itemType;

    public int maxStackSize = 1;//物品最大堆叠数量

    [Header("Item Effect")]
    public ItemEffect_DataSO effect;

    private void OnValidate()
    {
        dropChance = GetDropChance();

#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);//获得该物件的资产路径，相对于Assets文件夹的相对路径
        saveID = AssetDatabase.AssetPathToGUID(path);
#endif
    }

    public float GetDropChance()
    {
        float maxRarity = 1000;
        float chance = (maxRarity - itemRarity + 1) / maxRarity * 100;

        return Mathf.Min(chance, maxDropChance);
    }

}
