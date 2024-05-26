using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PDollarGestureRecognizer;


public class GameManager : ManagerSingleton<GameManager>
{
    [Header("Prefabs and GameObjects")]
    [SerializeField] private GameObject drawingManager;
    [SerializeField] private GameObject playerPref;
    public GameObject drawingUI;
    [HideInInspector] public GameObject player;
    public UIController UIController;
    [HideInInspector] public GameObject drawingMan;
    public GameObject playerSpawn;
    [SerializeField] private Camera drawCamera;
    public GameObject patternRecognizer;

    private bool firstTimeDrawing = false;
    private bool firstTimeFinishing = false;

    [Header("Drawing Lines")]
    public List<GameObject> linesDrawn = new List<GameObject>();
    public GameObject parent;
    public Color color = Color.black;

    [Header("Drawing Boundries")]
    [SerializeField] private GameObject top;
    [SerializeField] private GameObject bottom;
    [SerializeField] private GameObject right;
    [SerializeField] private GameObject left;

    //player variables
    public LookAround playerLook;
    public PlayerMovement playerMove;
    private Camera cameraObj;
    private Vector3 camPos;
    private Quaternion camRot;
    public bool isDrawing = false; // Boolean to disable spawning a draw panel while one is already up

    public List<GameObject> wood = new List<GameObject>();

    public float timeBetweenDrawings = 5.0f;
    private float timer;

    private GameObject tutMan;
    [HideInInspector] public TutorialManager tutorialManager;

    [HideInInspector] public TutorialText tutorialText;
    public int drawTutText;
    public int finTutText;
    [HideInInspector] public bool firstLogBreak;
    [HideInInspector] public UIState uiState = UIState.Game;
    public GameObject spawnMoveUp;

