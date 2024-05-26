using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TutorialText : MonoBehaviour
{
    [HideInInspector] public List<string> tutorialTextLines = new List<string>();
    [HideInInspector] public List<Color> tutorialTextColor = new List<Color>();
    [HideInInspector] public List<bool> isItalicized = new List<bool>();
    [HideInInspector] public List<bool> autoNext = new List<bool>();

    [Header("Editor Font Styles")]
    public Font winter;
    public Color fontColor;
    public Color backColor;
    [HideInInspector] public Texture2D backImage;

    public void AddToList()
    {
        tutorialTextLines.Add("");
        tutorialTextColor.Add(fontColor);
        isItalicized.Add(false);
        autoNext.Add(false);
    }

    public void SubtractFromList()
    {
        tutorialTextLines.RemoveAt(tutorialTextLines.Count - 1);
        tutorialTextColor.RemoveAt(tutorialTextColor.Count - 1);
        isItalicized.RemoveAt(isItalicized.Count - 1);
        autoNext.RemoveAt(autoNext.Count - 1);
    }

#if UNITY_EDITOR
    public void ApplyChanges()
    {
        PrefabUtility.ApplyPrefabInstance(transform.root.gameObject, InteractionMode.AutomatedAction);
    }
#endif
}