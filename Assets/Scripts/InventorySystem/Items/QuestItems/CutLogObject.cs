using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Inventory System/Items/Log", fileName = "Logname.asset")]
public class CutLogObject : ItemObject
{
    public enum LogType
    {
        Log,
        Plank,
        Board
    }
    public LogType logType;
    public void Awake()
    {
        type = ItemType.Log;
    }
}
