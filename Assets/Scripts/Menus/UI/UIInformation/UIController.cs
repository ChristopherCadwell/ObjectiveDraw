using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : UIData
{
    private void Start()
    {
        cooldown = gameManager.timeBetweenDrawings;
        SwitchUIPanel("Game");
        ChangeControl("Gameplay");
        recentlyDrawnObjectText.gameObject.SetActive(false);
        newRecentlyDrawnObjectText.gameObject.SetActive(false);
        dontDestroy = GetComponentInParent<DontDestroy>();
    }

    private void Update()
    {
        #region Input Handling
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            switch (gameManager.uiState)
            {
                case UIState.Draw:
                    gameManager.SwitchToGame(true);
                    blueprints.gameObject.SetActive(false);
                    overlay.gameObject.SetActive(false);
                    SwitchUIPanel("Game");
                    break;

                case UIState.Inventory:
                    InventoryToggle();
                    break;

                case UIState.Pause:
                    PauseUnpause();
                    break;

                case UIState.Game:
                    PauseUnpause();
                    break;

                default:
                    break;
            }
        }
        ImageCooldown();
        if (Input.GetKeyDown(KeyCode.I)) InventoryToggle();
        if (Input.GetMouseButton(0) && drawStates == States.Erase)
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawLine(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 100));

            RaycastHit2D hit2D = Physics2D.GetRayIntersection(ray);

            if (hit2D.collider != null)
            {
                gameManager.linesDrawn.Remove(hit2D.collider.gameObject);
                //TODO: make this properly remove the points with the strokeId that matches the erased line
                /*for (int i = 0; i < gameManager.points.Count; i++)
                {
                    if (gameManager.points[i].StrokeID == hit2D.collider.gameObject.GetComponent<Line>().strokeId) 
                    { 
                        gameManager.points.RemoveAt(i);
                        Debug.Log("remove point " + i);
                    }
                }*/
                Destroy(hit2D.collider.gameObject);
            }
        }
        #endregion
    }

    #region Pause Menu
    public void PauseUnpause()
    {
        if (gameManager.uiState != UIState.Pause)
        {
            Time.timeScale = 0;
            SwitchUIPanel("Pause");
            gameManager.uiState = UIState.Pause;

            //allows the player to move
            gameManager.playerLook.canMove = false;
            gameManager.playerMove.canMove = false;

            //locks the mouse
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (gameManager.uiState == UIState.Pause)
        {
            Time.timeScale = 1;
            SwitchUIPanel("Game");
            gameManager.uiState = UIState.Game;

            //allows the player to move
            gameManager.playerLook.canMove = true;
            gameManager.playerMove.canMove = true;

            //locks the mouse
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;


        }
    }
    //temp restart
    public void RestartLevel()
    {
        Destroy(gameManager.gameObject);
        Destroy(transform.parent.gameObject);
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync((int)SceneIndexes.TUTORIAL_LEVEL);
    }

    //quits the application
    public void Quit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//stop editor
#endif
    }
    #endregion

    #region Inventory Control
    public void InventoryToggle()
    {

        if (gameManager.uiState != UIState.Inventory)
        {
            Time.timeScale = 0;
            gameManager.uiState = UIState.Inventory;

            //allows the player to move
            gameManager.playerLook.canMove = false;
            gameManager.playerMove.canMove = false;

            //locks the mouse
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (gameManager.playerMove.isCarryingBasket)
            {
                //show apple basket
                SwitchUIPanel("AppleBasket");
                infoBox3.gameObject.SetActive(false);
                return;//prevent further execution
            }
            SwitchUIPanel("Inventory");
            infoBox1.gameObject.SetActive(false);
            
        }
        else if (gameManager.uiState == UIState.Inventory)
        {
            Time.timeScale = 1;
            SwitchUIPanel("Game");
            gameManager.uiState = UIState.Game;

            //allows the player to move
            gameManager.playerLook.canMove = true;
            gameManager.playerMove.canMove = true;

            //locks the mouse
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }
    public void OverlaySelection()
    {
        //overlayImage = gameManager.playerMove.blueprintInventory.FindItemInInventory(1);
    }
    #endregion

    #region Drawing States
    public void Drawing(string state)
    {
        drawStates = (States)Enum.Parse(typeof(States), state);
    }

    public void FinishDrawing(bool cancel)
    {
        gameManager.SwitchToGame(cancel);
        gameManager.drawingUI.GetComponentInChildren<DrawToolsController>().onDraw?.Invoke();
        SwitchUIPanel("Game");
        switch (gameManager.patternRecognizer.GetComponent<HandleRecognition>().checkIfNew())
        {
            case true:
                newRecentlyDrawnObjectText.gameObject.SetActive(true);
                newRecentlyDrawnObjectText.text = gameManager.patternRecognizer.GetComponent<HandleRecognition>().recentlyDrawnGesture;
                break;
            case false:
                recentlyDrawnObjectText.gameObject.SetActive(true);
                recentlyDrawnObjectText.text = gameManager.patternRecognizer.GetComponent<HandleRecognition>().recentlyDrawnGesture;
                break;
        }
    }

    private void ImageCooldown()
    {
        //if(Input.GetKey(KeyCode.U) && !isCooldown)
        //{
        //    Debug.Log("Potato");
        //    isCooldown = true;
        //    notebookCooldown.fillAmount = 1;
        //}

        if (isCooldown)
        {
            if (notebookCooldown.fillAmount >= 1) notebookCooldown.fillAmount = 0;

            notebookCooldown.fillAmount += 1 / cooldown * Time.deltaTime;

            if (notebookCooldown.fillAmount >= 1)
            {
                notebookCooldown.fillAmount = 1;
                isCooldown = false;
                recentlyDrawnObjectText.gameObject.SetActive(false);
                newRecentlyDrawnObjectText.gameObject.SetActive(false);
            }
        }
    }

    public void BluePrintInventory()
    {
        blueprints.gameObject.SetActive(true);
        infoBox2.gameObject.SetActive(false);
    }
    public void BluePrintClose()
    {
        blueprints.gameObject.SetActive(false);
    }
    public void BluePrintOverlay()
    {
        BluePrintClose();
        overlay.gameObject.SetActive(true);
    }
    public void CloseOverLay()
    {
        overlay.gameObject.SetActive(false);
    }
    #endregion

    #region Misc Functions
    public void SwitchUIPanel(string toSwitch)
    {
        for (int i = 0; i < UIPaneles.Count; i++)
            UIPaneles[i].SetActive(UIPaneles[i].name.Contains(toSwitch));
    }

    //changes the scene and loads up the menu
    public void GoToMenu()
    {
        Destroy(gameManager.gameObject);
        Destroy(transform.parent.gameObject);
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync((int)SceneIndexes.MAIN_MENU);
    }

    public void ChangeControl(string controlName)
    {
        for (int i = 0; i < controlTypes.Count; i++)
            controlTypes[i].SetActive(controlTypes[i].name.Contains(controlName));
    }

    #endregion
}