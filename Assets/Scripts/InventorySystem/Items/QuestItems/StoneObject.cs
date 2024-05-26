using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Inventory System/Items/Stone", fileName = "StoneName.asset")]
public class StoneObject : ItemObject
{
    public enum StoneType
    {
        Flint,
        Rock2,
        Rock3
    }
    public StoneType stoneType;
    public void Awake()
    {
        type = ItemType.Stone; 
    }
}
