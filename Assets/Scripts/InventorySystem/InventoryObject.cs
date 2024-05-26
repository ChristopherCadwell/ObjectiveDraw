using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEditor;
using System.Runtime.Serialization;

[CreateAssetMenu(menuName = "Inventory System/Inventory", fileName = "Inventory.asset")]
public class InventoryObject : ScriptableObject
{
    #region Variables
    [Header("Where to save inventory file"), Tooltip("Example '/save/location/file.extension'")]
    public string savePath;
    [Header("The Database Object")]
    public ItemDatabaseObject itemDB;
    [Header("The Inventory Object")]
    public Inventory container;
    public InventorySlot[] GetSlots { get { return container.slots; } }
    #endregion

    #region Functions
    
    //check to make sure this item is not already in inventory
    public bool AddItem(Item _item, int _amount)
    {
        if (EmptySlotCount <= 0)
            return false;
        InventorySlot slot = FindItemInInventory(_item);
        if (!itemDB.ItemObjects[_item.id].IsStackable || slot == null)
        {
            SetEmptySlot(_item, _amount);
            return true;
        }
        slot.AddAmount(_amount);
        return true;
    }
    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            for (int i = 0; i < GetSlots.Length; i++)
            {
                if (GetSlots[i].item.id <= -1)
                {
                    counter++;
                }
            }
            return counter;
        }
    }
    public InventorySlot SetEmptySlot(Item _item, int _amount)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.id <= -1)
            {
                GetSlots[i].UpdateSlot(_item, _amount);
                return GetSlots[i];
            }
        }
        //TODO setup full inventory
        return null;
    }
    public InventorySlot FindItemInInventory(Item _item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.id == _item.id)
            {
                return GetSlots[i];
            }
        }
        return null;
    }
    public InventorySlot FindItemInInventory(int ID)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.id == ID)
            {
                return GetSlots[i];
            }
        }
        return null;
    }
    #region Save/Load
    //Functions to save and load the inventory 
    [ContextMenu("Save")]
    public void Save()
    {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, container);
        stream.Close();

    }
    [ContextMenu("Load")]
    public void Load()
    {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
        Inventory newContainer = (Inventory)formatter.Deserialize(stream);
        for (int i = 0; i < GetSlots.Length; i++)
        {
            GetSlots[i].UpdateSlot(newContainer.slots[i].item, newContainer.slots[i].amount);
        }
        stream.Close();

    }
    //clear inventory
    [ContextMenu("Clear")]
    public void Clear()
    {
        container.Clear();
    }
    #endregion

    public void SwapItems(InventorySlot item1, InventorySlot item2)
    {
        if (item2.CanPlaceInSlot(item1.ItemObject) && item1.CanPlaceInSlot(item2.ItemObject))
        {
            InventorySlot temp = new InventorySlot(item2.item, item2.amount);
            item2.UpdateSlot(item1.item, item1.amount);
            item1.UpdateSlot(temp.item, temp.amount);
        }
    }
    public void RemoveItem(Item _item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item == _item)
                    GetSlots[i].UpdateSlot(null, 0);
        }
    }

    #endregion
}
[System.Serializable]
public class Inventory
{
    public InventorySlot[] slots = new InventorySlot[30];
    public void Clear()
    {
        for (int i = 0; i < slots.Length; i++)
            slots[i].RemoveItem();
    }
}

//create inventory to view
public delegate void SlotUpdated(InventorySlot _slot);

[System.Serializable]
public class InventorySlot
{
    #region Variables
    public AllowedType[] allowedItems = new AllowedType[1];
    public Item item;
    public int amount;

    [System.NonSerialized] public InventoryUI parent;
    [System.NonSerialized] public GameObject slotDisplay;
    [System.NonSerialized] public SlotUpdated OnAfterUpdate;
    [System.NonSerialized] public SlotUpdated OnBeforeUpdate;

    public ItemObject ItemObject
    {
        get
        {
            if (item.id >= 0)
            {
                return parent.inventory.itemDB.ItemObjects[item.id];
            }
            return null;
        }
    }
    #region Constructors
    public InventorySlot()
    {
        item = new Item();
        amount = 0;
    }
    public InventorySlot(Item _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }
    #endregion
    #endregion

    #region Functions
    
    public void RemoveItem()
    {
        item = new Item();
        amount = 0;
    }
    public void AddAmount(int val)
    {
        amount += val;
    }
    public void UpdateSlot(Item _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }
    public bool CanPlaceInSlot(ItemObject _itemObject)
    {
        if (allowedItems.Length <= 0 || _itemObject == null || _itemObject.data.id < 0)
            return true;
        for (int i = 0; i < allowedItems.Length; i++)
        {
            if (_itemObject.allowed == allowedItems[i])
                return true;
        }
        return false;
    }
    #endregion
}


