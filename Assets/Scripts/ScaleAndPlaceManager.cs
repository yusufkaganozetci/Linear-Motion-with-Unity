using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAndPlaceManager : MonoBehaviour
{
    [SerializeField] Camera mainCamera;

    [SerializeField] RectTransform topNavigationBar;
    [SerializeField] Transform background;
    [SerializeField] Transform blueSky;//background
    [SerializeField] Transform floor;


    
    private float cameraWidth, cameraHeight;
    
    
    // Start is called before the first frame update
    void Start()
    {
        cameraHeight = 2f * mainCamera.orthographicSize;
        cameraWidth = cameraHeight * mainCamera.aspect;
        /*quadOne.sortingLayerName = "Default";
        quadOne.sortingOrder = 0;
        ScaleObjects();
        PlaceObjects();*/
    }

    private void ScaleObjects()
    {
        float backgroundScale = (1080 - topNavigationBar.rect.height) / 1080;
        blueSky.localScale = new Vector3(backgroundScale, backgroundScale, backgroundScale);
        floor.localScale = new Vector3(backgroundScale, backgroundScale, backgroundScale);
    }

    private void PlaceObjects()
    {
        float blueSkyYPos = (-cameraHeight / 2) + (blueSky.GetComponent<SpriteRenderer>().bounds.size.y / 2);
        float floorYPos = (-cameraHeight / 2) + (floor.GetComponent<SpriteRenderer>().bounds.size.y / 2);
        blueSky.transform.position = new Vector3(0, blueSkyYPos, 0);
        floor.transform.position = new Vector3(0, floorYPos, 0);
    }
}