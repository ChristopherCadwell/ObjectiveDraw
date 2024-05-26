using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Inventory System/Items/Apple", fileName = "Apple.asset")]
public class AppleObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.Apple;
    }
}
