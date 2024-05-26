using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tabs : MonoBehaviour
{
    [SerializeField]
    private Canvas quest,
        collections;
    private void OnEnable()
    {
        EnableQuest();
    }

    public void EnableQuest()
    {
        quest.gameObject.SetActive(true);
        collections.gameObject.SetActive(false);
    }
    public void EnableCollections()
    {
        quest.gameObject.SetActive(false);
        collections.gameObject.SetActive(true);
    }
}
