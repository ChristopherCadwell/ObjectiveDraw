using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Inventory System/Items/Key", fileName ="KeyName.asset")]
public class KeyObject : ItemObject
{
    public enum KeyType
    {
        GateKey, 
        Key2, 
        Key3
    }
    public KeyType keyType;

    public void Awake()
    {
        type = ItemType.Key;
        
    }
}

