using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Database/Item Database", fileName = "ItemDB")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    [Header("Every item that goes into this inventory"), Tooltip("Make sure to add every scriptable object item to this list")]
    public ItemObject[] ItemObjects;

    public Dictionary<int, ItemObject> GetItem = new Dictionary<int, ItemObject>();

    [ContextMenu("Update IDs")]
    public void UpdateIDs()
    {
        for (int i = 0; i < ItemObjects.Length; i++)
        {
            if (ItemObjects[i].data.id != i)
                ItemObjects[i].data.id = i;
        }
    }
    public void OnAfterDeserialize()
    {
            UpdateIDs();
    }

    public void OnBeforeSerialize()
    {
    }
}
