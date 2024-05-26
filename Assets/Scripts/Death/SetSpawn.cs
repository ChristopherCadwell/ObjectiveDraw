using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSpawn : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.ChangeSpawn(gameObject);
    }
}