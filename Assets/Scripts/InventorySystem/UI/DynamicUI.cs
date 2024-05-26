using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicUI : InventoryUI
{
    #region Variables
    #region Grid Setup
    [SerializeField]
    private float xStart = -185.0f,
        yStart = 280.0f,
        xSpace = 80.0f,
        ySpace = 80.0f;
    [SerializeField]
    private int numberCols = 5;
    [SerializeField]
    private GameObject slotPrefab;
    #endregion
    #endregion

    #region Functions
    protected override void CreateSlots()
    {
        UISlots = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            var obj = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetLocalPos(i);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragBegin(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });

            inventory.GetSlots[i].slotDisplay = obj;
            UISlots.Add(obj, inventory.GetSlots[i]);
        }
    }
    private Vector3 GetLocalPos(int i)
    {
        return new Vector3(xStart + (xSpace * (i % numberCols)), yStart + (-ySpace * (i / numberCols)), 0f);
    }





    #endregion
}
