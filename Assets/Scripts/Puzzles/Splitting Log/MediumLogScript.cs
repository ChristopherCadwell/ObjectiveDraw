using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumLogScript : MonoBehaviour
{
    public int hitsTillBroken;

    public GameObject smallLogObject1;
    public GameObject smallLogObject2;
    public GameObject smallLogSpawn1;
    public GameObject smallLogSpawn2;
    // Start is called before the first frame update
    void Start()
    {
        smallLogSpawn1 = gameObject.transform.GetChild(0).gameObject;
        smallLogSpawn2 = gameObject.transform.GetChild(1).gameObject;
    }
    public void Break()
    {
        // Hides Big Log
        gameObject.SetActive(false);
        // Spawn new logs
        Instantiate(smallLogObject1, smallLogSpawn1.transform.position, gameObject.transform.rotation);
        Instantiate(smallLogObject2, smallLogSpawn2.transform.position, gameObject.transform.rotation);
        // deletes big log
        Destroy(gameObject);
    }
}
