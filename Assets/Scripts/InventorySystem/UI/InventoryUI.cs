using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System;

public abstract class InventoryUI : MonoBehaviour
{
    public InventoryObject inventory;
    public bool isBpInventory = false;
    public UIController uiController;
    public Canvas infoBox;

    protected Dictionary<GameObject, InventorySlot> UISlots = new Dictionary<GameObject, InventorySlot>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            inventory.GetSlots[i].parent = this;
        }

        CreateSlots();
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(); });
    }

    private void Update()
    {
        if (isBpInventory)
        {
            UISlots.UpdateBPDisplay();
            return;
        }
        UISlots.UpdateSlotsDisplay();
    }
    protected abstract void CreateSlots();

    
    #region events
    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry
        {
            eventID = type
        };
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }
    public void OnPointerExit()
    {
       
    }
    public void OnEnter(GameObject obj)
    {
        if (UISlots[obj].ItemObject != null)
        {
            infoBox.gameObject.SetActive(true);
            MouseData.itemDescription = UISlots[obj].ItemObject.itemDescription;
            MouseData.slotHoveredOver = obj;

            Vector3 mousePos = Input.mousePosition;
            infoBox.transform.position = mousePos;
            infoBox.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = MouseData.itemDescription;
        } 
        MouseData.slotHoveredOver = obj;
    }

    public void OnExit()
    {
        MouseData.slotHoveredOver = null;
        MouseData.itemDescription = null;
        infoBox.gameObject.SetActive(false);
    }

    public void OnEnterInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = obj.GetComponent<InventoryUI>();
    }

    public void OnExitInterface()
    {
        MouseData.interfaceMouseIsOver = null;
    }

    public void OnDragBegin(GameObject obj)
    {
        if (isBpInventory)
            return;
        MouseData.tempItemBeingDragged = CreateTempItem(obj);
    }
    #endregion

    public GameObject CreateTempItem(GameObject obj)
    {
        if (isBpInventory)
            return null;
        GameObject tempItem = null;
        if (UISlots[obj].item.id >= 0)
        {
            tempItem = new GameObject();
            var rt = tempItem.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(50, 50);
            tempItem.transform.SetParent(transform.parent);
            var img = tempItem.AddComponent<Image>();
            img.sprite = UISlots[obj].ItemObject.interfaceDisplay;
            img.raycastTarget = false;
        }
        return tempItem;
    }

    public void OnDrag()
    {   if(isBpInventory)
            return;
        if (MouseData.tempItemBeingDragged != null)
            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
    }

    public void OnDragEnd(GameObject obj)
    {
        if (isBpInventory)
            return;
        Destroy(MouseData.tempItemBeingDragged);
        if (MouseData.interfaceMouseIsOver == null)
        {
            UISlots[obj].RemoveItem();
            return;
        }
        if (MouseData.slotHoveredOver)
        {
            InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.UISlots[MouseData.slotHoveredOver];
            inventory.SwapItems(UISlots[obj], mouseHoverSlotData);
        }
    }
    public void OnClick(GameObject obj)
    {
        if(isBpInventory)
        {
            if (UISlots[obj].ItemObject != null)
            {
                MouseData.bpSprite = UISlots[obj].ItemObject.blueprintImage;
                uiController.overlay.GetComponentInChildren<Image>().sprite = MouseData.bpSprite;
            }
            
        }
    }

}
public static class ExtensionMethods
{
    public static void UpdateSlotsDisplay(this Dictionary<GameObject, InventorySlot> _UISlots)
    {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in _UISlots)
        {
            if (_slot.Value.item.id >= 0)
            {
                if (_slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null)
                {
                    _slot.Key.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.ItemObject.name;
                    return;
                }
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = _slot.Value.ItemObject.interfaceDisplay;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString("n0");
            }
            else
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }

    public static void UpdateBPDisplay(this Dictionary<GameObject, InventorySlot> _UISlots)
    {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in _UISlots)
        {
            if (_slot.Value.item.id >= 0)
                _slot.Key.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.ItemObject.data.name;
        }
    }
}

