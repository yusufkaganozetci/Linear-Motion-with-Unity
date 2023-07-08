using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointManager : MonoBehaviour
{
    /*[SerializeField] float minCoordinatePosition = -95;
    [SerializeField] float maxCoordinatePosition = 95;
    [SerializeField] LineRenderer lr;
    [SerializeField] GameObject[] points;
    [SerializeField] GameObject[] xCoordinateObjects;
    [SerializeField] GameObject[] yCoordinateObjects;
    //[SerializeField] GameObject point;
    [SerializeField] Transform parentObjectForPoints;
    //[SerializeField] GameObject xCoordinatePointObject;
    [SerializeField] Transform parentObjectForXCoordinatePointObject;
    //[SerializeField] GameObject yCoordinatePointObject;
    [SerializeField] Transform parentObjectForYCoordinatePointObject;

    List<Point> allPoints;

    private int predefinedObjectCount;
    private List<int> pointIndexesThatHasImages;

    string pointAssignType = "xValue";//xValue,yValue,indexNumber
    //position values
    float maxXValue, maxYValue, xFactor, yFactor;

    private void Start()
    {
        allPoints = new List<Point>();
        pointIndexesThatHasImages = new List<int>();
        predefinedObjectCount = points.Length;
        StartCoroutine(CreatePoints());
        allPoints.Add(new Point(0, 0));
        CalculatePositions();
        allPoints.Add(new Point(1, 20));
        CalculatePositions();
        allPoints.Add(new Point(2, 40));
        CalculatePositions();
        allPoints.Add(new Point(3, 60));
        CalculatePositions();
        allPoints.Add(new Point(4, 80));
        CalculatePositions();
        allPoints.Add(new Point(5, 100));
        CalculatePositions();
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
            //p.pointObject = Instantiate(point,parentObjectForPoints);
            if (startXValue != 0)// x deki cizgiyi olusturur
            {
                p.xCoordinatePointObject = Instantiate(xCoordinatePointObject, parentObjectForXCoordinatePointObject);
                p.xCoordinatePointObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = startXValue.ToString();
            }
            if (startYValue != 0)//y deki cizgiyi olusturur
            {
                p.yCoordinatePointObject = Instantiate(yCoordinatePointObject, parentObjectForYCoordinatePointObject);
                p.yCoordinatePointObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = startYValue.ToString();
            }
            allPoints.Add(p);
            CalculateXAndYFactorsForPositoning();
            AssignImages();
            
            CalculatePositions();

            startXValue += xIncrease + Random.Range(1,10);
            startYValue += yIncrease + Random.Range(1, 10);
            yield return new WaitForSecondsRealtime(1f);
            ResetImages();
        }

    }

    private void AssignImages()
    {
        List<int> indexes = GetIndexes(allPoints.Count, 10);
        for (int i = 0; i < indexes.Count; i++)
        {
            
            pointIndexesThatHasImages.Add(indexes[i]);
            allPoints[indexes[i]].pointObject = points[i];
            if (allPoints[indexes[i]].xValue != 0)
            {
                allPoints[indexes[i]].xCoordinatePointObject = xCoordinateObjects[i];
                allPoints[indexes[i]].xCoordinatePointObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = allPoints[indexes[i]].xValue.ToString();
            }
            if (allPoints[indexes[i]].yValue != 0)
            {
                allPoints[indexes[i]].yCoordinatePointObject = yCoordinateObjects[i];
                allPoints[indexes[i]].yCoordinatePointObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = allPoints[indexes[i]].yValue.ToString();

            }

        }

        

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
                indexes.Add(0);
                for (int i = 1; i < listLength - 1; i++)
                {
                    if (i % ((listLength - 2) / (indexesAmount - 2)) == 0)
                    {
                        indexes.Add(i);
                    }

                }
                indexes.Add(listLength - 1);
                int currentLeft = 1;
                int currentRight = indexes.Count - 2;
                int marker = 0;

                while (indexes.Count > indexesAmount)
                {
                    if (marker % 2 == 0)
                    {
                        indexes.Remove(currentLeft);
                        currentLeft += 1;
                        currentRight = indexes.Count - 2;
                    }
                    else
                    {
                        indexes.Remove(currentRight);
                        currentRight -= 1;
                    }
                    marker += 1;
                }
            }
            else if (pointAssignType.Equals("xValue"))
            {
                indexes.Add(0);
                indexes.Add(listLength - 1);
                int index = (int)((maxCoordinatePosition - minCoordinatePosition) / (indexesAmount - 1));//16,32,48
                int runTimeIndex = (int)(minCoordinatePosition+index);
                Debug.Log("Positions!");
                for (int i=0;i< indexesAmount - 2; i++)
                {
                    Debug.Log(runTimeIndex);
                    float chosenXPosForGreater = float.MaxValue;
                    float chosenXPosForSmaller = float.MinValue;
                    bool isGreaterChosen = false;
                    int chosenIndex = 0;
                    for(int j = 1; j < allPoints.Count - 1; j++)
                    {
                        float xPos = minCoordinatePosition + (allPoints[j].xValue * xFactor);
                        if(xPos>= runTimeIndex && xPos<chosenXPosForGreater && !indexes.Contains(j))
                        {
                            chosenXPosForGreater = xPos;
                            isGreaterChosen = true;
                            chosenIndex = j;
                        }
                        //minCoordinatePosition + allPoints[i].xValue * xFactor;
                    }
                    Debug.Log("FINISHED");
                    if (isGreaterChosen)
                    {
                        indexes.Add(chosenIndex);
                    }
                    else
                    {
                        for (int j = 1; j < allPoints.Count - 1; j++)
                        {
                            float xPos = minCoordinatePosition + allPoints[j].xValue * xFactor;
                            if (xPos <= runTimeIndex && xPos > chosenXPosForSmaller && !indexes.Contains(j))
                            {
                                chosenXPosForSmaller = xPos;
                                chosenIndex = j;
                            }
                            //minCoordinatePosition + allPoints[i].xValue * xFactor;
                        }
                        indexes.Add(chosenIndex);
                    }
                    runTimeIndex += index;
                    
                    //runTimeIndex *= i + 2;
                }
                
                Debug.Log("START");
                for(int i = 0; i < indexes.Count; i++)
                {
                    Debug.Log(indexes[i]);
                }
            }
        }
        return indexes;
    }

    private void ResetImages()
    {
        for(int i = 0; i < allPoints.Count; i++)
        {
            if(allPoints[i].pointObject != null)
            {
                allPoints[i].DestroyPoint();
            }
        }
        for(int i=0;i< pointIndexesThatHasImages.Count; i++)
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
        pointIndexesThatHasImages.Clear();
    }

    private void CalculateXAndYFactorsForPositoning()
    {
        maxXValue = allPoints[0].xValue;
        maxYValue = allPoints[0].yValue;
        for (int i = 0; i < allPoints.Count; i++)
        {
            if (allPoints[i].xValue > maxXValue)
            {
                maxXValue = allPoints[i].xValue;
            }

            if (allPoints[i].yValue > maxYValue)
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
            xFactor = (maxCoordinatePosition - minCoordinatePosition) / maxXValue;
        }
        if (maxYValue == 0)
        {
            yFactor = 0;
        }
        else
        {
            yFactor = (maxCoordinatePosition - minCoordinatePosition) / maxYValue;
        }
    }

    private void CalculatePositions()
    {
        for (int i=0;i< allPoints.Count; i++)
        {
            allPoints[i].xCoordinatePosition = minCoordinatePosition + allPoints[i].xValue * xFactor;
            allPoints[i].yCoordinatePosition = minCoordinatePosition + allPoints[i].yValue * yFactor;
            if(allPoints[i].pointObject != null)
            {
                allPoints[i].GetRectTransformOfPoint().anchoredPosition = new Vector2(allPoints[i].xCoordinatePosition,
                    allPoints[i].yCoordinatePosition);
            }
            if(allPoints[i].xCoordinatePointObject != null)
            {
                allPoints[i].GetRectTransformOfXCoordinatePointObject().anchoredPosition = new Vector2(allPoints[i].xCoordinatePosition -16.5f,
                    0);
            }
            if (allPoints[i].yCoordinatePointObject != null)
            {
                allPoints[i].GetRectTransformOfYCoordinatePointObject().anchoredPosition = new Vector2(0,
                    allPoints[i].yCoordinatePosition - 16.5f);
            }
        }
        lr.positionCount = allPoints.Count;
        for(int i = 0; i < allPoints.Count; i++)
        {
            lr.SetPosition(i, new Vector2(allPoints[i].xCoordinatePosition, allPoints[i].yCoordinatePosition));
        }
    }*/

}

/*public class Point
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
        if(rectTransformOfPoint == null)
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

}*/




