using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Inventory System/Items/Blueprint", fileName = "Blueprint.asset")]
public class BlueprintItemObject: ItemObject
{
    public enum BlueprintName
    {
        AxeBlueprint,
        LadderBlueprint,
        PickaxeBlueprint,
        Blueprint4,
        Blueprint5
    }
    public BlueprintName printType;
    public void Awake()
    {
        type = ItemType.Blueprint;
    }
}
