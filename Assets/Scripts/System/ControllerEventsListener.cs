using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ControllerEventsListener : MonoBehaviour
{


    public static VRTK_ControllerEvents latestEventSender; // Which is the controller that just sent an event
    public static bool touchpadReleased; // If the touchpad is just released
    public static bool triggerUnclicked; // If the trigger is just unclicked
    public static bool triggerClickedDown; // If the tirgger is clicked down
    public static bool triggerClicked; // If the trigger is just clicked
    public static bool gripReleased; // If the grip button is just released
    public static bool gripPressedDown; // If the grip is clicked down

    // Testing
    public VRTK_ControllerEvents testLatestEventSender;

    // Use this for initialization
    void Start()
    {
        AssignGlobalControllerEvents();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(ResetReleasingIndicators());

        // Testing
        testLatestEventSender = latestEventSender;

        //if (touchpadReleased)
        //{
        //    print("update: " + touchpadReleased);
        //}
    }

    private void LateUpdate()
    {
        ResetReleasingIndicators();
    }

    /// <summary>
    /// Reset a release statement back to false if it is true
    /// </summary>
    public IEnumerator ResetReleasingIndicators()
    {
        if (touchpadReleased)
        {
            yield return new WaitForEndOfFrame();
            touchpadReleased = false;
        }

        if (triggerUnclicked)
        {
            yield return new WaitForEndOfFrame();
            triggerUnclicked = false;
        }

        if (triggerClicked)
        {
            yield return new WaitForEndOfFrame();
            triggerClicked = false;
        }

        if (gripReleased)
        {
            yield return new WaitForEndOfFrame();
            gripReleased = false;
        }
    }

    /// <summary>
    /// Assign global functions to the VRTK_ControllerEvents on both controllers
    /// </summary>
    public void AssignGlobalControllerEvents()
    {
        foreach (VRTK_ControllerEvents c in FindObjectsOfType<VRTK_ControllerEvents>())
        {
            c.TouchpadReleased += new ControllerInteractionEventHandler(DetectTouchpadRelease);
            c.TriggerClicked += new ControllerInteractionEventHandler(DetectTriggerClick);
            c.TriggerUnclicked += new ControllerInteractionEventHandler(DetectTriggerUnclick);
            c.GripReleased += new ControllerInteractionEventHandler(DetectGripRelease);
            c.GripPressed += new VRTK.ControllerInteractionEventHandler(DetectGripPressed);
        }
    }

    /// <summary>
    /// Detect when the touchpad of any of the two controllers is released
    /// </summary>
    public void DetectGripRelease(object sender, ControllerInteractionEventArgs e)
    {
        latestEventSender = (VRTK_ControllerEvents)sender;
        gripReleased = true;
        gripPressedDown = false;
    }

    /// <summary>
    /// Detect when the touchpad of any of the two controllers is released
    /// </summary>
    public void DetectGripPressed(object sender, ControllerInteractionEventArgs e)
    {
        latestEventSender = (VRTK_ControllerEvents)sender;
        gripPressedDown = true;
    }

    /// <summary>
    /// Detect when the touchpad of any of the two controllers is released
    /// </summary>
    public void DetectTriggerUnclick(object sender, ControllerInteractionEventArgs e)
    {
        latestEventSender = (VRTK_ControllerEvents)sender;
        triggerUnclicked = true;
        triggerClickedDown = false;
    }

    /// <summary>
    /// Detect when the touchpad of any of the two controllers is released
    /// </summary>
    public void DetectTriggerClick(object sender, ControllerInteractionEventArgs e)
    {
        latestEventSender = (VRTK_ControllerEvents)sender;
        triggerClickedDown = true;
        triggerClicked = true;

        // Testing
        //print("trigger clicked event: " + triggerClicked);

    }

    /// <summary>
    /// Detect when the touchpad of any of the two controllers is released
    /// </summary>
    public void DetectTouchpadRelease(object sender, ControllerInteractionEventArgs e)
    {
        latestEventSender = (VRTK_ControllerEvents)sender;
        touchpadReleased = true;
    }
}
