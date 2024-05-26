using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : TutorialData
{
    private void Start()
    {
        tutorialTextScript = GetComponent<TutorialText>();
    }

    private void Update()
    {
        if (hidden && autoContinue)
        {
            autoContinue = false;
            tutorialTextNum++;
        }

        if (Input.GetKeyDown(KeyCode.H) && !tutorialText.gameObject.transform.parent.gameObject.activeInHierarchy && tutorialTextExists)
            DisplayTutorialText(tutorialTextNum, autoContinue);
        else if (Input.GetKeyDown(KeyCode.H) && tutorialText.gameObject.transform.parent.gameObject.activeInHierarchy && tutorialTextExists)
            HideTutorialText();
    }

    public void DisplayTutorialText(int textToCall, bool autoContinue)
    {
        hidden = false;
        tutorialTextNum = textToCall;
        this.autoContinue = autoContinue;

        if (tutorialTextScript.isItalicized[tutorialTextNum])
            tutorialText.fontStyle = FontStyles.Italic | FontStyles.Bold;
        else if (!tutorialTextScript.isItalicized[tutorialTextNum])
            tutorialText.fontStyle = FontStyles.Bold;

        tutorialText.color = tutorialTextScript.tutorialTextColor[tutorialTextNum];
        tutorialText.text = tutorialTextScript.tutorialTextLines[tutorialTextNum];

        tutorialText.gameObject.transform.parent.gameObject.SetActive(true);

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)tutorialText.gameObject.transform.parent.gameObject.transform);

        if (!this.autoContinue)
        {
            if (hideTextCoroutine != null)
                StopCoroutine(hideTextCoroutine);
            if (autoTextCoroutine != null)
                StopCoroutine(autoTextCoroutine);

            hideTextCoroutine = StartCoroutine(TimeForTutorialText());
        }
        else if (this.autoContinue)
        {
            if (hideTextCoroutine != null)
                StopCoroutine(hideTextCoroutine);
            if (autoTextCoroutine != null)
                StopCoroutine(autoTextCoroutine);

            autoTextCoroutine = StartCoroutine(TimeToContinueTutText());
        }
    }

    private IEnumerator TimeForTutorialText()
    {
        yield return new WaitForSecondsRealtime(7.5f);
        tutorialText.gameObject.transform.parent.gameObject.SetActive(false);
    }

    private IEnumerator TimeToContinueTutText()
    {
        yield return new WaitForSecondsRealtime(7.5f);
        tutorialText.gameObject.transform.parent.gameObject.SetActive(false);
        autoContinue = tutorialTextScript.autoNext[tutorialTextNum + 1];
        DisplayTutorialText(tutorialTextNum+1, autoContinue);
    }

    public void HideTutorialText()
    {
        tutorialText.gameObject.transform.parent.gameObject.SetActive(false);
        hidden = true;
        if (hideTextCoroutine != null)
            StopCoroutine(hideTextCoroutine);
        if (autoTextCoroutine != null)
            StopCoroutine(autoTextCoroutine);
    }
}