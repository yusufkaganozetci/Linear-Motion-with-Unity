using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class CarMovement : MonoBehaviour
{
    [SerializeField] CoordinateSystemsManager coordinateSystemsManager;
    [SerializeField] RectTransform leftWheel;
    [SerializeField] RectTransform rightWheel;

    [SerializeField] TextMeshProUGUI displacementText;
    [SerializeField] TextMeshProUGUI velocityText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI accelerationText;

    [SerializeField] TMP_InputField velocityInput;
    [SerializeField] TMP_InputField accelerationInput;
    [SerializeField] Image stopContinueImage;
    [SerializeField] Sprite stopImage;
    [SerializeField] Sprite continueImage;
    private bool isCarMoving = true;
    private SimulationManager simulationManager;
    private BackgroundScroller backgroundScroller;
    //private IEnumerator moveTheCarCoroutine;
    private Coroutine moveTheCarCoroutine;
    float displacement = 0;
    public float startVelocity = 0;
    public float currentVelocity = 0;
    private float exactCurrentVelocity = 0;// this is used for showing graphs
    private float exactDisplacement = 0;
    float acceleration = 0;
    float time = 0;
    float timeAsExact = 0;
    float updateInterval = 1f;
    float lastTime = 0;
    bool isUpdateIntervalChanged = false;

    bool isVelocityDifferentThanZero = true;

    private bool isXTGraphFinished = false;
    private bool isVTGraphFinished = false;
    // Start is called before the first frame update
    void Start()
    {
        simulationManager = FindObjectOfType<SimulationManager>();
        backgroundScroller = FindObjectOfType<BackgroundScroller>();
        //moveTheCarCoroutine = MoveTheCar();
    }

    private void ManageInputs()
    {
        float.TryParse(velocityInput.text, out float tempVelocity);
        if(accelerationInput != null)
        {
            float.TryParse(accelerationInput.text, out float acceleration);
        }
    }

    public void StartTheCarMovement()
    {
        simulationManager.isSimulationStartedOnce = true;
        //ManageInputs();
        float tempAcceleration = 0;
        float.TryParse(velocityInput.text, out float tempVelocity);
        if(accelerationInput != null)
        {
            tempAcceleration = float.Parse(accelerationInput.text);
        }
        
        coordinateSystemsManager.AssignCurrentCoordinateSystems(tempVelocity, tempAcceleration);
        ResetEverything();
        startVelocity = tempVelocity;
        currentVelocity = startVelocity;
        exactCurrentVelocity = currentVelocity;
        exactDisplacement = displacement;
        acceleration = tempAcceleration;
        moveTheCarCoroutine = StartCoroutine(MoveTheCar());


    }

    public void StopTheSimulation()
    {
        simulationManager.isSimulationPlaying = false;
    }

    public void ContinueToTheSimulation()
    {
        if (!simulationManager.isSimulationAlreadyStopped)
        {
            simulationManager.isSimulationPlaying = true;
        }
        
    }

    public void StopTheCarMovement()
    {
        if (simulationManager.isSimulationStartedOnce && simulationManager.isSimulationPlaying)
        {
            simulationManager.isSimulationPlaying = false;
            simulationManager.isSimulationAlreadyStopped = true;
            stopContinueImage.sprite = continueImage;
        }
        else
        {
            simulationManager.isSimulationPlaying = true;
            simulationManager.isSimulationAlreadyStopped = false;
            stopContinueImage.sprite = stopImage;
        }
        
    }

    public void ChangePointAssignType(int comingValue)
    {
        string[] pointAssignTypes = new string[] { "xValue", "yValue", "indexNumber" };
        if (simulationManager.isSimulationStartedOnce)
        {
            coordinateSystemsManager.ChangeGraphicsPointAssignType(pointAssignTypes[comingValue]);
        }
    }

    public void ChangePointCount(int pointCount)
    {
        coordinateSystemsManager.ChangeGraphicsPointCount(pointCount + 2);
    }

    public void UpdateIntervalInSeconds(string comingUpdateInterval)
    {
        try
        {
            float updateIntervalAsSeconds = float.Parse(comingUpdateInterval);
            updateInterval = updateIntervalAsSeconds;
            isUpdateIntervalChanged = true;
        }
        catch
        {
            //hata popuný çýkar!
        }
    }

    private void ResetEverything()
    {
        if(moveTheCarCoroutine != null)
        {
            StopCoroutine(moveTheCarCoroutine);
        }
        isXTGraphFinished = false;
        isVTGraphFinished = false;
        backgroundScroller.AssignSpeed(0);
        displacement = 0;
        exactDisplacement = 0;
        startVelocity = 0;
        currentVelocity = 0;
        exactCurrentVelocity = 0;
        acceleration = 0;
        time = 0;
        timeAsExact = 0;
        displacementText.text = "0";
        velocityText.text = "0";
        timeText.text = "0";
        
        coordinateSystemsManager.ResetCurrentCoordinateSystems();
        
    }

    private IEnumerator MoveTheCar()
    {
        simulationManager.isSimulationPlaying = true;
        UpdateGraphics();
        backgroundScroller.AssignSpeed(currentVelocity);
        float timeInSequence = 0;
        int digitCountAfterComa = GetDigitCountAfterComma(updateInterval);
        while (true)
        {
            if (isUpdateIntervalChanged)
            {
                digitCountAfterComa = GetDigitCountAfterComma(updateInterval);
                isUpdateIntervalChanged = false;
            }
            if (simulationManager.isSimulationPlaying)
            {
                if (timeInSequence >= updateInterval)
                {
                    timeAsExact += updateInterval;
                    timeAsExact = (float)Math.Round(timeAsExact, digitCountAfterComa);
                    if(acceleration == 0)
                    {
                        displacement = currentVelocity * timeAsExact;
                        exactDisplacement = (float)Math.Round(displacement, digitCountAfterComa);
                        exactCurrentVelocity = (float)Math.Round(currentVelocity, digitCountAfterComa);
                    }
                    else 
                    {
                        exactCurrentVelocity = (float)Math.Round(startVelocity + (acceleration * timeAsExact), digitCountAfterComa);
                        displacement = (startVelocity * timeAsExact) + (acceleration * timeAsExact * timeAsExact / 2);
                        exactDisplacement = (float)Math.Round(displacement, digitCountAfterComa);
                    }
                    ChangeTexts();
                    UpdateGraphics();
                    timeInSequence = 0;
                }
                time += Time.deltaTime;
                timeInSequence += Time.deltaTime;
                if(acceleration != 0)
                {
                    currentVelocity = startVelocity + (acceleration * timeAsExact);
                    backgroundScroller.AssignSpeed(currentVelocity);
                }
                leftWheel.Rotate(0, 0, (float) -currentVelocity * Time.deltaTime * 10);
                rightWheel.Rotate(0, 0, (float) -currentVelocity * Time.deltaTime * 10);
            }
            yield return null;
        }
    }

    private bool CanVTGraphicBeUpdated()
    {
        if (isVTGraphFinished)
        {
            return false;
        }
        else if((startVelocity > 0 && exactCurrentVelocity <= 0)
            || (startVelocity < 0 && exactCurrentVelocity >= 0))
        {
            float timeAtZeroVelocity = (float)Math.Round(-startVelocity / acceleration, GetDigitCountAfterComma(updateInterval));
            coordinateSystemsManager.UpdateVTGraphic(timeAtZeroVelocity, 0);
            isVTGraphFinished = true;
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool CanATGraphicBeUpdated()
    {
        if(acceleration == 0)
        {
            return false;
        }
        return true;
    }

    private int GetDigitCountAfterComma(float value)
    {
        string valueAsString = value.ToString();
        if (value == (int)value)
        {
            return 0;
        }
        return valueAsString.Split(',')[1].Length;
    }

    

    private void UpdateGraphics()
    {
        if (CanVTGraphicBeUpdated())
        {
            coordinateSystemsManager.UpdateVTGraphic(timeAsExact, exactCurrentVelocity);
        }
        if (CanXTGraphicBeUpdated())
        {
            coordinateSystemsManager.UpdateXTGraphic(timeAsExact, exactDisplacement);
        }
        if (CanATGraphicBeUpdated())
        {
            coordinateSystemsManager.UpdateATGraphic(timeAsExact, acceleration);
        }
    }

    private bool CanXTGraphicBeUpdated()
    {
        if (isXTGraphFinished)
        {
            return false;
        }
        else if((startVelocity > 0 && currentVelocity <= 0 && acceleration < 0 && displacement <= 0)
             || (startVelocity < 0 && currentVelocity >= 0 && acceleration > 0 && displacement >= 0))
        {
            lastTime = (float)Math.Round(-2 * startVelocity / acceleration, GetDigitCountAfterComma(updateInterval));
            coordinateSystemsManager.UpdateXTGraphic(lastTime, 0);
            isXTGraphFinished = true;
            lastTime = 0;
            return false;
        }
        else
        {
            return true;
        }
    }

    private void ChangeTexts()
    {
        displacementText.text = (exactDisplacement).ToString();
        velocityText.text = (exactCurrentVelocity).ToString();
        timeText.text = (timeAsExact).ToString();
        if(accelerationText != null)
        {
            accelerationText.text = acceleration.ToString();
        }
    }
}