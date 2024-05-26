using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PDollarGestureRecognizer;

public class Line : MonoBehaviour
{
    [HideInInspector] public float width = 0.8f;

    public List<Vector3> lineOne;
    public List<Vector3> lineTwo;
    public List<Vector3> lineThree;
    public List<Vector3> lineFour;

    [HideInInspector] public GameObject top;
    [HideInInspector] public GameObject bottom;
    [HideInInspector] public GameObject right;
    [HideInInspector] public GameObject left;

    [HideInInspector] public List<Vector3> verticies = new List<Vector3>();
    [HideInInspector] public int[] triangles;

    [HideInInspector] public LineRenderer lineRenderer;
    private bool canDraw = true;
    private Color color;
    [HideInInspector] public int orderInLayer;

    private Mesh mesh;
    private MeshRenderer meshRenderer;
    private PolygonCollider2D polygonCollider;

    //private Vector3 lastMousePosition;

    private int PointID = -1;
    [HideInInspector] public int strokeId = 0;

    private void Start()
    {
        GetComponent<LineRenderer>().sortingOrder = orderInLayer;

        GameManager.Instance.patternRecognizer.GetComponent<HandleRecognition>().strokeId++;
        strokeId = GameManager.Instance.patternRecognizer.GetComponent<HandleRecognition>().strokeId;

        //gets the mouse pos
        var cameraPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //checks of the mouse leaves the drawing area
        if (Input.GetMouseButtonUp(0) || cameraPos.x <= left.transform.position.x || cameraPos.x >= right.transform.position.x || cameraPos.y >= top.transform.position.y || cameraPos.y <= bottom.transform.position.y)
            Destroy(gameObject);

        lineRenderer = GetComponent<LineRenderer>();

        //sets the varaibles for the line renderer
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = GameManager.Instance.color;
        lineRenderer.endColor = GameManager.Instance.color;
        color = GameManager.Instance.color;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;

        //Start line 1
        lineOne.Clear();
        Vector3 tempLineOne = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        tempLineOne.z += 10;
        lineOne.Add(tempLineOne);

        //start line 2
        lineTwo.Clear();
        Vector3 tempLineTwo = tempLineOne;
        tempLineTwo.x -= width;
        tempLineTwo.y -= width;
        lineTwo.Add(tempLineTwo);

        //start line 3
        lineThree.Clear();
        Vector3 tempLineThree = tempLineOne;
        tempLineThree.x -= width;
        tempLineThree.y += width;
        tempLineThree.z += width;
        lineThree.Add(tempLineThree);

        //start line 4
        lineFour.Clear();
        Vector3 tempLineFour = tempLineOne;
        tempLineFour.x -= width;
        tempLineFour.y -= width;
        tempLineFour.z += width;
        lineFour.Add(tempLineFour);

        //line renderer start
        Vector3 lineRendererPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lineRendererPos.z += 10;
        lineRenderer.SetPosition(0, lineRendererPos);
        lineRenderer.SetPosition(1, lineRendererPos);
    }

