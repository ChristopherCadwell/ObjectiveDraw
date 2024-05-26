using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialCall : MonoBehaviour
{
    private GameObject tutMan;
    [HideInInspector] public TutorialManager tutorialManager;
    public TutorialText tutorialText;
    public int tutorialTextCall;
    public bool autoContinue;
    [HideInInspector] public Texture2D backImage;

    private void Start()
    {
        tutMan = GameObject.FindWithTag("TutorialManager");
        tutorialManager = tutMan.GetComponent<TutorialManager>();
        tutorialText = tutMan.GetComponent<TutorialText>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            tutorialManager.DisplayTutorialText(tutorialTextCall, tutorialText.autoNext[tutorialTextCall]);
            tutorialManager.tutorialTextExists = true;
            Destroy(gameObject);
        }
    }
}