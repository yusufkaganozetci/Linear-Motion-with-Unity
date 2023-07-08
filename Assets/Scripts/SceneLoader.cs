using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    [SerializeField] Animator transitionAnimator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowSceneChooseScreen()
    {
        transitionAnimator.gameObject.SetActive(true);
        transitionAnimator.SetTrigger("PlayTransitionClosingAnim");
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
