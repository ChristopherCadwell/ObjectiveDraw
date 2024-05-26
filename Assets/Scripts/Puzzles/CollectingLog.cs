using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectingLog : MonoBehaviour
{
    public GameObject splitLog;
    public PlayerData pd;
    // Start is called before the first frame update
    void Start()
    {
        pd = GameManager.Instance.player.GetComponent<PlayerData>();
    }
}
