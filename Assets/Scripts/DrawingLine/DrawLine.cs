using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PDollarGestureRecognizer;

public class DrawLine : MonoBehaviour
{
    //the different variables needed to be passed through
    public GameObject linePrefab;
    [HideInInspector] public GameObject top;
    [HideInInspector] public GameObject bottom;
    [HideInInspector] public GameObject right;
    [HideInInspector] public GameObject left;
    public float width = 0.5f;
    private int orderInLayer = 0;

    //creates the line on click
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.Instance.UIController.drawStates == UIData.States.Draw)
        {
            CreateLine();
        }
    }

    //creates the line
    private void CreateLine()
    {
        var line = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        line.layer = 12;
        line.GetComponent<Line>().top = top;
        line.GetComponent<Line>().bottom = bottom;
        line.GetComponent<Line>().right = right;
        line.GetComponent<Line>().left = left;
        line.GetComponent<Line>().width = width;
        line.GetComponent<Line>().orderInLayer = orderInLayer;
        line.GetComponent<Line>().strokeId = GameManager.Instance.patternRecognizer.GetComponent<HandleRecognition>().strokeId;
        orderInLayer++;
    }
}