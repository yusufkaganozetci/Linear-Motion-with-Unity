using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject settingsPopup;
    [SerializeField] private Animator instructionScreenAnimator;
    private CarMovement carMovement;
    void Start()
    {
        carMovement = FindObjectOfType<CarMovement>();
    }

    

    public void OnPressSettingsButton()
    {
        settingsPopup.SetActive(true);
        carMovement.StopTheSimulation();
    }

    public void OnPressCloseSettingsButton()
    {
        settingsPopup.SetActive(false);
        carMovement.ContinueToTheSimulation();
    }

    public void OnPressLearnButton()
    {
        instructionScreenAnimator.SetTrigger("PlayInstructionScreenClosingAnim");
    }
}
