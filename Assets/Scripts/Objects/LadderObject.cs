using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderObject : MonoBehaviour
{
    //sets the location and the gameobject
    [SerializeField] private Transform placeTarget;
    public GameObject theLadder;

    //places ladder in set location
    public void PlaceLadder()
    {
        theLadder.transform.position = placeTarget.transform.position;
        theLadder.layer = 10;
        SetLayerRecursively(theLadder, 10);
        GameManager.Instance.player.GetComponent<PlayerData>().canPickUp = false;
    }

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        //if (obj == null) return;

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (child == null) continue;

            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}