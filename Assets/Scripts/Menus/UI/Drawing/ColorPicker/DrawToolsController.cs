using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class DrawToolsController : MonoBehaviour
{
    [SerializeField] private List<GameObject> moreTools = new List<GameObject>();
    [SerializeField] private GameObject colorPreview;
    [SerializeField] private TextMeshProUGUI placeholderInputText;
    [SerializeField] private TMP_InputField inputBox;
    [SerializeField] private Slider slidWidth;

    [HideInInspector] public Color previousColor;
    public UnityEvent onDraw;

    private GameManager gameManager;

    //public Texture2D cursorTexture;
    //public CursorMode cursorMode = CursorMode.Auto;

    private void Start()
    {
        gameManager = GameManager.Instance;
        SetColorPreview();
        OpenMoreTools("NoTools");
        ChangeWidth(gameManager.drawingMan.GetComponent<DrawLine>().width);
        slidWidth.value = gameManager.drawingMan.GetComponent<DrawLine>().width;

        //Vector2 cursorOffset = new Vector2(cursorTexture.width/2, cursorTexture.height/2);
        //Cursor.SetCursor(cursorTexture, cursorOffset, cursorMode);
    }

    private void OnEnable()
    {
        if (gameManager != null)
            ChangeWidth(slidWidth.value);
    }

    public void SetColorPreview()
    {
        colorPreview.GetComponent<Image>().color = gameManager.color;
        previousColor = colorPreview.GetComponent<Image>().color;
    }

    public void SetPreviousColor()
    {
        previousColor = colorPreview.GetComponent<Image>().color;
    }

    public void OpenMoreTools(string toolName)
    {
        for (int i = 0; i < moreTools.Count; i++)
        {
            if (moreTools[i].name.Contains(toolName))
            {
                if (moreTools[i].gameObject.activeInHierarchy)
                {
                    colorPreview.GetComponent<Image>().color = previousColor;
                    moreTools[i].SetActive(false);
                }
                else
                    moreTools[i].SetActive(moreTools[i].name.Contains(toolName));
            }
            else
                moreTools[i].SetActive(moreTools[i].name.Contains(toolName));
        }
    }

    public void Rotate90Degrees()
    {

    }

    public void NewObject()
    {
        gameManager.DisableOldDrawing();
        gameManager.StartNewDrawing();
    }

    public void ChangeWidth(float width)
    {
        gameManager.drawingMan.GetComponent<DrawLine>().width = width;
        placeholderInputText.text = width.ToString("#0.00");
    }

    public void StringToFloatInputWidth(string width)
    {
        if (width.Length == 0) return;

        float widthFloat = float.Parse(width);
        slidWidth.value = widthFloat;
        inputBox.text = "";

        if (widthFloat > 1) widthFloat = 1;
        else if (widthFloat <= 0.01f) widthFloat = 0.01f;
        ChangeWidth(widthFloat);
    }
}