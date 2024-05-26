using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum UIState
{
    Draw,
    Options,
    Inventory,
    Blueprint,
    AppleQuest,
    Pause,
    Game
}
public class UIData : Settings
{
    [SerializeField] protected List<GameObject> UIPaneles = new List<GameObject>();
    [SerializeField] protected List<GameObject> controlTypes = new List<GameObject>();
    protected bool isPaused = false,
        isInInventory = false;
    
    public enum States
    {
        Draw,
        Erase
    }
    public States drawStates = States.Draw;

    [Header("Abilities")]
    [SerializeField] protected Image notebookCooldown;
    [SerializeField] protected float cooldown;
    [SerializeField] protected GameObject paternRecognizer;
    public Text recentlyDrawnObjectText;
    public Text newRecentlyDrawnObjectText;
    [HideInInspector] public bool isCooldown = false;
    protected DontDestroy dontDestroy;
    public Canvas blueprints,
        overlay,
        infoBox1,
        infoBox2,
        infoBox3;

    public Image overlayImage;
}