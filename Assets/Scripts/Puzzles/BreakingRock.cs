using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingRock : MonoBehaviour
{
    public int hitsTillBroken;

    [SerializeField] private bool isFlintRock;

    [SerializeField] private GameObject flintObject;
    [SerializeField] private GameObject flintSpawn;

    // Start is called before the first frame update
    void Start()
    {
        flintSpawn = gameObject.transform.GetChild(0).gameObject;
    }

    public void Break()
    {
        // Hides Big Log
        gameObject.SetActive(false);
        // Spawn new logs
        if (isFlintRock)
        {
            Instantiate(flintObject, flintSpawn.transform.position, gameObject.transform.rotation);
        }
        // deletes big log
        Destroy(gameObject);
    }
}
