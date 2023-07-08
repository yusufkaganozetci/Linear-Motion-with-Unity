using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CoordinateSystem : MonoBehaviour
{
    [SerializeField] float minCoordinatePositionForXAxis = -95;
    [SerializeField] float maxCoordinatePositionForXAxis = 95;
    [SerializeField] float minCoordinatePositionForYAxis = -95;
    [SerializeField] float maxCoordinatePositionForYAxis = 95;


    [SerializeField] LineRenderer lr;

    [SerializeField] GameObject[] preGeneratedPoints;
    [SerializeField] GameObject[] preGeneratedxCoordinateObjects;
    [SerializeField] GameObject[] preGeneratedyCoordinateObjects;

    [SerializeField] Transform parentObjectForPoints;
    [SerializeField] Transform parentObjectForXCoordinatePointObject;
    [SerializeField] Transform parentObjectForYCoordinatePointObject;

    [SerializeField] Transform xAxis;
    [SerializeField] Transform yAxis;

    List<Point> allPoints;

    private int predefinedObjectCount;
    private List<int> pointIndexesThatHasImages;

    string pointAssignType = "xValue";//xValue,yValue,indexNumber

    //position values
    float maxXValue, maxYValue, xFactor, yFactor; // floattý bunlar

    private int pointCount = 5;
    private bool isPointCountNewlyChanged = false;

    private float xPositionForYAxis, yPositionForXAxis;

    private float xDeflection, yDeflection;

    private void Start()
    {
        allPoints = new List<Point>();
        pointIndexesThatHasImages = new List<int>();
        predefinedObjectCount = preGeneratedPoints.Length;
        //xPositionForYAxis = 

        xDeflection = parentObjectForXCoordinatePointObject.gameObject.GetComponent<RectTransform>().anchoredPosition.x;
        yDeflection = parentObjectForYCoordinatePointObject.gameObject.GetComponent<RectTransform>().anchoredPosition.y;
        
        gameObject.SetActive(false);



        /*Point p = new Point(0, 0);
        allPoints.Add(p);
        CalculateXAndYFactorsForPositoning();
        AssignImages();
        CalculatePositions();
        ResetImages();

        Point p1 = new Point(-100, -100);
        allPoints.Add(p1);
        CalculateXAndYFactorsForPositoning();
        AssignImages();
        CalculatePositions();
        ResetImages();

        Point p2 = new Point(-0, -0);
        allPoints.Add(p2);
        CalculateXAndYFactorsForPositoning();
        AssignImages();
        CalculatePositions();
        ResetImages();*/

    }

    public void ResetValues()
    {
        lr.positionCount = 0;
        for(int i=0;i< preGeneratedPoints.Length; i++)
        {
            preGeneratedPoints[i].GetComponent<RectTransform>().anchoredPosition =
                new Vector2(5000, 5000);
            preGeneratedxCoordinateObjects[i].GetComponent<RectTransform>().anchoredPosition =
                new Vector2(5000, 5000);
            preGeneratedyCoordinateObjects[i].GetComponent<RectTransform>().anchoredPosition =
                new Vector2(5000, 5000);
        }
        allPoints.Clear();
    }

    public void ChangePointAssignType(string newType)
    {
        pointAssignType = newType;
    }

    



    private IEnumerator CreatePoints()
    {
        int startXValue = 0;
        int startYValue = 0;
        int xIncrease = 1;
        int yIncrease = 2;
        while (true)
        {
            Point p = new Point(startXValue, startYValue);
            
            allPoints.Add(p);
            CalculateXAndYFactorsForPositoning();
            AssignImages();

            CalculatePositions();

            startXValue += xIncrease + UnityEngine.Random.Range(1, 10);
            startYValue += yIncrease + UnityEngine.Random.Range(1, 10);
            yield return new WaitForSecondsRealtime(1f);
            ResetImages();
        }

    }

    private void ResetPreGeneratedPointsPositions()
    {
        for (int i = 0; i < preGeneratedPoints.Length; i++)
        {
            preGeneratedPoints[i].GetComponent<RectTransform>().anchoredPosition =
                new Vector2(5000, 5000);
            preGeneratedxCoordinateObjects[i].GetComponent<RectTransform>().anchoredPosition =
                new Vector2(5000, 5000);
            preGeneratedyCoordinateObjects[i].GetComponent<RectTransform>().anchoredPosition =
                new Vector2(5000, 5000);
        }
    }

    public void CreateNewPoint(float xValue, float yValue)
    {
        ResetPreGeneratedPointsPositions();
        Point p = new Point(xValue, yValue);
        allPoints.Add(p);
        CalculateXAndYFactorsForPositoning();
        AssignImages();
        CalculatePositions();
        ResetImages();
    }

    public void ChangePointCount(int newPointCount)
    {
        isPointCountNewlyChanged = true;
        pointCount = newPointCount;
        

    }

    private bool CheckXCoordinatePointObjectAssigned(int index)
    {
        float xValue = allPoints[index].xValue;
        bool isExist = false;
        for(int i = 0; i < allPoints.Count; i++)
        {
            if(allPoints[i].xValue == xValue && allPoints[i].xCoordinatePointObject != null)
            {
                isExist = true;
            } 
        }
        return isExist;
    }

    private bool CheckYCoordinatePointObjectAssigned(int index)
    {
        float yValue = allPoints[index].yValue;
        bool isExist = false;
        for (int i = 0; i < allPoints.Count; i++)
        {
            if (allPoints[i].yValue == yValue && allPoints[i].yCoordinatePointObject != null)
            {
                isExist = true;
            }
        }
        return isExist;
    }

    private void AssignImages()
    {
        List<int> indexes = GetIndexes(allPoints.Count, pointCount);
        
        for (int i = 0; i < indexes.Count; i++)
        {

            pointIndexesThatHasImages.Add(indexes[i]);
            allPoints[indexes[i]].pointObject = preGeneratedPoints[i];
            
            if (/*(int)*/allPoints[indexes[i]].xValue != 0 && !CheckXCoordinatePointObjectAssigned(indexes[i]))//basýnda int vardý
            {
                allPoints[indexes[i]].xCoordinatePointObject = preGeneratedxCoordinateObjects[i];
                allPoints[indexes[i]].xCoordinatePointObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (/*(int)*/allPoints[indexes[i]].xValue).ToString();
            }
            if (/*(int)*/allPoints[indexes[i]].yValue != 0 && !CheckYCoordinatePointObjectAssigned(indexes[i]))//basýnda int vardý
            {
                allPoints[indexes[i]].yCoordinatePointObject = preGeneratedyCoordinateObjects[i];
                allPoints[indexes[i]].yCoordinatePointObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (/*(int)*/allPoints[indexes[i]].yValue).ToString();

            }

        }
        indexes.Clear();



    }

    private List<int> GetIndexes(int listLength, int indexesAmount)
    {

        List<int> indexes = new List<int>();


        if (listLength <= indexesAmount)
        {
            for (int i = 0; i < listLength; i++)
            {
                indexes.Add(i);
            }
        }
        else
        {
            if (pointAssignType.Equals("indexNumber"))
            {

                /*indexes.Add(0); MIDDLE NUMBER
                int middleIndex = (int)Math.Round((double)(listLength - 1) / 2);
                Debug.Log("Middle: " + middleIndex);
                indexes.Add(middleIndex);
                int leftPos = middleIndex;
                int rightPos = middleIndex;
                int marker = 0;
                while (indexes.Count < indexesAmount - 1)
                {
                    if (marker % 2 == 0)
                    {
                        //left
                        leftPos -= (int)Math.Round((double)(listLength - 3) / indexesAmount);
                        indexes.Add(leftPos);
                    }
                    else
                    {
                        //right
                        rightPos += (int)Math.Round((double)(listLength - 3) / indexesAmount);
                        indexes.Add(rightPos);
                    }
                    marker += 1;
                }
                indexes.Add(listLength - 1);*/
                
                
                int spacing = (int)Math.Round((double)(listLength - 2) / (indexesAmount - 1));
                
                indexes.Add(0);
                int lastPos = 0;
                for (int i = 1; i < indexesAmount - 1; i++)
                {
                    int index = lastPos + spacing;
                    lastPos = index;
                    if (index < listLength - 1)
                    {
                        indexes.Add(index);
                    }
                    
                }
                indexes.Add(listLength - 1);
                /*
                int currentPos = 0;
                
                indexes.Add(0);
                
                while (indexes.Count < indexesAmount - 1)
                {
                    currentPos += (int)Math.Round((double)(listLength - 2) / (indexesAmount - 1));
                    if(currentPos >= listLength - 1)
                    {
                        break;
                    }
                    else
                    {
                        indexes.Add(currentPos);
                    }
                }
                indexes.Add(listLength - 1);
                //Debug.Log(listLength - 1);
                */

                /*indexes.Add(0);
                for (int i = 1; i < listLength - 1; i++)
                {
                    if (i % ((listLength - 2) / (indexesAmount - 2)) == 0)
                    {
                        indexes.Add(i);
                    }

                }
                indexes.Add(listLength - 1);
                Debug.Log("Count" + indexes.Count);*/
                /*int currentLeft = 1;
                int currentRight = indexes.Count - 2;
                int marker = 0;

                while (indexes.Count > indexesAmount)
                {
                    if (marker % 2 == 0)
                    {
                        indexes.RemoveAt(currentLeft);
                        currentLeft = 1;
                        currentRight = indexes.Count - 2;
                    }
                    else
                    {
                        indexes.RemoveAt(currentRight);
                        currentLeft = 1;
                        currentRight = indexes.Count - 2;
                    }
                    marker += 1;
                }
                */
            }
            else if (pointAssignType.Equals("xValue"))
            {
                indexes.Add(0);
                indexes.Add(listLength - 1);
                float index = (maxCoordinatePositionForXAxis - minCoordinatePositionForXAxis) / (indexesAmount - 1);//16,32,48
                float runTimeIndex = minCoordinatePositionForXAxis + index;
                for (int i = 0; i < indexesAmount - 2; i++)
                {
                    float chosenXPosForGreater = float.MaxValue;
                    float chosenXPosForSmaller = float.MinValue;
                    bool isGreaterChosen = false;
                    int chosenIndex = 0;
                    for (int j = 1; j < allPoints.Count - 1; j++)
                    {
                        float xPos = minCoordinatePositionForXAxis + (allPoints[j].xValue * xFactor);
                        if (xPos >= runTimeIndex && xPos < chosenXPosForGreater && !indexes.Contains(j))
                        {
                            chosenXPosForGreater = xPos;
                            isGreaterChosen = true;
                            chosenIndex = j;
                        }
                    }
                    if (isGreaterChosen)
                    {
                        indexes.Add(chosenIndex);
                    }
                    else
                    {
                        for (int j = 1; j < allPoints.Count - 1; j++)
                        {
                            float xPos = minCoordinatePositionForXAxis + allPoints[j].xValue * xFactor;
                            if (xPos <= runTimeIndex && xPos > chosenXPosForSmaller && !indexes.Contains(j))
                            {
                                chosenXPosForSmaller = xPos;
                                chosenIndex = j;
                            }
                        }
                        indexes.Add(chosenIndex);
                    }
                    runTimeIndex += index;

                }

            }

            else if (pointAssignType.Equals("yValue"))
            {
                indexes.Add(0);
                indexes.Add(listLength - 1);
                float index = (maxCoordinatePositionForYAxis - minCoordinatePositionForYAxis) / (indexesAmount - 1);//16,32,48
                float runTimeIndex = minCoordinatePositionForXAxis + index;
                for (int i = 0; i < indexesAmount - 2; i++)
                {
                    float chosenYPosForGreater = float.MaxValue;
                    float chosenYPosForSmaller = float.MinValue;
                    bool isGreaterChosen = false;
                    int chosenIndex = 0;
                    for (int j = 1; j < allPoints.Count - 1; j++)
                    {
                        float yPos = minCoordinatePositionForYAxis + (allPoints[j].yValue * yFactor);
                        if (yPos >= runTimeIndex && yPos < chosenYPosForGreater && !indexes.Contains(j))
                        {
                            chosenYPosForGreater = yPos;
                            isGreaterChosen = true;
                            chosenIndex = j;
                        }
                    }
                    if (isGreaterChosen)
                    {
                        indexes.Add(chosenIndex);
                    }
                    else
                    {
                        for (int j = 1; j < allPoints.Count - 1; j++)
                        {
                            float yPos = minCoordinatePositionForYAxis + allPoints[j].yValue * yFactor;
                            if (yPos <= runTimeIndex && yPos > chosenYPosForSmaller && !indexes.Contains(j))
                            {
                                chosenYPosForSmaller = yPos;
                                chosenIndex = j;
                            }
                        }
                        indexes.Add(chosenIndex);
                    }
                    runTimeIndex += index;

                }

            }
        }
        return indexes;
    }

    private void ResetImages()
    {
        lr.Simplify(0);
        for (int i = 0; i < allPoints.Count; i++)
        {
            if (allPoints[i].pointObject != null)
            {
                allPoints[i].DestroyPoint();
            }
        }
        
        /*for(int i=0;i< pointIndexesThatHasImages.Count; i++)
        {
            allPoints[pointIndexesThatHasImages[i]].GetRectTransformOfPoint().anchoredPosition =
                new Vector2(5000, 5000);
            allPoints[pointIndexesThatHasImages[i]].GetRectTransformOfXCoordinatePointObject().anchoredPosition =
                new Vector2(5000, 5000);
            allPoints[pointIndexesThatHasImages[i]].GetRectTransformOfYCoordinatePointObject().anchoredPosition =
                new Vector2(5000, 5000);
            allPoints[pointIndexesThatHasImages[i]].pointObject = null;
            allPoints[pointIndexesThatHasImages[i]].xCoordinatePointObject = null;
            allPoints[pointIndexesThatHasImages[i]].yCoordinatePointObject = null;
        }
        pointIndexesThatHasImages.Clear();*/
    }

    private void CalculateXAndYFactorsForPositoning()
    {
        maxXValue = allPoints[0].xValue;
        maxYValue = allPoints[0].yValue;
        for (int i = 0; i < allPoints.Count; i++)
        {
            if (Math.Abs(allPoints[i].xValue) > Mathf.Abs(maxXValue))
            {
                maxXValue = allPoints[i].xValue;
            }

            if (Math.Abs(allPoints[i].yValue) > Mathf.Abs(maxYValue))
            {
                maxYValue = allPoints[i].yValue;
            }
        }
        if (maxXValue == 0)
        {
            xFactor = 0;
        }
        else
        {
            xFactor = (maxCoordinatePositionForXAxis - minCoordinatePositionForXAxis) / maxXValue;
        }
        if (maxYValue == 0)
        {
            yFactor = 0;
        }
        else
        {
            yFactor = (maxCoordinatePositionForYAxis - minCoordinatePositionForYAxis) / maxYValue;
        }
    }

    private void CalculatePositions()
    {
        for (int i = 0; i < allPoints.Count; i++)
        {
            allPoints[i].xCoordinatePosition = minCoordinatePositionForXAxis + allPoints[i].xValue * xFactor;
            allPoints[i].yCoordinatePosition = minCoordinatePositionForYAxis + allPoints[i].yValue * yFactor;
            if (allPoints[i].pointObject != null)
            {
                allPoints[i].GetRectTransformOfPoint().anchoredPosition = new Vector2(allPoints[i].xCoordinatePosition,
                    allPoints[i].yCoordinatePosition);
            }
            if (allPoints[i].xCoordinatePointObject != null)
            {
                allPoints[i].GetRectTransformOfXCoordinatePointObject().anchoredPosition = new Vector2(allPoints[i].xCoordinatePosition - xDeflection,
                    0);
            }
            if (allPoints[i].yCoordinatePointObject != null)
            {
                allPoints[i].GetRectTransformOfYCoordinatePointObject().anchoredPosition = new Vector2(0,
                    allPoints[i].yCoordinatePosition - yDeflection);//parentObjectForYCoordinatePointObject.position.y);
            }
        }
        lr.positionCount = allPoints.Count;
        for (int i = 0; i < allPoints.Count; i++)
        {
            lr.SetPosition(i, new Vector2(allPoints[i].xCoordinatePosition, allPoints[i].yCoordinatePosition));
        }
    }

}

