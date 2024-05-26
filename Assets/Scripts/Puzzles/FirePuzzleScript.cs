using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePuzzleScript : MonoBehaviour
{
    private PlayerData pd;

    public GameObject firePS;

    // Start is called before the first frame update
    void Start()
    {
        pd = GameManager.Instance.player.GetComponent<PlayerData>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && pd.isCarryingFlint)
        {
            int layerMask = 1 << 13;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 2, layerMask))
            {
                if (hit.collider.gameObject.CompareTag("Fire"))
                {
                    firePS.SetActive(true);
                }
            }
        }
    }
}
