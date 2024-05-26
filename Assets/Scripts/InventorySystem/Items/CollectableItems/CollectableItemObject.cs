using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Inventory System/Collectables/Collectable", fileName = "Collectable.asset")]
public class CollectableItemObject : ItemObject
{
    public enum CollectableName
    {
        Flower,
        Item2,
        Item3
    }
    public CollectableName collectable;
    public void Awake()
    {
        type = ItemType.Collectable;
    }
}
