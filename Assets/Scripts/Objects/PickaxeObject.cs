using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeObject : MonoBehaviour
{
    //checks if swinging
    public bool isSwinging = false;
    public int hits;
    //sets if swinging or not
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.Instance.player.GetComponent<PlayerMovement>().objectCarrying == gameObject)
        {
            isSwinging = true;
            hits++;
        }

        if (!isSwinging)
        {
            hits = 0;
        }

        if (isSwinging)
        {
            int layerMask = 1 << 7;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 2, layerMask))
            {
                isSwinging = true;
                if (hit.collider.gameObject.tag == "Rock")
                {
                    if (hits == hit.collider.gameObject.GetComponent<BreakingRock>().hitsTillBroken)
                    {
                        hit.collider.gameObject.GetComponent<BreakingRock>().Break();
                        hits = 0;
                    }
                }
            }
            else
            {
                isSwinging = false;
            }
        }
    }
}