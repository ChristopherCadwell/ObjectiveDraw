using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppingBlockPuzzle : MonoBehaviour
{
    [SerializeField] private GameObject log;
    private PlayerData pd;
    public int hitsTillBroken;

    [SerializeField] private GameObject chopppedLogObject1;
    [SerializeField] private GameObject chopppedLogObject2;
    private GameObject chopppedLogSpawn1;
    private GameObject chopppedLogSpawn2;
    
    void Start()
    {
        pd = GameManager.Instance.player.GetComponent<PlayerData>();
        chopppedLogSpawn1 = log.transform.GetChild(0).gameObject;
        chopppedLogSpawn2 = log.transform.GetChild(1).gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && pd.IsCarryingLog)
        {
            int layerMask = 1 << 8;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 3, layerMask))
            {
                var item = pd.questsInventory.FindItemInInventory(4);//this searches for the ID of the scriptable item
                if (!log.activeInHierarchy)
                {
                    item.AddAmount(-1);
                    log.SetActive(true);
                }
                if (item.amount == 0)
                {
                    pd.IsCarryingLog = false;
                    item.RemoveItem();
                }
            }
        }
    }

    public void Break()
    {
        if (log.activeSelf == true)
        {
            // Hides Big Log
            log.SetActive(false);
            // Spawn new logs
            Instantiate(chopppedLogObject1, chopppedLogSpawn1.transform.position, gameObject.transform.rotation);
            Instantiate(chopppedLogObject2, chopppedLogSpawn2.transform.position, gameObject.transform.rotation);
        }
        
    }
}
