using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlyActivateOnce : MonoBehaviour
{
    public OnlyActivateOnce previousTutorial;

    public bool hasOpened;
    public bool shouldOpen;
    public Canvas tutorialCanvas;

    // Use this for initialization
    void Start()
    {
        hasOpened = false;
        tutorialCanvas = GetComponent<Canvas>();
    }

    private void OnEnable()
    {
        shouldOpen = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(shouldOpen)
        {
            tutorialCanvas.enabled = true;
        }
        if(hasOpened)
        {
            tutorialCanvas.enabled = false;
        }
        if (previousTutorial != null && !previousTutorial.hasOpened)
        {
            tutorialCanvas.enabled = false;
        }
    }
}
