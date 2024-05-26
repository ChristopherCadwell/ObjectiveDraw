using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PlayerData
{
    void Update()
    {
        //Checks if the player can move or not
        if (canMove)
            MoveAndJump();

        //checks for picking up objects
        PickUps();


        //save/load testing
        if (Input.GetKeyDown(KeyCode.F1))
        {
            questsInventory.Save();
            blueprintInventory.Save();
            collectionsInventory.Save();
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            questsInventory.Load();
            blueprintInventory.Load();
            collectionsInventory.Load();
        }

    }

    private void MoveAndJump()
    {
        //isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //if (isGrounded && velocity.y < 0)
        //    velocity.y = -2f;

        //Allows for player movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(speed * Time.deltaTime * move);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        //Checks jump
        if (Input.GetButtonDown("Jump") && PlayerIsGrounded())
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        //Sprint
        if (Input.GetKeyDown(KeyCode.LeftShift))
            speed *= 1.5f;
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            speed /= 1.5f;
    }

    bool PlayerIsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    private void PickUps()
    {
        //picks up an object
        if (Input.GetKeyDown(KeyCode.E) && !isCarryingObject)
        {
            int layerMask = 1 << 6;
            //int qLayerMask = 1 << 16;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            #region InventoryItems
            if (Physics.Raycast(ray, out RaycastHit hit, 3))
            {
                ItemPickup item = hit.collider.GetComponent<ItemPickup>();
                if (item != null)
                {
                    #region Standard Quest Item Inventory Additions
                    switch (item.item.type)
                    {

                        case ItemType.Log:
                            questsInventory.AddItem(new Item(item.item), 1);
                            item.item.PickedUp();
                            //var slot = questsInventory.FindItemInInventory(item.item.data.id);
                            //slot.AddAmount(0);
                            Destroy(item.gameObject);
                            break;

                        case ItemType.Stone:
                            questsInventory.AddItem(new Item(item.item), 1);
                            item.item.PickedUp();
                            //slot = questsInventory.FindItemInInventory(item.item.data.id);
                            //slot.AddAmount(0);
                            Destroy(item.gameObject);
                            break;

                        case ItemType.Key:
                            questsInventory.AddItem(new Item(item.item), 1);
                            item.item.PickedUp();
                            Destroy(item.gameObject);
                            break;

                        case ItemType.Blueprint:
                            blueprintInventory.AddItem(new Item(item.item), 1);
                            item.item.PickedUp();
                            Destroy(item.gameObject);
                            break;

                        case ItemType.Collectable:
                            collectionsInventory.AddItem(new Item(item.item), 1);
                            item.item.PickedUp();
                            Destroy(item.gameObject);
                            break;

                        default:
                            break;
                    }
                    return;
                    #endregion
                }
            }
            #endregion
            if (Physics.Raycast(ray, out hit, 3, layerMask))
            {
                if (hit.collider.gameObject.layer == 6)
                    if (hit.collider.GetComponent<Rigidbody>() == null)
                    {
                        pickUpObject = hit.collider.transform.parent.gameObject;
                        PickUp();
                    }
                    else
                    {
                        pickUpObject = hit.collider.gameObject;
                        PickUp();
                    }
            }
        }

        //drops an object
        else if (Input.GetKeyDown(KeyCode.Q) && isCarryingObject || Input.GetKeyDown(KeyCode.Q) && isCarryingBasket)
        {
            isCarryingBasket = false;//dropped basket
            foreach (Collider col in objectCarrying.GetComponentsInChildren<Collider>())
                col.enabled = true;

            objectCarrying.GetComponent<DrawnObject>().Drop();
            isCarryingObject = false;

            if (tutText[1] && !tutText[2])
            {
                tutText[2] = true;
                tutorialManager.DisplayTutorialText(dropText2, tutorialText.autoNext[dropText2]);
            }

            if (!tutText[1])
            {
                tutText[1] = true;
                tutorialManager.DisplayTutorialText(dropText, tutorialText.autoNext[dropText]);
            }
        }
    }

    //checks the other object and changes varaibles depending on object
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9 && isCarryingladder)
        {
            GameObject ladder = other.gameObject;

            ladder.GetComponent<LadderObject>().theLadder = GameManager.Instance.player.GetComponent<PlayerMovement>().objectCarrying;
            ladder.GetComponent<LadderObject>().theLadder.transform.SetParent(null);
            foreach (Collider col in ladder.GetComponent<LadderObject>().theLadder.GetComponentsInChildren<Collider>())
            {
                col.enabled = true;
            }
            ladder.AddComponent<LadderScript>();

            isCarryingladder = false;
            isCarryingObject = false;
            Destroy(objectCarrying.GetComponent<Rigidbody>());
            objectCarrying = null;
            //canPickUp = true;
            other.gameObject.GetComponent<LadderObject>().PlaceLadder();
        }
    }

    //makes it so the player can no longer pick up an object
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            //canPickUp = false;
            pickUpObject = null;
        }
    }

    //sets the position and rotation of the object that the player picked up
    private void PickUp()
    {
        var mainLine = pickUpObject.transform;
        objectCarrying = mainLine.gameObject;
        GameManager.Instance.linesDrawn.Clear();
        mainLine.GetComponent<Rigidbody>().isKinematic = true;
        mainLine.transform.parent = itemHolder.transform;
        mainLine.gameObject.transform.SetPositionAndRotation(itemHolder.transform.position, Quaternion.Euler(0, -90, -45));
        isCarryingObject = true;

        if (!tutText[0] && tutText[1])
        {
            tutText[0] = true;
            tutorialManager.DisplayTutorialText(pickUpText, tutorialText.autoNext[pickUpText]);
        }
        
        foreach (Collider col in mainLine.gameObject.GetComponentsInChildren<Collider>())
            col.enabled = false;

        if(mainLine.gameObject.GetComponent<Collider>() != null)
            mainLine.gameObject.GetComponent<Collider>().enabled = true;

        switch(mainLine.gameObject.tag)
        {
            case "AppleBasket":
                isCarryingBasket = true;
                break;

            default:
                break;
        }
    }

    //clear inventory on close
    private void OnApplicationQuit()
    {
        questsInventory.Clear();
        collectionsInventory.Clear();
        blueprintInventory.Clear();
    }

}