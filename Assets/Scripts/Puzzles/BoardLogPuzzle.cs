using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardLogPuzzle : MonoBehaviour
{
    public int hitsTillBroken;

    [SerializeField] private GameObject boardObject;
    private GameObject boardSpawn;

    // Start is called before the first frame update
    void Start()
    {
        boardSpawn = gameObject.transform.GetChild(0).gameObject;
    }

    public void Break()
    {
        // Hides Big Log
        gameObject.SetActive(false);
        // Spawn new logs
        Instantiate(boardObject, boardSpawn.transform.position, boardSpawn.transform.rotation);
        // deletes big log
        Destroy(gameObject);
    }
}
