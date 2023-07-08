using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    private float scrollingSpeed;
    [SerializeField] RectTransform leftPart;
    [SerializeField] RectTransform rightPart;


    private float partWidth;
    private SimulationManager simulationManager;

    // Start is called before the first frame update
    void Start()
    {
        partWidth = leftPart.rect.width;
        simulationManager = FindObjectOfType<SimulationManager>();
        //backgroundWidth = background.bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (simulationManager.isSimulationPlaying)
        {
            Scroll();
        }

    }

    public void AssignSpeed(double currentVelocity)
    {
        scrollingSpeed = (float)(currentVelocity * 10);
    }


    private void Scroll()
    {
        float addXPos = scrollingSpeed * Time.deltaTime;
        leftPart.anchoredPosition -= new Vector2(addXPos, 0);
        rightPart.anchoredPosition -= new Vector2(addXPos, 0);
        Replace();
    }

    private void Replace()
    {
        if(scrollingSpeed >= 0)
        {
            PositiveVelocityReplacement();
        }
        else
        {
            NegativeVelocityReplacement();
        }

        
    }

    

    private void PositiveVelocityReplacement()
    {
        if (leftPart.anchoredPosition.x <= -partWidth)
        {
            leftPart.anchoredPosition = new Vector2(rightPart.anchoredPosition.x + partWidth,
                leftPart.anchoredPosition.y);
        }
        else if (rightPart.anchoredPosition.x <= -partWidth)
        {
            rightPart.anchoredPosition = new Vector2(leftPart.anchoredPosition.x + partWidth,
                rightPart.anchoredPosition.y);
        }
    }

    private void NegativeVelocityReplacement()
    {
        if (leftPart.anchoredPosition.x >= partWidth)
        {
            leftPart.anchoredPosition = new Vector2(rightPart.anchoredPosition.x - partWidth,
                leftPart.anchoredPosition.y);
        }
        else if (rightPart.anchoredPosition.x >= partWidth)
        {
            rightPart.anchoredPosition = new Vector2(leftPart.anchoredPosition.x - partWidth,
                rightPart.anchoredPosition.y);
        }
    }
}