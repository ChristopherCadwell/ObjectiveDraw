using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderScript : MonoBehaviour
{

    public GameObject player;
    public bool canClimb;
    public float speed = 2;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.player;
    }

    private void Update()
    {
        if (canClimb)
        {
            if (Input.GetKey(KeyCode.W))
            {
                player.transform.Translate(speed * Time.deltaTime * new Vector3(0, 1, 0));
            }
            if (Input.GetKey(KeyCode.S))
            {
                player.transform.Translate(speed * Time.deltaTime * new Vector3(0, -1, 0));
            }
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if(other.CompareTag(("Player")))
        {
            canClimb = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(("Player")))
        {
            canClimb = false;
        }
    }
}
