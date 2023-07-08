using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SwitchingGraphsManager : MonoBehaviour
{
    [SerializeField] RectTransform[] currentGraphics;
    [SerializeField] RectTransform[] graphicsImages;
    [SerializeField] TextMeshProUGUI graphicText;
    private string[] graphicTexts = new string[] { "X - T Graphic", "V - T Graphic", "A - T Graphic" };
    private RectTransform currentXTGraphic;
    private RectTransform currentVTGraphic;
    private RectTransform currentATGraphic;
    private int currentGraphicIndex = 0;
    private float graphicXPosition, graphicYPosition;
    private float leftImageXPosition, leftImageYPosition, rightImageXPosition, rightImageYPosition;
    // Start is called before the first frame update
    void Start()
    {
        graphicXPosition = currentGraphics[0].anchoredPosition.x;
        graphicYPosition = currentGraphics[0].anchoredPosition.y;
        leftImageXPosition = graphicsImages[1].anchoredPosition.x;
        leftImageYPosition = graphicsImages[1].anchoredPosition.y;
        rightImageXPosition = graphicsImages[2].anchoredPosition.x;
        rightImageYPosition = graphicsImages[2].anchoredPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnChangeGraphicsPressed(int index)
    {
        currentGraphics[currentGraphicIndex].anchoredPosition = new Vector2(5000, 5000);
        currentGraphicIndex = index;
        if(currentGraphicIndex >= currentGraphics.Length)
        {
            currentGraphicIndex = 0;
        }
        graphicText.text = graphicTexts[currentGraphicIndex];
        HandleGraphicButtons(currentGraphicIndex);
        currentGraphics[currentGraphicIndex].anchoredPosition = new Vector2(graphicXPosition, graphicYPosition);
        
    }

    private void HandleGraphicButtons(int shownIndex)
    {
        graphicsImages[shownIndex].anchoredPosition = new Vector2(5000, 5000);
        bool isLeftImageAssigned = false;
        for(int i = 0; i < graphicsImages.Length; i++)
        {
            if (i != shownIndex)
            {
                if (!isLeftImageAssigned)
                {
                    graphicsImages[i].anchoredPosition = new Vector2(leftImageXPosition, leftImageYPosition);
                    isLeftImageAssigned = true;
                }
                else
                {
                    graphicsImages[i].anchoredPosition = new Vector2(rightImageXPosition, rightImageYPosition);
                }
            }
        }
    }

    public void OnChangeGraphicsToLeftSidePressed()
    {
        currentGraphicIndex += 1;
    }
}
