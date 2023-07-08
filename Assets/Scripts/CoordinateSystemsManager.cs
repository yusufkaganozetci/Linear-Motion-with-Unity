using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinateSystemsManager : MonoBehaviour
{
    [SerializeField] CoordinateSystem positiveYXTGraphic;
    [SerializeField] CoordinateSystem positiveYVTGraphic;
    [SerializeField] CoordinateSystem positiveYATGraphic;
    [SerializeField] CoordinateSystem negativeYXTGraphic;
    [SerializeField] CoordinateSystem negativeYVTGraphic;
    [SerializeField] CoordinateSystem negativeYATGraphic;

    private CoordinateSystem currentXTGraphic;
    private CoordinateSystem currentVTGraphic;
    private CoordinateSystem currentATGraphic;

    private string pointAssignType;
    private int pointCount = -1;

    public void AssignCurrentCoordinateSystems(float velocity, float acceleration)
    {
        DeactivateAllGraphics();
        if (velocity > 0)
        {
            currentXTGraphic = positiveYXTGraphic;
            currentVTGraphic = positiveYVTGraphic;

        }
        else
        {
            currentXTGraphic = negativeYXTGraphic;
            currentVTGraphic = negativeYVTGraphic;
        }
        currentXTGraphic.gameObject.SetActive(true);
        currentVTGraphic.gameObject.SetActive(true);
        if (acceleration > 0)
        {
            currentATGraphic = positiveYATGraphic;
            currentATGraphic.gameObject.SetActive(true);
        }
        else if(acceleration < 0)
        {
            currentATGraphic = negativeYATGraphic;
            currentATGraphic.gameObject.SetActive(true);
        }
        AssignPointAssignTypeIfNeeded();
        AssignPointCountIfNeeded();
    }
    private void AssignPointCountIfNeeded()
    {
        if (pointCount != -1)
        {
            currentXTGraphic.ChangePointCount(pointCount);
            currentVTGraphic.ChangePointCount(pointCount);
            if (currentATGraphic != null)
            {
                currentATGraphic.ChangePointCount(pointCount);
            }
            pointCount = -1;
        }
        
    }
    private void AssignPointAssignTypeIfNeeded()
    {
        if(pointAssignType != null)
        {
            currentXTGraphic.ChangePointAssignType(pointAssignType);
            currentVTGraphic.ChangePointAssignType(pointAssignType);
            if(currentATGraphic != null)
            {
                currentATGraphic.ChangePointAssignType(pointAssignType);
            }
            pointAssignType = null;
        }
        
    }

    public void ResetCurrentCoordinateSystems()
    {
        currentXTGraphic.ResetValues();
        currentVTGraphic.ResetValues();
        if(currentATGraphic != null)
        {
            currentATGraphic.ResetValues();
        }
    }

    public void UpdateGraphics(float time, float currentVelocity, float displacement, float acceleration)
    {
        currentXTGraphic.CreateNewPoint((float) time, (float) displacement);
        currentVTGraphic.CreateNewPoint((float) time, (float) currentVelocity);
        if(acceleration != 0)
        {
            currentATGraphic.CreateNewPoint((float) time, (float) acceleration);
        }
    }

    public void UpdateXTGraphic(float time, float displacement)
    {
        currentXTGraphic.CreateNewPoint(time, displacement);
    }

    public void UpdateVTGraphic(float time, float currentVelocity)
    {
        currentVTGraphic.CreateNewPoint(time, currentVelocity);
    }

    public void UpdateATGraphic(float time, float acceleration)
    {
        currentATGraphic.CreateNewPoint(time, acceleration);
    }

    public void ChangeGraphicsPointAssignType(string pointAssignType)
    {
        try
        {
            currentXTGraphic.ChangePointAssignType(pointAssignType);
            currentVTGraphic.ChangePointAssignType(pointAssignType);
            if (currentATGraphic != null)
            {
                currentATGraphic.ChangePointAssignType(pointAssignType);
            }
        }
        catch
        {
            this.pointAssignType = pointAssignType;
        }
        
    }

    public void ChangeGraphicsPointCount(int pointCount)
    {
        try
        {
            currentXTGraphic.ChangePointCount(pointCount);
            currentVTGraphic.ChangePointCount(pointCount);
            if (currentATGraphic != null)
            {
                currentATGraphic.ChangePointCount(pointCount);
            }
        }
        catch
        {
            this.pointCount = pointCount;
        }
        
    }

    private void DeactivateAllGraphics()
    {
        positiveYXTGraphic.gameObject.SetActive(false);
        positiveYVTGraphic.gameObject.SetActive(false);
        negativeYXTGraphic.gameObject.SetActive(false);
        negativeYVTGraphic.gameObject.SetActive(false);
        if(positiveYATGraphic != null)
        {
            positiveYATGraphic.gameObject.SetActive(false);
            negativeYATGraphic.gameObject.SetActive(false);
        }
    }

    public CoordinateSystem GetCurrentXTGraphic()
    {
        return currentXTGraphic;
    }

    public CoordinateSystem GetCurrentVTGraphic()
    {
        return currentVTGraphic;
    }

    public CoordinateSystem GetCurrentATGraphic()
    {
        return currentATGraphic;
    }
    
}