    private void Start()
    {
        Spawn();
        drawingUI.GetComponent<Canvas>().worldCamera = drawCamera;
        drawingUI.GetComponent<Canvas>().planeDistance = 10;
        tutMan = GameObject.FindWithTag("TutorialManager");
        tutorialManager = tutMan.GetComponent<TutorialManager>();
        tutorialText = tutMan.GetComponent<TutorialText>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Spawn()
    {
        player = Instantiate(playerPref, playerSpawn.transform.position, Quaternion.identity);
        playerLook = player.GetComponentInChildren<LookAround>();
        playerMove = player.GetComponent<PlayerMovement>();
        cameraObj = player.GetComponentInChildren<Camera>();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            DeathSpawnChange();

        if (player == null)
            Spawn();

        if (isDrawing) return;
        
        timer -= Time.deltaTime;
        
        if (Input.GetKeyDown(KeyCode.Tab) && timer <= 0)
        {
            // check if the player is drawing, to prevent spawning multiple instances of drawing
            SwitchToDraw();
            isDrawing = true;
            timer = timeBetweenDrawings;
            if(!firstTimeDrawing)
            {
                tutorialManager.DisplayTutorialText(drawTutText, tutorialText.autoNext[drawTutText]);
                firstTimeDrawing = true;
            }
            uiState = UIState.Draw;
        }
    }

    public void SwitchToDraw()
    {
        Time.timeScale = 0;
        UIController.drawStates = UIData.States.Draw;

        ////gets the original pos and rot of the camera
        //camPos = cameraObj.gameObject.transform.position;
        //camRot = cameraObj.gameObject.transform.rotation;

        ////sets the new pos and rot of the camera
        //cameraObj.gameObject.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 25, player.transform.position.z);
        //cameraObj.gameObject.transform.rotation = Quaternion.identity;
        //cameraObj.orthographic = true;
        cameraObj.tag = "Untagged";
        drawCamera.tag = "MainCamera";
        cameraObj.enabled = false;
        drawCamera.enabled = true;

        StartNewDrawing();

        //disables player movement
        playerLook.canMove = false;
        playerMove.canMove = false;

        //releases the mouse to be able to draw
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //switches UI
        UIController.SwitchUIPanel("Draw");
        UIController.isCooldown = true;
    }

    public void StartNewDrawing()
    {
        //creates the drawing manager
        drawingMan = Instantiate(drawingManager, new Vector3(0, 0, 0), Quaternion.identity);

        //sets varaibles of the drawing manager
        drawingMan.GetComponent<DrawLine>().top = top;
        drawingMan.GetComponent<DrawLine>().bottom = bottom;
        drawingMan.GetComponent<DrawLine>().right = right;
        drawingMan.GetComponent<DrawLine>().left = left;
    }

    public void DisableOldDrawing()
    {
        Destroy(drawingMan);
    }
    public void SwitchToGame(bool cancel)
    {
        Time.timeScale = 1;

        ////resets the camera pos and rot
        //cameraObj.gameObject.transform.position = camPos;
        //cameraObj.gameObject.transform.rotation = camRot;
        //cameraObj.orthographic = false;

        cameraObj.tag = "MainCamera";
        drawCamera.tag = "Untagged";
        cameraObj.enabled = true;
        drawCamera.enabled = false;

        //destroys the drawing manager
        Destroy(drawingMan);

        //allows the player to move
        playerLook.canMove = true;
        playerMove.canMove = true;

        //locks the mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (linesDrawn.Count > 0 && !cancel)
            EndDrawing();
        else if(linesDrawn.Count <= 0 || cancel)
            CancelDrawing();
        uiState = UIState.Game;
    }

    public void EndDrawing()
    {
        if(!firstTimeFinishing)
        {
            firstTimeFinishing = true;
            tutorialManager.DisplayTutorialText(finTutText, tutorialText.autoNext[finTutText]);
        }

        //gets the object that was drawn and checks the number of lines and assigns what object it is
        var temp = Instantiate(parent, linesDrawn[0].GetComponent<Line>().lineOne[Mathf.FloorToInt(linesDrawn[0].GetComponent<Line>().lineOne.Count / 2)], Quaternion.Euler(0, 0, 0));
        temp.AddComponent<DrawnObject>();

        patternRecognizer.GetComponent<HandleRecognition>().FindTheResult();

        patternRecognizer.GetComponent<HandleRecognition>().assignClass(temp, player);

        isDrawing = false;

        //switches the line to 3D
        for (int i = 0; i < linesDrawn.Count; i++)
        {
            if (linesDrawn[i] != null)
            {
                linesDrawn[i].transform.parent = temp.transform;
                linesDrawn[i].GetComponent<Line>().SwitchTo3D();

                var collider = linesDrawn[i].AddComponent<BoxCollider>();
                collider.size += new Vector3(drawingMan.GetComponent<DrawLine>().width, drawingMan.GetComponent<DrawLine>().width, drawingMan.GetComponent<DrawLine>().width);
                
                linesDrawn[i].layer = 6;
            }
        }

        //var collider = temp.AddComponent<BoxCollider>();
        //collider.size += new Vector3(drawingMan.GetComponent<DrawLine>().width * 2, drawingMan.GetComponent<DrawLine>().width * 2, drawingMan.GetComponent<DrawLine>().width * 2);

        //var trigger = temp.AddComponent<BoxCollider>();
        //trigger.size = new Vector3(5, 5, 5);
        //trigger.isTrigger = true;

        //adds rigidbody and sets layer
        temp.AddComponent<Rigidbody>();
        temp.transform.position = player.transform.position + player.transform.forward * 5;
        temp.layer = 6;

        linesDrawn.Clear();
    }

    //cancels the drawing and deletes all of the lines drawn and resets back to the game
    public void CancelDrawing()
    {
        if (linesDrawn.Count > 0)
        {
            for (int i = 0; i < linesDrawn.Count; i++)
            {
                if (linesDrawn[i] != null)
                    Destroy(linesDrawn[i]);
            }
        }
        isDrawing = false;
        linesDrawn.Clear();
    }

    public void ChangeSpawn(GameObject newSpawn)
    {
        Debug.Log(newSpawn.transform.position);
        playerSpawn = newSpawn;
    }

    public void Death()
    {
        Destroy(player);
        Spawn();
    }

    private void DeathSpawnChange()
    {
        Destroy(player);
        ChangeSpawn(spawnMoveUp);
        Spawn();
    }
}