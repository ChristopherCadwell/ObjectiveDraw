using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialData : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI tutorialText;
    protected TutorialText tutorialTextScript;

    protected int tutorialTextNum = 0;

    protected bool autoContinue;
    protected bool hidden;
    [HideInInspector] public bool tutorialTextExists;

    protected Coroutine hideTextCoroutine = null;
    protected Coroutine autoTextCoroutine = null;
}