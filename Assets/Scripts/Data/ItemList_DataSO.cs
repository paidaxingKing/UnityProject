using UnityEditor;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(menuName = "RPG SetUp/Item Data/Item list", fileName = "List of items - ")]

public class ItemList_DataSO : ScriptableObject
{
    public Item_DataSO[] itemList;

    public Item_DataSO GetItemData(string saveID)
    {
        return itemList.FirstOrDefault(item => item != null && item.saveID == saveID);
    }
    /*
     你创建一个 ItemList_DataSO 资源。
    在 Inspector 里执行 Auto-fill with all Item_DataSO。
    Unity 搜索项目中所有 Item_DataSO 资源。
    把它们加载出来。
    填入 itemList 数组。
    标记资源已修改并保存。
     */
#if UNITY_EDITOR
    [ContextMenu("Auto-fill with all Item_DataSO")]
    public void CollectItemsData()
    {
        string[] guids = AssetDatabase.FindAssets("t:Item_DataSO");//返回GUID

        itemList = guids
                .Select(guid => AssetDatabase.LoadAssetAtPath<Item_DataSO>(AssetDatabase.GUIDToAssetPath(guid)))
                .Where(item => item != null)
                .ToArray();

    EditorUtility.SetDirty(this);
    AssetDatabase.SaveAssets();
    }
#endif
}
