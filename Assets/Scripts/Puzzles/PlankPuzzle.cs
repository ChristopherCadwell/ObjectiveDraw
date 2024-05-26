using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankPuzzle : MonoBehaviour
{
    [SerializeField] private GameObject plank;
    private PlayerData pd;

    // Start is called before the first frame update
    void Start()
    {
        pd = GameManager.Instance.player.GetComponent<PlayerData>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(("Player")))
        {
            if (pd.IsCarryingPlank && Input.GetKey(KeyCode.E))
            {
                var item = pd.questsInventory.FindItemInInventory(5);//this searches for the ID of the scriptable item
                item.RemoveItem();
                plank.SetActive(true);
                pd.IsCarryingPlank = false;
            }
        }
    }
}