public class Point
{
    
    public float xValue, yValue, xCoordinatePosition = 0, yCoordinatePosition = 0;
    public GameObject pointObject = null;
    public GameObject xCoordinatePointObject = null;
    private RectTransform rectTransformOfXCoordinatePoint = null;
    public GameObject yCoordinatePointObject = null;
    private RectTransform rectTransformOfYCoordinatePoint = null;
    private RectTransform rectTransformOfPoint = null;

    public Point(float xValue, float yValue)
    {
        this.xValue = xValue;
        this.yValue = yValue;
    }
    
    public RectTransform GetRectTransformOfPoint()
    {
        if (rectTransformOfPoint == null)
        {
            rectTransformOfPoint = pointObject.GetComponent<RectTransform>();
        }
        return rectTransformOfPoint;
    }

    public RectTransform GetRectTransformOfXCoordinatePointObject()
    {
        if (rectTransformOfXCoordinatePoint == null)
        {
            rectTransformOfXCoordinatePoint = xCoordinatePointObject.GetComponent<RectTransform>();
        }
        return rectTransformOfXCoordinatePoint;
    }

    public RectTransform GetRectTransformOfYCoordinatePointObject()
    {
        if (rectTransformOfYCoordinatePoint == null)
        {
            rectTransformOfYCoordinatePoint = yCoordinatePointObject.GetComponent<RectTransform>();
        }
        return rectTransformOfYCoordinatePoint;
    }

    public void DestroyPoint()
    {
        pointObject = null;
        xCoordinatePointObject = null;
        rectTransformOfXCoordinatePoint = null;
        yCoordinatePointObject = null;
        rectTransformOfYCoordinatePoint = null;
        rectTransformOfPoint = null;

    }



}
