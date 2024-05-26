using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsController : MonoBehaviour
{
    [SerializeField] private List<GameObject> optionMenus = new List<GameObject>();

    //sets the panel to mainmenu on loadup
    private void Start()
    {
        ChangeMenu("Graphics");
    }

    //changes the panel to things like the settings
    public void ChangeMenu(string menuName)
    {
        for (int i = 0; i < optionMenus.Count; i++)
            optionMenus[i].SetActive(optionMenus[i].name.Contains(menuName));
    }
}