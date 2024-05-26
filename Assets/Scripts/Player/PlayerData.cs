using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    //Movement / Controllers
    [Header("Movement")]
    [SerializeField] protected float speed = 9f;
    public float gravity;
    public float jumpHeight = 1.5f;

    public Transform groundCheck;
    protected float groundDistance = 0.4f;
    public LayerMask groundMask;

    public bool isGrounded;

    public CharacterController controller;
    /*[HideInInspector]*/
    public bool canMove = true;
    protected Vector3 velocity;

    //Pick ups
    [Header("Pickups")]
    [SerializeField] protected GameObject itemHolder;
    [HideInInspector] public GameObject objectCarrying = null;

    //inventory
    [Header("Inventory")]
    public InventoryObject questsInventory,
        blueprintInventory,
        collectionsInventory;
    //ItemPickup detectedItem;//for displaying pickable object "pop up"

    protected List<bool> tutText = new List<bool>() { false, false, false };
    protected GameObject tutMan;
    [HideInInspector] public TutorialManager tutorialManager;
    [HideInInspector] public TutorialText tutorialText;

    [SerializeField] protected int dropText;
    [SerializeField] protected int dropText2;
    [SerializeField] protected int pickUpText;

    public GameObject pickUpObject;
    protected float directionY;
    public bool canPickUp = true,
        isCarryingObject = false;
    #region Quest Stuff
    public bool
        isCarryingladder = false,
        IsCarryingPlank = false,
        IsCarryingLog = false,
        isCarryingBoard = false,
        isCarryingBasket = false,
        isCarryingFlint = false,
        isCarryingStone2 = false,
        isCarryingStone3 = false,
        hasGateKey = false,
        hasKey2 = false,
        hasKey3 = false,
        box1Filled = false,
        box2Filled =false,
        box3Filled = false;

    #endregion

    void Start()
    {
        controller = GetComponent<CharacterController>();
        tutMan = GameObject.FindWithTag("TutorialManager");
        tutorialManager = tutMan.GetComponent<TutorialManager>();
        tutorialText = tutMan.GetComponent<TutorialText>();
    }
}