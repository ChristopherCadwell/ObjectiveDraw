using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SettingsData : MonoBehaviour
{
    [Header("Drop Downs")]
    [SerializeField] protected TMPro.TMP_Dropdown graphicQualityOptions;
    [SerializeField] protected TMPro.TMP_Dropdown resolutionQualityOptions;

    [Header("Toggles")]
    [SerializeField] protected Toggle fullScreenToggle;

    [Header("Audio Mixers")]
    [SerializeField] protected AudioMixer audioMixer;

    [SerializeField] protected TextMeshProUGUI sensitivityFeedbackTxt;

    protected Resolution[] resolutions;
    protected GameManager gameManager;
}