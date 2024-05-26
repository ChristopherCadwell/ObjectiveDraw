using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

//connects it to the tutorialcall script
[CustomEditor(typeof(TutorialCall)), CanEditMultipleObjects]

//sets it to editor so it changes the editor and runs constantly
public class TutorialTextDisplay : Editor
{
    //creates custom gui
    public override void OnInspectorGUI()
    {
        //runs the base gui creation
        base.OnInspectorGUI();

        //sets target variable
        TutorialCall tutorialCall = (TutorialCall)target;

        //gets teh tutorial manager if it is null; i did this so it would not have to be set everytime; in other script its executed at runtime
        //if check so it doesnt run all the time
        if(tutorialCall.tutorialText == null)
        {
            tutorialCall.tutorialText = GameObject.FindWithTag("TutorialManager").GetComponent<TutorialText>();
        }

        //creates texture2d for background of label
        tutorialCall.backImage = new Texture2D(128, 128);

        //sets the color of the background pixel by pixel
        for (int y = 0; y < tutorialCall.backImage.height; y++)
        {
            for (int x = 0; x < tutorialCall.backImage.width; x++)
            {
                Color color = Color.grey;
                tutorialCall.backImage.SetPixel(x, y, color);
            }
        }
        //applies the changes to the image
        tutorialCall.backImage.Apply();

        //creates a new style and sets the different variables of that new style
        GUIStyle gUIStyle = new GUIStyle();
        gUIStyle.normal.textColor = Color.black;
        gUIStyle.normal.background = tutorialCall.backImage;
        gUIStyle.alignment = (TextAnchor)TextAlignment.Center;
        gUIStyle.alignment = TextAnchor.MiddleCenter;

        //creates a label in the UI, sets the text to the specific text in the list in the tutorialmanager depending on the number inputed in the inspector, sets the guistyle to the one created
        GUILayout.Label(tutorialCall.tutorialText.tutorialTextLines[tutorialCall.tutorialTextCall], gUIStyle);
    }
}
