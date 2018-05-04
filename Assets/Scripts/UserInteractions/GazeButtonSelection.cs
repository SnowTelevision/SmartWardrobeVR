using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Trigger events when an item is gazed and a certain button on the controller is pressed and released
/// </summary>
public class GazeButtonSelection : MonoBehaviour
{
    public bool detectClickedTrigger; // Do we detect trigger just clicked
    public bool detectUnclickedTrigger; // Do we detect trigger just unclicked
    public bool detectHoldingGrip; // Do we detect grip holding down
    public bool detectTouchpadReleased; // Do we detect touchpad released
    public float gazeSizeMultiplier; // How much the size of the object should increase when gazed
    public GameObject alternateIncreaseSizeModel; // Increase the size of a specific gameobject 
                                                  //instead of the gameobject that this script is attached to
    public GameObject gazingSecondTitle;
    public GameObject notGazingSecondTitle;
    public GameObject pullSecondMenuTutorial;
    //public OnlyActivateOnce rotateFirstMenuTutorial;

    public UnityEvent toBeCalledWhenConfirmed; // The function to be called when
    public bool isLarge; // If the object is enlarged

    // Use this for initialization
    void Start()
    {
        isLarge = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfInvokeFunction();
    }

    /// <summary>
    /// Call the target function
    /// </summary>
    public void CallTargetFunction()
    {
        toBeCalledWhenConfirmed.Invoke();
    }

    /// <summary>
    /// Check if the target function should be called when this item is being gazed and the controller touchpad is pressed down
    /// </summary>
    public void CheckIfInvokeFunction()
    {
        // If it has a gaze detector
        if (GetComponentInChildren<CheckIfGazed>())
        {
            // If it is gazed
            if (GetComponentInChildren<CheckIfGazed>().isBeingGazed)
            {
                // If it is not enlarged & it is the first GazeButtonSelection component on the object
                if (!isLarge && GetComponents<GazeButtonSelection>()[0] == this)
                {
                    if (alternateIncreaseSizeModel == null)
                    {
                        transform.localScale *= gazeSizeMultiplier; // Make it large
                    }
                    else
                    {
                        alternateIncreaseSizeModel.transform.localScale *= gazeSizeMultiplier; // Make it large
                    }

                    isLarge = true;
                }

                if (CheckButtonConditions())
                {
                    CallTargetFunction();
                }

                if (pullSecondMenuTutorial != null && //rotateFirstMenuTutorial.hasOpened && 
                    !pullSecondMenuTutorial.GetComponent<OnlyActivateOnce>().hasOpened)
                {
                    pullSecondMenuTutorial.SetActive(true);
                }
                if (gazingSecondTitle != null)
                {
                    gazingSecondTitle.SetActive(true);
                    notGazingSecondTitle.SetActive(false);
                }
            }
            else
            {
                // If it is enlarged & it is the first GazeButtonSelection component on the object
                if (isLarge && GetComponents<GazeButtonSelection>()[0] == this)
                {
                    if (alternateIncreaseSizeModel == null)
                    {
                        transform.localScale /= gazeSizeMultiplier; // Make it small
                    }
                    else
                    {
                        alternateIncreaseSizeModel.transform.localScale /= gazeSizeMultiplier; // Make it large
                    }

                    isLarge = false;
                }

                if (pullSecondMenuTutorial != null)
                {
                    //pullSecondMenuTutorial.SetActive(false);
                }
                if (gazingSecondTitle != null)
                {
                    gazingSecondTitle.SetActive(false);
                    notGazingSecondTitle.SetActive(true);
                }
            }
        }
    }

    /// <summary>
    /// Check if the button status meet the conditions to trigger the event
    /// </summary>
    public bool CheckButtonConditions()
    {
        bool callFunction = true;

        if (detectTouchpadReleased && !ControllerEventsListener.touchpadReleased)
        {
            callFunction = false;
        }
        if (detectClickedTrigger && !ControllerEventsListener.triggerClicked)
        {
            callFunction = false;
        }
        if (detectUnclickedTrigger && !ControllerEventsListener.triggerUnclicked)
        {
            callFunction = false;
        }

        // Test
        //if (transform.name == "Sport")
        //{
        //    print("trigger unclicked: " + ControllerEventsListener.triggerUnclicked);
        //    //print("");
        //}

        return callFunction;
    }
}