    void Update()
    {
        if (!canDraw) return;

        if (Input.GetMouseButton(0))
        {
            //temp line one
            Vector3 tempLineOne = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tempLineOne.z += 10;

            // temp line two
            Vector3 tempLineTwo = tempLineOne;
            tempLineTwo.x -= width;
            tempLineTwo.y -= width;

            //temp line three
            Vector3 tempLineThree = tempLineOne;
            tempLineThree.x -= width;
            tempLineThree.y += width;
            tempLineThree.z += width;

            //temp line four
            Vector3 tempLineFour = tempLineOne;
            tempLineFour.x -= width;
            tempLineFour.y -= width;
            tempLineFour.z += width;

            //line renderer
            Vector3 lineRendererPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lineRendererPos.z += 10;

            //checks distance from previous point and adds after a certain distance
            if (Vector3.Distance(tempLineOne, lineOne[lineOne.Count - 1]) > 0.01f)
                UpdateLine(tempLineOne, tempLineTwo, tempLineThree, tempLineFour, lineRendererPos);

            //invokes on draw functions
            GameManager.Instance.drawingUI.GetComponentInChildren<DrawToolsController>().onDraw?.Invoke();
        }

        //mouse positon
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //checks if the mouse goes out of the drawing area and ends the line
        if (Input.GetMouseButtonUp(0) || mousePos.x <= left.transform.position.x || mousePos.x >= right.transform.position.x || mousePos.y >= top.transform.position.y || mousePos.y <= bottom.transform.position.y)
        {
            if (lineOne.Count <= 4)
            {
                Destroy(gameObject);
                return;
            }
            ConvertToVector2D();
            GameManager.Instance.linesDrawn.Add(gameObject);
            canDraw = false;
            SortVertices();
            SetTriangles();
        }
    }

    //creates a 2d polycollider that allows for erasing
    private void ConvertToVector2D()
    {
        var collider = gameObject.AddComponent<PolygonCollider2D>();
        List<Vector2> colliderPoints = new List<Vector2>();

        //adds all the points into the collider
        for (int i = 0; i < lineOne.Count; i++)
        {
            colliderPoints.Add(lineOne[i]);
        }
        collider.points = colliderPoints.ToArray();
        polygonCollider = collider;
    }

    private void UpdateLine(Vector3 newLineOne, Vector3 newLineTwo, Vector3 newLineThree, Vector3 newLineFour, Vector3 newLinePos)
    {
        lineOne.Add(newLineOne);
        lineTwo.Add(newLineTwo);
        lineThree.Add(newLineThree);
        lineFour.Add(newLineFour);
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newLinePos);

