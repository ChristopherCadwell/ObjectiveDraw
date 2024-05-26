using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Settings : SettingsData
{
    private void Awake()
    {
        gameManager = GameManager.Instance;

        //populates list of resolutions
        int CurrentResolutionIndex = 0;
        resolutions = Screen.resolutions;

        resolutionQualityOptions.ClearOptions();

        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string Option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(Option);

            if (resolutions[i].Equals(Screen.currentResolution))
            {
                CurrentResolutionIndex = i;
            }
        }

        //adds to dropdown and sets default
        resolutionQualityOptions.AddOptions(options);
        resolutionQualityOptions.value = CurrentResolutionIndex;

        graphicQualityOptions.ClearOptions();
        graphicQualityOptions.AddOptions(QualitySettings.names.ToList());

        fullScreenToggle.isOn = Screen.fullScreen;
        graphicQualityOptions.value = QualitySettings.GetQualityLevel();
    }

    //sets volume
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
    }

    //sets graphics
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    //sets fullscreen
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    //sets resolution
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.width, Screen.fullScreen);
    }

    public void SetSensitivity(float sensitivity)
    {
        gameManager.playerLook.mouseSensitivity = sensitivity;
        sensitivityFeedbackTxt.text = (sensitivity / 5f).ToString("P0");
    }
}