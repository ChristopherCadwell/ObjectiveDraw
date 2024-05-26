using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum ItemType
{
    Stone,
    Key,
    Log,
    Blueprint,
    Apple,
    Collectable
}
public enum QuestState
{
    None,
    hasGateKey,
    hasKey2,
    hasKey3,
    hasLog,
    hasPlank,
    hasBoard,
    hasFlint,
    hasStone2,
    hasStone3
}
public enum AllowedType
{
    All,
    Stone,
    Key,
    Log,
    Blueprint,
    Apple,
    Collectable
}
public abstract class ItemObject : ScriptableObject
{
    public Sprite interfaceDisplay;
    public bool IsStackable = false;
    [Header("This is what we will see in the inventory")]
    public ItemType type;
    [TextArea(20,20)]
    public string itemDescription = "Describe this object";
    public Item data = new Item();
    public QuestState state;
    public AllowedType allowed;
    public Sprite blueprintImage;

    public Item CreateItem()
    {
        Item newItem = new Item();
        return newItem;
    }
    public virtual void PickedUp()
    {
        switch (state)
        {
            case QuestState.hasGateKey:
                GameManager.Instance.playerMove.hasGateKey = true;
                break;

            case QuestState.hasKey2:
                GameManager.Instance.playerMove.hasKey2 = true;
                break;

            case QuestState.hasKey3:
                GameManager.Instance.playerMove.hasKey3 = true;
                break;

            case QuestState.hasLog:
                GameManager.Instance.playerMove.IsCarryingLog = true;
                break;

            case QuestState.hasPlank:
                GameManager.Instance.playerMove.IsCarryingPlank = true;
                break;

            case QuestState.hasBoard:
                GameManager.Instance.playerMove.isCarryingBoard = true;
                break;

            case QuestState.hasFlint:
                GameManager.Instance.playerMove.isCarryingFlint = true;
                break;

            case QuestState.hasStone2:
                GameManager.Instance.playerMove.isCarryingStone2 = true;
                break;

            case QuestState.hasStone3:
                GameManager.Instance.playerMove.isCarryingStone3 = true;
                break;

            default:
                break;
        }
        
    }
}

[System.Serializable]
public class Item
{
    public string name;
    public int id = -1;
    public readonly QuestState questState;
    public readonly AllowedType allowed;
    public Item()
    {
        name = "";
        id = -1;
        questState = QuestState.None;
        allowed = AllowedType.All;
    }
    public Item(ItemObject item)
    {
        name = item.name;
        id = item.data.id;
        questState = item.data.questState;
        allowed = item.data.allowed;
    }
    
}