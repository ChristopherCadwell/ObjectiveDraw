using System;
using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
 
using PDollarGestureRecognizer; 
 
public class HandleRecognition : MonoBehaviour
{
    [HideInInspector] public List<Point> points = new List<Point>();
    [HideInInspector] public int pointID = -1;
    [HideInInspector] public int strokeId = -1;
    [HideInInspector] public string recentlyDrawnGesture;
    [HideInInspector] public List<string> drawnGestures = new List<string>();
    private List<Gesture> gestureSet = new List<Gesture>();
    private Gesture candidate;
    Result gestureResult;

    private void Start()
    {
                TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("Gestures/");
        foreach (TextAsset gestureXml in gesturesXml)
        {
            gestureSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));
        }
    }

    public void FindTheResult()
    {
        candidate = new Gesture(points.ToArray());
        gestureResult = PointCloudRecognizer.Classify(candidate, gestureSet.ToArray());
        recentlyDrawnGesture = gestureResult.GestureClass;
        strokeId = -1;
        points.Clear();
    }

    public void addPoints(float pointx, float pointy)
    {
        points.Add(new Point(pointx, pointy, strokeId));
    }

    public void assignClass(GameObject drawnObject, GameObject player)
    {
        if (gestureResult.GestureClass == "Pickaxe")
            drawnObject.AddComponent<PickaxeObject>();
        else if (gestureResult.GestureClass == "Axe")
            drawnObject.AddComponent<AxeObject>();
        else if (gestureResult.GestureClass == "Ladder") 
        player.GetComponent<PlayerMovement>().isCarryingladder = true; 
        else
            Debug.Log("Object Unidentified");

        /*
         *moderately functional making new gestures
        This exports the list of points to an .xml file, creating a new base gesture to compare user made gestures to.
        file gets exported under the library of {0} (default i've made is "Assets/Resources/New/", with the name of {1} and a timestamp.
        does not seem to properly clear after a file has been made, though this needs more testing. Restarting the program will clear it properly.
        The .xml file should be moved to Assets/Resources/Gestures/ for use (or could just make the files directly in Assets/Resources/Gestures/)
        */
        //string fileName = String.Format("{0}/{1}-{2}.xml", "Assets/Resources/New/", "Pickaxe Gesture", DateTime.Now.ToFileTime());
        //GestureIO.WriteGesture(points.ToArray(), "Pickaxe", fileName);
    }

    public bool checkIfNew()
    {
        if (drawnGestures.Count == 0)
        {
            drawnGestures.Add(recentlyDrawnGesture);
            return true;
        }
        for (int i = 0; i < drawnGestures.Count; i++)
        {
            Debug.Log(drawnGestures.Count);
            if (drawnGestures[i] == recentlyDrawnGesture)
                return false;
        }
        Debug.Log(drawnGestures[0]);
        drawnGestures.Add(recentlyDrawnGesture);
        return true;
    }
}
