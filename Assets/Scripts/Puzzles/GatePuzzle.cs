using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatePuzzle : MonoBehaviour
{
    private Animator anim;
    private PlayerData pd;

    // Start is called before the first frame update
    void Start()
    {
        pd = GameManager.Instance.player.GetComponent<PlayerData>();
        anim = gameObject.GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && pd.hasGateKey)
        {
            int layerMask = 1 << 14;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 2, layerMask))
            {
                if (hit.collider.gameObject.CompareTag("Lock"))
                {
                    var item = pd.questsInventory.FindItemInInventory(0);//this searches for the ID of the scriptable item
                    anim.SetBool("GateUnlocked", true);
                    gameObject.GetComponent<Collider>().enabled = false;
                    pd.hasGateKey = false;
                    item.RemoveItem();
                }
            }
        }
    }
}
