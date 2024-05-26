using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;

    private void OnTriggerEnter(Collider other)
    {
        if ((playerLayer.value & (1 << other.transform.gameObject.layer)) > 0)
            EndLevelNextLevel();
    }

    private void EndLevelNextLevel()
    {
        SceneManager.LoadSceneAsync((int)SceneIndexes.LEVEL_2);
    }
}