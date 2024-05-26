using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerDrawCamera : MonoBehaviour
{
    private void LateUpdate()
    {
        if(GameManager.Instance != null)
            transform.position = GameManager.Instance.player.transform.position + new Vector3(0, 5, -5);
    }
}
