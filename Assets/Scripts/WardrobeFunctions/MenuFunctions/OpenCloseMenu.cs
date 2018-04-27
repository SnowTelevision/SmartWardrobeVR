using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the open and close of the menu when the menu is closed or is in the first level
/// </summary>
public class OpenCloseMenu : MonoBehaviour
{
    public GameObject firstLevelMenuWrap; // The gameobject that contains the first level menu, hide it when switch to second level menu
    public Transform firstLevelMenuWheel; // The transform of the rotating wheel for first level menu
    public Vector3 menuRelativeAppearPosition; // Where the menu should appear relative to a transform
    public float menuDropHeight; // How far should the menu drops down
    public float menuItemAnimationDuration; // How fast should each menu item drop down or raise up
    public Transform menuAppearPositionReference; // The transform that the appearing position of the menu that is relative to
    public Transform gestureRelativeReference; // What is the gesture relative to

    public bool menuOpened; // Is the menu opened
    public bool isMakingGesture; // If the user is making a gesture

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (CheckIfMirrorGazed() || firstLevelMenuWrap.activeInHierarchy)
        {
            if (!isMakingGesture)
            {
                if (ControllerEventsListener.triggerClicked)
                {
                    isMakingGesture = true;
                    ControllerTriggerDownGestureListener.sControllerTriggerDownGestureListener.GestureStart(gestureRelativeReference, ControllerEventsListener.latestEventSender.transform);
                }
            }
        }

        if (isMakingGesture)
        {
            if (ControllerEventsListener.triggerUnclicked)
            {
                isMakingGesture = false;

                if (firstLevelMenuWrap.activeInHierarchy && ControllerTriggerDownGestureListener.lastGesture == "up")
                {
                    StartCoroutine(CloseMenuAnimation());
                }
                else if (!firstLevelMenuWrap.activeInHierarchy && !menuOpened && ControllerTriggerDownGestureListener.lastGesture == "down")
                {
                    StartCoroutine(OpenMenuAnimation());
                }

                ControllerTriggerDownGestureListener.sControllerTriggerDownGestureListener.GestureStop();
            }
        }
    }

    /// <summary>
    /// Check if the mirror is being gazed
    /// </summary>
    /// <returns></returns>
    public bool CheckIfMirrorGazed()
    {
        return GetComponentInChildren<CheckIfGazed>().isBeingGazed;
    }

    /// <summary>
    /// Animation that opens the menu
    /// </summary>
    /// <returns></returns>
    public IEnumerator OpenMenuAnimation()
    {
        firstLevelMenuWrap.transform.position = menuAppearPositionReference.TransformPoint(menuRelativeAppearPosition);
        firstLevelMenuWrap.SetActive(true);
        menuOpened = true;
        for (int i = 0; i < firstLevelMenuWrap.GetComponentInChildren<OrganizeMenuItems>().menuItems.Count; i++)
        {
            firstLevelMenuWrap.GetComponentInChildren<OrganizeMenuItems>().menuItems[i].transform.localPosition += Vector3.up * menuDropHeight;
        }

        //Vector3 itemInitialPosition = firstLevelMenuWheel.localPosition;
        //for (float t = 0; t < 1; t += Time.deltaTime / menuItemAnimationDuration)
        //{
        //    firstLevelMenuWheel.localPosition = Vector3.Lerp(itemInitialPosition, itemInitialPosition - Vector3.up * menuDropHeight, t);
        //    yield return null;
        //}
        //firstLevelMenuWheel.localPosition = itemInitialPosition - Vector3.up * menuDropHeight;

        for (int i = 0; i < firstLevelMenuWrap.GetComponentInChildren<OrganizeMenuItems>().menuItems.Count; i++)
        {
            Vector3 itemInitialPosition = firstLevelMenuWrap.GetComponentInChildren<OrganizeMenuItems>().menuItems[i].transform.localPosition;
            for (float t = 0; t < 1; t += Time.deltaTime / menuItemAnimationDuration)
            {
                firstLevelMenuWrap.GetComponentInChildren<OrganizeMenuItems>().menuItems[i].transform.localPosition =
                    Vector3.Lerp(itemInitialPosition, itemInitialPosition - Vector3.up * menuDropHeight, t);
                yield return null;
            }
            firstLevelMenuWrap.GetComponentInChildren<OrganizeMenuItems>().menuItems[i].transform.localPosition =
                itemInitialPosition - Vector3.up * menuDropHeight;
        }

        //firstLevelMenuWrap.transform.position -= Vector3.up * menuDropHeight;
        //firstLevelMenuWheel.localPosition += Vector3.up * menuDropHeight;
        //for (int i = 0; i < firstLevelMenuWrap.GetComponentInChildren<OrganizeMenuItems>().menuItems.Count; i++)
        //{
        //    firstLevelMenuWrap.GetComponentInChildren<OrganizeMenuItems>().menuItems[i].transform.localPosition += Vector3.up * menuDropHeight;
        //}
    }

    /// <summary>
    /// Animation that closes the menu
    /// </summary>
    /// <returns></returns>
    public IEnumerator CloseMenuAnimation()
    {
        //firstLevelMenuWrap.transform.position += Vector3.up * menuDropHeight;
        //for (int i = 0; i < firstLevelMenuWrap.GetComponentInChildren<OrganizeMenuItems>().menuItems.Count; i++)
        //{
        //    firstLevelMenuWrap.GetComponentInChildren<OrganizeMenuItems>().menuItems[i].transform.localPosition -= Vector3.up * menuDropHeight;
        //}

        //Vector3 itemInitialPosition = firstLevelMenuWheel.localPosition;
        //for (float t = 0; t < 1; t += Time.deltaTime / menuItemAnimationDuration)
        //{
        //    firstLevelMenuWheel.localPosition = Vector3.Lerp(itemInitialPosition, itemInitialPosition + Vector3.up * menuDropHeight, t);
        //    yield return null;
        //}
        //firstLevelMenuWheel.localPosition = itemInitialPosition + Vector3.up * menuDropHeight;

        for (int i = 0; i < firstLevelMenuWrap.GetComponentInChildren<OrganizeMenuItems>().menuItems.Count; i++)
        {
            Vector3 itemInitialPosition = firstLevelMenuWrap.GetComponentInChildren<OrganizeMenuItems>().menuItems[i].transform.localPosition;
            for (float t = 0; t < 1; t += Time.deltaTime / menuItemAnimationDuration)
            {
                firstLevelMenuWrap.GetComponentInChildren<OrganizeMenuItems>().menuItems[i].transform.localPosition =
                    Vector3.Lerp(itemInitialPosition, itemInitialPosition + Vector3.up * menuDropHeight, t);
                yield return null;
            }
            firstLevelMenuWrap.GetComponentInChildren<OrganizeMenuItems>().menuItems[i].transform.localPosition =
                itemInitialPosition + Vector3.up * menuDropHeight;
        }

        //firstLevelMenuWrap.transform.position += Vector3.up * menuDropHeight;
        //firstLevelMenuWheel.localPosition -= Vector3.up * menuDropHeight;
        //for (int i = 0; i < firstLevelMenuWrap.GetComponentInChildren<OrganizeMenuItems>().menuItems.Count; i++)
        //{
        //    firstLevelMenuWrap.GetComponentInChildren<OrganizeMenuItems>().menuItems[i].transform.localPosition -= Vector3.up * menuDropHeight;
        //}

        firstLevelMenuWrap.SetActive(false);
        menuOpened = false;
    }
}
