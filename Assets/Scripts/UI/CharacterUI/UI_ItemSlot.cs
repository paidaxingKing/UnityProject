using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour,IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler
{
    public Inventory_Item itemInSlot { get; private set; }
    protected Inventory_Player inventory;
    protected UI ui;
    protected RectTransform rect;

    [Header("UI Slot Setup")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemStackSize;

    [SerializeField] private Image defaultIcon;
    [SerializeField] Sprite originIcon;

    private event Action discardItem;

    private void Awake()
    {
        inventory = FindAnyObjectByType<Inventory_Player>();
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        discardItem += DiscardItem;
    }

    private void DiscardItem()
    {
        inventory.RemoveItem(itemInSlot);
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {

        if (itemInSlot == null) return;

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            ui.confirmationDialogUI.Show("Discard",discardItem);
            return;
        }

        if (itemInSlot.itemData.itemType == ItemType.Material) return;


        if (itemInSlot.itemData.itemType == ItemType.Consumable)
        {
            inventory.TryUseItem(itemInSlot);
        }
        else
        {
            inventory.TryEquipItem(itemInSlot);
        }
      
        ui.itemToolTip.ShowToolTip(false, null);      
    }

    public void UpdateSlot(Inventory_Item item)
    {
        itemInSlot = item;

        if (itemInSlot == null)
        {
            itemStackSize.text = "";
            defaultIcon.gameObject.SetActive(true);
            itemIcon.sprite = originIcon;
            return;
        }
        defaultIcon.gameObject.SetActive(false);
        Color color = Color.white;
        itemIcon.color = color;

        itemIcon.sprite = itemInSlot.itemData.itemIcon;
        itemStackSize.text = itemInSlot.stackSize > 1 ? itemInSlot.stackSize.ToString() : "";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemInSlot == null) return;

        ui.itemToolTip.ShowToolTip(true, rect, itemInSlot);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.itemToolTip.ShowToolTip(false, null);
    }
}
