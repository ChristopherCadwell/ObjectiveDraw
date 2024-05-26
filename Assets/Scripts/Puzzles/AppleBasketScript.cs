using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleBasketScript : MonoBehaviour
{
    private PlayerData pd;

    [SerializeField] private GameObject[] applesObjectList;
    private int applesNum;

    [SerializeField] private GameObject appleBox1;
    [SerializeField] private GameObject appleBox2;
    [SerializeField] private GameObject appleBox3;
    //Inventory
    [SerializeField] InventoryObject appleInventory;
  


    [SerializeField] private GameObject plankPrefab;
    //public GameObject plankSpawn;
    private bool plankHasSpawned = false;
    private bool canPickApples = true;

    private GameManager gameManager;
    private int tutText = 20;

    // Start is called before the first frame update
    void Start()
    {
        pd = GameManager.Instance.player.GetComponent<PlayerData>();
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(applesNum >= 12)
        {
            canPickApples = false;
            applesNum = 12;
        }

        //if (box1Filled && !plankHasSpawned)
        //{
        //    Instantiate(plankPrefab, plankSpawn.transform);
        //    plankHasSpawned = true;
        //}

        if (Input.GetKeyDown(KeyCode.E) && pd.isCarryingBasket && canPickApples)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 2))
            {
                ItemPickup item = hit.collider.GetComponent<ItemPickup>();
                if (hit.collider.gameObject.CompareTag("Apple"))
                {
                    if (item != null)
                    {
                        appleInventory.AddItem(new Item(item.item), 1);
                        item.item.PickedUp();
                        Destroy(hit.collider.gameObject);
                        applesNum++;
                        for (int i = 0; i < applesNum; i++)
                        {
                            applesObjectList[i].SetActive(true);
                        }
                    }
                    
                }
            }
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("AppleStand") && applesNum == 12 && pd.box2Filled)
        {
            ResetApples();
            appleBox3.SetActive(true);
            pd.box3Filled = true;
            gameManager.tutorialManager.DisplayTutorialText(tutText, gameManager.tutorialText.autoNext[tutText]);
        }

        if (other.CompareTag("AppleStand") && applesNum == 12 && pd.box1Filled)
        {
            ResetApples();
            appleBox2.SetActive(true);
            pd.box2Filled = true;
        }

        if (other.CompareTag("AppleStand") && applesNum == 12 && !pd.box1Filled)
        {
            ResetApples();
            appleBox1.SetActive(true);
            pd.box1Filled = true;
        }
    }

    public void ResetApples()
    {
        appleInventory.Clear();
        for (int i = 0; i < applesObjectList.Length; i++)
        {
            applesObjectList[i].SetActive(false);
        }
        canPickApples = true;
        applesNum = 0;
    }
    //clear inventory on close
    private void OnApplicationQuit()
    {
        appleInventory.Clear();
    }
 
}
