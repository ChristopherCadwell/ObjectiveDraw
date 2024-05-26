using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeObject : MonoBehaviour
{
    //checks if swinging
    public bool isSwinging = false;
    public int hits;
    private GameManager gameManager;
    private bool firstBreak;
    [SerializeField] private int tutText = 13;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    //sets is swinging
    public void Update()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.Instance.player.GetComponent<PlayerMovement>().objectCarrying == gameObject)
        {
            isSwinging = true;
            hits++;
        }

        if (isSwinging)
        {
            int layerMask = 1 << 8;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 3, layerMask))
            {
                isSwinging = true;
                if (hit.collider.gameObject.tag == "BigLog")
                {
                    
                    if (hits == hit.collider.gameObject.GetComponent<BigLogScript>().hitsTillBroken)
                    {
                        hit.collider.gameObject.GetComponent<BigLogScript>().Break();
                        hits = 0;
                    }
                }
            }
            else
            {
                isSwinging = false;
            }

            if (Physics.Raycast(ray, out hit, 3, layerMask))
            {
                isSwinging = true;
                if (hit.collider.gameObject.tag == "MediumLog")
                {
                    
                    if (hits == hit.collider.gameObject.GetComponent<MediumLogScript>().hitsTillBroken)
                    {
                        hit.collider.gameObject.GetComponent<MediumLogScript>().Break();
                        hits = 0;
                    }
                }
                
            }
            else
            {
                isSwinging = false;
            }

           

            if (Physics.Raycast(ray, out hit, 3, layerMask))
            {
                isSwinging = true;
                if (hit.collider.gameObject.tag == "BoardLog")
                {
                    isSwinging = true;
                    if (hits == hit.collider.gameObject.GetComponent<BoardLogPuzzle>().hitsTillBroken)
                    {
                        hit.collider.gameObject.GetComponent<BoardLogPuzzle>().Break();
                        hits = 0;
                    }
                }
            }
            else
            {
                isSwinging = false;
            }

            if (Physics.Raycast(ray, out hit, 3, layerMask))
            {
                isSwinging = true;
                if (hit.collider.gameObject.tag == "ChoppingLog")
                {   
                    if (hits == hit.collider.gameObject.GetComponent<ChoppingBlockPuzzle>().hitsTillBroken)
                    {
                        hit.collider.gameObject.GetComponent<ChoppingBlockPuzzle>().Break();
                        hits = 0;
                    }

                    if (!firstBreak)
                    {
                        gameManager.tutorialManager.DisplayTutorialText(tutText, gameManager.tutorialText.autoNext[tutText]);
                        firstBreak = true;
                    }

                    
                }
            }
            else
            {
                isSwinging = false;
            }

            for (int i = 0; i < GameManager.Instance.wood.Count; i++)
            {
                if (GameManager.Instance.wood[i] == null) continue;
                GameManager.Instance.wood[i].GetComponent<Rigidbody>().isKinematic = false;
            }
        }
        else if (!isSwinging)
        {
            hits = 0;
        }
    }
}