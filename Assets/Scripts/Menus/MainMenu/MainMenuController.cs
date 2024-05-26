using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : Settings
{
    //list of the menues accessable to the player
    [Header("Menus")]
    [SerializeField] protected List<GameObject> menuTypes = new List<GameObject>();
    [SerializeField] protected List<GameObject> controlTypes = new List<GameObject>();

    //sets the panel to mainmenu on loadup
    private void Start()
    {
        ChangeMenu("MainMenu");
    }

    //changes the panel to things like the settings
    public void ChangeMenu(string menuName)
    {
        for (int i = 0; i < menuTypes.Count; i++)
            menuTypes[i].SetActive(menuTypes[i].name.Contains(menuName));
    }


    public void ChangeControl(string controlName)
    {
        for (int i = 0; i < controlTypes.Count; i++)
            controlTypes[i].SetActive(controlTypes[i].name.Contains(controlName));
    }

    //changes the scene and loads up the game
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync((int)SceneIndexes.TUTORIAL_LEVEL);
    }

    //quits the application
    public void Quit()
    {
        Application.Quit();
    }
}