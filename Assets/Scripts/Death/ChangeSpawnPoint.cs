using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSpawnPoint : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;

    private void OnTriggerEnter(Collider other)
    {
        if ((playerLayer.value & (1 << other.transform.gameObject.layer)) > 0)
            GameManager.Instance.ChangeSpawn(gameObject);
    }
}