        PointID++;
        GameManager.Instance.patternRecognizer.GetComponent<HandleRecognition>().addPoints(lineRenderer.GetPosition(PointID).x, lineRenderer.GetPosition(PointID).y);
    }

    private void SortVertices()
    {
        for (int i = 0; i < lineOne.Count; i++)
        {
            verticies.Add(lineOne[i]);
            verticies.Add(lineTwo[i]);
            verticies.Add(lineThree[i]);
            verticies.Add(lineFour[i]);
        }

        if (verticies.Count <= 5)
            Destroy(gameObject);

        while (verticies.Count % 3 != 0)
        {
            verticies.RemoveAt(verticies.Count - 1);
            verticies.RemoveAt(verticies.Count - 2);
            verticies.RemoveAt(verticies.Count - 3);
            verticies.RemoveAt(verticies.Count - 4);
        }
    }

    private void SetTriangles()
    {
        triangles = new int[verticies.Count * 9];

        for (int i = 0; i < verticies.Count / 4; i++)
        {
            if (i == 0 || i == (verticies.Count / 4) - 1)
            {
                //Front Face
                triangles[i] = i + 5;
                triangles[i + 1] = i;
                triangles[i + 2] = i + 1;
                triangles[i + 3] = i + 5;
                triangles[i + 4] = i + 4;
                triangles[i + 5] = i;

                //Top Face
                triangles[i + 6] = i;
                triangles[i + 7] = i + 4;
                triangles[i + 8] = i + 6;
                triangles[i + 9] = i;
                triangles[i + 10] = i + 6;
                triangles[i + 11] = i + 2;

                //Right Face
                triangles[i + 12] = i + 1;
                triangles[i + 13] = i;
                triangles[i + 14] = i + 2;
                triangles[i + 15] = i + 1;
                triangles[i + 16] = i + 2;
                triangles[i + 17] = i + 3;

                //Left Face
                triangles[i + 18] = i + 5;
                triangles[i + 19] = i + 7;
                triangles[i + 20] = i + 6;
                triangles[i + 21] = i + 5;
                triangles[i + 22] = i + 6;
                triangles[i + 23] = i + 4;

                //Back Face
                triangles[i + 24] = i + 2;
                triangles[i + 25] = i + 6;
                triangles[i + 26] = i + 7;
                triangles[i + 27] = i + 2;
                triangles[i + 28] = i + 7;
                triangles[i + 29] = i + 3;

                //Bottom Face
                triangles[i + 30] = i + 5;
                triangles[i + 31] = i + 3;
                triangles[i + 32] = i + 7;
                triangles[i + 33] = i + 5;
                triangles[i + 34] = i + 1;
                triangles[i + 35] = i + 3;
            }
            else if (i >= 1)
            {
                if (i >= lineOne.Count - 3) return;

                //Front Face
                triangles[i * 36] = i + 5 + (3 * i);
                triangles[i * 36 + 1] = i + (3 * i);
                triangles[i * 36 + 2] = i + 1 + (3 * i);
                triangles[i * 36 + 3] = i + 5 + (3 * i);
                triangles[i * 36 + 4] = i + 4 + (3 * i);
                triangles[i * 36 + 5] = i + (3 * i);

                //Top Face
                triangles[i * 36 + 6] = i + (3 * i);
                triangles[i * 36 + 7] = i + 4 + (3 * i);
                triangles[i * 36 + 8] = i + 6 + (3 * i);
                triangles[i * 36 + 9] = i + (3 * i);
                triangles[i * 36 + 10] = i + 6 + (3 * i);
                triangles[i * 36 + 11] = i + 2 + (3 * i);

                //Right Face
                //triangles[i * 36 + 12] = i + 1 + (3 * i);
                //triangles[i * 36 + 13] = i + (3 * i);
                //triangles[i * 36 + 14] = i + 2 + (3 * i);
                //triangles[i * 36 + 15] = i + 1 + (3 * i);
                //triangles[i * 36 + 16] = i + 2 + (3 * i);
                //triangles[i * 36 + 17] = i + 3 + (3 * i);

                //Left Face
                //triangles[i * 36 + 18] = i + 5 + (3 * i);
                //triangles[i * 36 + 19] = i + 7 + (3 * i);
                //triangles[i * 36 + 20] = i + 6 + (3 * i);
                //triangles[i * 36 + 21] = i + 5 + (3 * i);
                //triangles[i * 36 + 22] = i + 6 + (3 * i);
                //triangles[i * 36 + 23] = i + 4 + (3 * i);

                //Back Face
                triangles[i * 36 + 24] = i + 2 + (3 * i);
                triangles[i * 36 + 25] = i + 6 + (3 * i);
                triangles[i * 36 + 26] = i + 7 + (3 * i);
                triangles[i * 36 + 27] = i + 2 + (3 * i);
                triangles[i * 36 + 28] = i + 7 + (3 * i);
                triangles[i * 36 + 29] = i + 3 + (3 * i);

                //Bottom Face
                triangles[i * 36 + 30] = i + 5 + (3 * i);
                triangles[i * 36 + 31] = i + 3 + (3 * i);
                triangles[i * 36 + 32] = i + 7 + (3 * i);
                triangles[i * 36 + 33] = i + 5 + (3 * i);
                triangles[i * 36 + 34] = i + 1 + (3 * i);
                triangles[i * 36 + 35] = i + 3 + (3 * i);
            }
        }
    }

    //switching to 3D, setting the mesh, the material and the verticies and triangles, then destroyign the linerenderer
    public void SwitchTo3D()
    {
        mesh = gameObject.AddComponent<MeshFilter>().mesh;
        meshRenderer = gameObject.AddComponent<MeshRenderer>();

        meshRenderer.material = new Material(Shader.Find("Sprites/Default"));
        meshRenderer.material.color = color;

        mesh.vertices = verticies.ToArray();
        mesh.triangles = triangles;

        Destroy(GetComponent<LineRenderer>());
        DestroyImmediate(polygonCollider);
    }
}