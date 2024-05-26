using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StaticUI : InventoryUI
{
    [SerializeField]
    private GameObject[] slots;

    protected override void CreateSlots()
    {
        //loops through items in equipment and adds it to the inventory slots
        UISlots = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            var obj = slots[i];

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragBegin(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.PointerClick, delegate { OnClick(obj); });

            inventory.GetSlots[i].slotDisplay = obj;
            UISlots.Add(obj, inventory.GetSlots[i]);
        }
    }
}
