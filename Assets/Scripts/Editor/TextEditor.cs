using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TutorialText))]
public class TextEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TutorialText tutorialText = (TutorialText)target;

        tutorialText.backImage = new Texture2D(128, 128);

        for (int y = 0; y < tutorialText.backImage.height; y++)
        {
            for (int x = 0; x < tutorialText.backImage.width; x++)
            {
                Color color = tutorialText.backColor;
                tutorialText.backImage.SetPixel(x, y, color);
            }
        }
        tutorialText.backImage.Apply();

        GUIStyle gUIStyle = new GUIStyle();
        gUIStyle.normal.textColor = tutorialText.fontColor;
        gUIStyle.normal.background = tutorialText.backImage;
        gUIStyle.font = tutorialText.winter;
        gUIStyle.fontStyle = FontStyle.Bold;
        gUIStyle.fontSize = 24;
        gUIStyle.alignment = (TextAnchor)TextAlignment.Center;
        gUIStyle.alignment = TextAnchor.MiddleCenter;
        gUIStyle.fixedWidth = 750;
        gUIStyle.fixedHeight = 75;
        gUIStyle.clipping = TextClipping.Clip;
        gUIStyle.wordWrap = false;

        GUIStyle label = new GUIStyle();
        label.fontSize = 16;
        label.normal.textColor = Color.white;
        label.alignment = (TextAnchor)TextAlignment.Center;

        GUILayout.Space(25);
        EditorGUILayout.LabelField("The Tutorial Text", label);

        for (int i = 0; i < tutorialText.tutorialTextLines.Count; i++)
        {
            GUILayout.Space(25);
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Auto Next"))
            {
                if (tutorialText.autoNext[i])
                    tutorialText.autoNext[i] = false;
                else if (!tutorialText.autoNext[i])
                    tutorialText.autoNext[i] = true;
            }

            EditorGUILayout.LabelField("Auto Next " + tutorialText.autoNext[i]);

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Tutorial Text " + (i + 1));

            if (GUILayout.Button("Bold + Italicize") && tutorialText.isItalicized[i] == false)
            {
                tutorialText.isItalicized[i] = true;
            }
            if (GUILayout.Button("Bold") && tutorialText.isItalicized[i] == true)
            {
                tutorialText.isItalicized[i] = false;
            }

            if (tutorialText.isItalicized[i])
                gUIStyle.fontStyle = FontStyle.BoldAndItalic;
            else
                gUIStyle.fontStyle = FontStyle.Bold;

            tutorialText.tutorialTextColor[i] = EditorGUILayout.ColorField(tutorialText.tutorialTextColor[i]);
            gUIStyle.normal.textColor = tutorialText.tutorialTextColor[i];

            GUILayout.EndHorizontal();

            tutorialText.tutorialTextLines[i] = EditorGUILayout.TextArea(tutorialText.tutorialTextLines[i], gUIStyle);

            GUILayout.EndVertical();
        }

        GUILayout.Space(15);

        GUILayout.BeginHorizontal();

        if(GUILayout.Button("+ (Add To List)"))
        {
            tutorialText.AddToList();
        }

        if(GUILayout.Button("- (Subtract From List)"))
        {
            tutorialText.SubtractFromList();
        }

        if (GUILayout.Button("Apply (Save Prefab)"))
        {
            tutorialText.ApplyChanges();
        }

        GUILayout.EndHorizontal();
    }
}