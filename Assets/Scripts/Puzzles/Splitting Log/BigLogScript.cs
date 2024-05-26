using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigLogScript: MonoBehaviour
{
    public int hitsTillBroken;

    [SerializeField] private GameObject mediumLogObject1;
    [SerializeField] private GameObject mediumLogObject2;
    private GameObject mediumLogSpawn1;
    private GameObject mediumLogSpawn2;

    [SerializeField] private int tutText;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        mediumLogSpawn1 = gameObject.transform.GetChild(0).gameObject;
        mediumLogSpawn2 = gameObject.transform.GetChild(1).gameObject;
        gameManager = GameManager.Instance;
    }

    public void Break()
    {
        if (!gameManager.firstLogBreak)
        {
            gameManager.tutorialManager.DisplayTutorialText(tutText, gameManager.tutorialText.autoNext[tutText]);
            gameManager.firstLogBreak = true;
        }
        // Hides Big Log
        gameObject.SetActive(false);
        // Spawn new logs
        Instantiate(mediumLogObject1, mediumLogSpawn1.transform.position, gameObject.transform.rotation);
        Instantiate(mediumLogObject2, mediumLogSpawn2.transform.position, gameObject.transform.rotation);
        // deletes big log
        Destroy(gameObject);
    }
    

}
