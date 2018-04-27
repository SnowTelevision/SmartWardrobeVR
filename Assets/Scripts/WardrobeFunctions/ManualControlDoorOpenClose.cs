using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.UI;

public class ManualControlDoorOpenClose : VRTK_InteractableObject
{
    public GameObject openDoorButton;
    public GameObject closeDoorButton;
    public Image openInactiveImage;
    public Image openActiveImage;
    public Image closeInactiveImage;
    public Image closeActiveImage;

    public bool justControlled;
    //public static bool manuallyOpened; // If the door is manually opened
    //public static bool manuallyClosed; // If the door is manually closed

    // Use this for initialization
    void Start()
    {
        justControlled = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        ControlButtonActive();
        ControlButtonColor();
        CheckIfControlDoor();
    }

    public void ControlButtonActive()
    {
        if (WardrobeDatabase.isDoorOpen)
        {
            if (openDoorButton.activeInHierarchy)
            {
                openDoorButton.SetActive(false);
                closeDoorButton.SetActive(true);
            }
        }
        else
        {
            if (closeDoorButton.activeInHierarchy)
            {
                openDoorButton.SetActive(true);
                closeDoorButton.SetActive(false);
            }
        }
    }

    public void ControlButtonColor()
    {
        if (IsTouched())
        {
            openInactiveImage.enabled = false;
            openActiveImage.enabled = true;
            closeInactiveImage.enabled = false;
            closeActiveImage.enabled = true;
        }
        else
        {
            openInactiveImage.enabled = true;
            openActiveImage.enabled = false;
            closeInactiveImage.enabled = true;
            closeActiveImage.enabled = false;
        }
    }

    public void CheckIfControlDoor()
    {
        if (!IsTouched() || justControlled)
        {
            justControlled = false;
            return;
        }

        foreach (GameObject g in touchingObjects)
        {
            if (ControllerEventsListener.touchpadReleased &&
                g.GetComponent<VRTK_ControllerEvents>() &&
                ControllerEventsListener.latestEventSender == g.GetComponent<VRTK_ControllerEvents>())
            {
                justControlled = true; // Prevent the door control run twice because of the controller event set back delay

                if (openDoorButton.activeInHierarchy)
                {
                    if (WardrobeDatabase.database.rotateDoorCoroutine != null)
                    {
                        StopCoroutine(WardrobeDatabase.database.rotateDoorCoroutine);
                    }
                    WardrobeDatabase.database.rotateDoorCoroutine =
                        WardrobeDatabase.database.StartCoroutine(WardrobeDatabase.database.RotateDoor(true));
                }
                else
                {
                    if (WardrobeDatabase.database.rotateDoorCoroutine != null)
                    {
                        StopCoroutine(WardrobeDatabase.database.rotateDoorCoroutine);
                    }
                    WardrobeDatabase.database.rotateDoorCoroutine =
                        WardrobeDatabase.database.StartCoroutine(WardrobeDatabase.database.RotateDoor(false));
                }
            }
        }

    }
}
