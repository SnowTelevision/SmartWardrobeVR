using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

/// <summary>
/// Let user hold down trigger to pull menu items in certain conditions
/// </summary>
public class TriggerPullsMenuItem : MonoBehaviour
{
    //public float startPullAngle; // What is the maximum angle between the vector from the body to the menu item and the body to the controller can be to start pull
    //public float startPullMinDist; // What is the minimum distance between the body and the controller to start pull
    //public float startPullMaxDist; // What is the maximum distance between the body and the controller to start pull
    public Vector3 headToBodyDist; // What is the distance from the head to the body
    //public float pullDistanceRatio; // How long the item should be pulled along an axis when the actual controller is moved along that axis
    //public bool pullX; // Does the item pulls along the x axis;
    //public bool pullY; // Does the item pulls along the y axis;
    //public bool pullZ; // Does the item pulls along the z axis;
    public GameObject menuNavigator; // The UI element that can be interact to move the menu items
    //public float confirmDist; // How close the menu item needs to be with the body to confirm entering the next menu
    public GameObject firstLevelMenuWrap; // The gameobject that contains the first level menu, hide it when switch to second level menu
    public GameObject secondLevelMenuWrap; // The gameobject that contains the second level menu
    public Vector3 secondLevelMenuRelativePosition; // Where the second level menu should appear according to the user
    public bool isSecondLevel; // Is this second level menu

    public Transform userHead; // The transform of user's head
    //public GameObject pullCenter; // Everytime the user starts pulling, a new center will be created at the position of the player, facing the menu item, and its up pointing up
    public bool isPulling; // Is the item being pulled right now
    public Vector3 startRelativePosition; // The menu items relative position to the center when the pull started
    public Transform pullingController; // The transform of the controller that is currently pulling it
    public bool inEndPullAnimation; // If the end pull animation is played
    public bool retractMenuItemWhenStartPull; // If play the retract menu item animation when start pull
    public bool expandMenuItemWhenEndPull; // If play the expand menu item animation when end pull
    public Vector3 secondMenuGoBackPosition; // Where the retracted second level menu should move back

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (userHead == null)
        {
            userHead = FindObjectOfType<SteamVR_Camera>().transform;
        }

        if (isSecondLevel)
        {
            if (!isPulling)
            {
                if (ControllerEventsListener.triggerClicked)
                {
                    isPulling = true;
                    ControllerTriggerDownGestureListener.sControllerTriggerDownGestureListener.GestureStart(transform, ControllerEventsListener.latestEventSender.transform);
                }
            }
            else
            {
                if (ControllerEventsListener.triggerUnclicked)
                {
                    isPulling = false;

                    if (ControllerTriggerDownGestureListener.lastGesture == "push")
                    {
                        StartCoroutine(SwitchToFirstLevel());
                    }
                    else if (ControllerTriggerDownGestureListener.lastGesture == "left")
                    {
                        GetComponent<SwapMenuItems>().MoveMenu(true);
                    }
                    else if (ControllerTriggerDownGestureListener.lastGesture == "right")
                    {
                        GetComponent<SwapMenuItems>().MoveMenu(false);
                    }
                     
                    ControllerTriggerDownGestureListener.sControllerTriggerDownGestureListener.GestureStop();
                }
            }
        }

        if (isPulling)
        {
            //Pull();
            //CheckIfPulledToConfirm();
        }

        // Test
        //if (transform.name == "Sport" && FindObjectOfType<VRTK_ControllerEvents>())
        //{
        //    Transform controller = FindObjectOfType<VRTK_ControllerEvents>().transform;
        //    print(Vector3.Angle((controller.position - (userHead.position + headToBodyDist)), (transform.position - (userHead.position + headToBodyDist))));
        //}
    }

    /// <summary>
    /// Called when the user starts pulling
    /// </summary>
    public void StartPullWhileGaze()
    {
        // Test
        //print("start pull: " + ControllerEventsListener.latestEventSender.transform.name);

        // Stop it being pulled by both controllers
        if (isPulling)
        {
            return;
        }

        pullingController = ControllerEventsListener.latestEventSender.transform;

        // If the controller is not within pull area
        if (!CheckIfControllerWithinPullArea())
        {
            return;
        }

        ControllerTriggerDownGestureListener.sControllerTriggerDownGestureListener.GestureStart(null, pullingController);

        menuNavigator.GetComponent<VRTK_InteractableObject>().isGrabbable = false; // Don't let the user move the menu if the user start pulling a menu item
        isPulling = true;

        //pullCenter = Instantiate(new GameObject(), ControllerEventsListener.latestEventSender.transform.position, Quaternion.identity); // Create new center
        //pullCenter.transform.LookAt(transform); // Center look at the menu item
        //pullCenter.transform.eulerAngles = new Vector3(0, pullCenter.transform.eulerAngles.y, 0); // Center points its up to up

        //startRelativePosition = pullCenter.transform.InverseTransformPoint(transform.position); // Save the menu item's position when the pull start
        startRelativePosition = ControllerEventsListener.latestEventSender.transform.InverseTransformPoint(transform.position); // Save the menu item's position when the pull start
    }

    /// <summary>
    /// Check if the controller is within the pulling area when the user starts pulling
    /// </summary>
    /// <returns></returns>
    public bool CheckIfControllerWithinPullArea()
    {
        // Test
        //print("start check");

        bool withinArea = true;

        //// Check the angle
        //if (Vector3.Angle((pullingController.position - (userHead.position + headToBodyDist)), (transform.position - (userHead.position + headToBodyDist))) >= startPullAngle)
        //{
        //    withinArea = false;
        //}
        //// Check the distance
        //if ((pullingController.position - (userHead.position + headToBodyDist)).magnitude <= startPullMinDist ||
        //    (pullingController.position - (userHead.position + headToBodyDist)).magnitude >= startPullMaxDist)
        //{
        //    withinArea = false;
        //}

        // Test
        //print("angle:" + Vector3.Angle((pullingController.position - (userHead.position + headToBodyDist)), (transform.position - (userHead.position + headToBodyDist))));
        //print("distance: " + (pullingController.position - (userHead.position + headToBodyDist)).magnitude);

        return withinArea;
    }

    ///// <summary>
    ///// Called when the user is pulling
    ///// </summary>
    //public void Pull()
    //{
    //    Vector3 newRelativePosition = Vector3.zero;

    //    if (pullX)
    //    {
    //        newRelativePosition.x = startRelativePosition.x + pullCenter.transform.InverseTransformPoint(pullingController.position).x * pullDistanceRatio;
    //    }
    //    if (pullY)
    //    {
    //        newRelativePosition.y = startRelativePosition.y + pullCenter.transform.InverseTransformPoint(pullingController.position).y * pullDistanceRatio;
    //    }
    //    if (pullZ)
    //    {
    //        newRelativePosition.z = startRelativePosition.z + pullCenter.transform.InverseTransformPoint(pullingController.position).z * pullDistanceRatio;
    //    }

    //    transform.position = pullCenter.transform.TransformPoint(newRelativePosition);
    //}

    /// <summary>
    /// Called when the user ends pulling
    /// </summary>
    /// <param name="confirm"></param>
    public void EndPullWhileGaze(bool confirm)
    {
        if (!isPulling)
        {
            return;
        }

        if (ControllerTriggerDownGestureListener.lastGesture == "pull" && !isSecondLevel)
        {
            StartCoroutine(SwitchToSecondLevel());
        }

        if (ControllerTriggerDownGestureListener.lastGesture == "push" && isSecondLevel)
        {
            StartCoroutine(SwitchToFirstLevel());
        }

        if (ControllerTriggerDownGestureListener.lastGesture == "")
        {
            isPulling = false;
            menuNavigator.GetComponent<VRTK_InteractableObject>().isGrabbable = true;
            pullingController = null;
        }

        ControllerTriggerDownGestureListener.sControllerTriggerDownGestureListener.GestureStop();

        //if (confirm)
        //{
        //    // Test
        //    //print("end pull true");

        //    isPulling = false;
        //    menuNavigator.GetComponent<VRTK_InteractableObject>().isGrabbable = true;
        //    pullingController = null;
        //    //Destroy(pullCenter);

        //    // Show second level menu
        //    if (!isSecondLevel)
        //    {
        //        SwitchToSecondLevel();
        //    }
        //    // Show first level menu
        //    else
        //    {
        //        SwitchToFirstLevel();
        //    }
        //}
        //else
        //{
        //    // Test
        //    //print("end pull false");

        //    StartCoroutine(ReturnMenuItem());
        //}
    }

    /// <summary>
    /// Show second level menu at the correct position then hide the first level menu
    /// </summary>
    public IEnumerator SwitchToSecondLevel()
    {
        Vector3 secondLevelMenuTargetPosition = userHead.TransformPoint(secondLevelMenuRelativePosition);
        secondLevelMenuWrap.GetComponent<TriggerPullsMenuItem>().secondMenuGoBackPosition = transform.position;

        secondLevelMenuWrap.transform.position = secondLevelMenuWrap.GetComponent<TriggerPullsMenuItem>().secondMenuGoBackPosition;
        secondLevelMenuWrap.transform.LookAt(
            new Vector3(userHead.position.x, secondLevelMenuWrap.transform.position.y, userHead.position.z), Vector3.up);
        secondLevelMenuWrap.transform.eulerAngles =
            new Vector3(secondLevelMenuWrap.transform.eulerAngles.x, secondLevelMenuWrap.transform.eulerAngles.y + 180, secondLevelMenuWrap.transform.eulerAngles.z);
        //secondLevelMenuWrap.transform.eulerAngles = new Vector3(0, secondLevelMenuWrap.transform.eulerAngles.y, 0);
        //secondLevelMenuWrap.transform.eulerAngles = new Vector3(0, userHead.eulerAngles.y, 0);
        secondLevelMenuWrap.SetActive(true);

        for (float t = 0; t < 1; t += Time.deltaTime / GetComponentInParent<OrganizeMenuItems>().menuChangeAnimationDuration)
        {
            secondLevelMenuWrap.transform.position =
                Vector3.Lerp(secondLevelMenuWrap.GetComponent<TriggerPullsMenuItem>().secondMenuGoBackPosition, secondLevelMenuTargetPosition, t);
            yield return null;
        }
        secondLevelMenuWrap.transform.position = userHead.TransformPoint(secondLevelMenuRelativePosition);

        secondLevelMenuWrap.GetComponent<OrganizeMenuItems>().followPositionOffset = secondLevelMenuWrap.transform.position - userHead.position;

        GetComponentInParent<OrganizeMenuItems>().playExpandAnimation = false;

        // Enable the menu item animations when start or end pull second level
        secondLevelMenuWrap.GetComponent<TriggerPullsMenuItem>().expandMenuItemWhenEndPull = true;
        secondLevelMenuWrap.GetComponent<TriggerPullsMenuItem>().retractMenuItemWhenStartPull = true;

        yield return null;
        isPulling = false;
        menuNavigator.GetComponent<VRTK_InteractableObject>().isGrabbable = true;
        pullingController = null;

        firstLevelMenuWrap.SetActive(false);
    }

    /// <summary>
    /// Hide the second level menu then show first level menu
    /// </summary>
    public IEnumerator SwitchToFirstLevel()
    {
        StartCoroutine(GetComponent<OrganizeMenuItems>().RetractMenuItems());

        Vector3 secondLevelMenuStartGoBackPosition = transform.position;
        for (float t = 0; t < 1; t += Time.deltaTime / GetComponent<OrganizeMenuItems>().menuChangeAnimationDuration)
        {
            transform.position = Vector3.Lerp(secondLevelMenuStartGoBackPosition, secondMenuGoBackPosition, t);
            yield return null;
        }

        firstLevelMenuWrap.SetActive(true);

        if (isSecondLevel)
        {
            expandMenuItemWhenEndPull = false;
            retractMenuItemWhenStartPull = false;
        }

        isPulling = false;
        menuNavigator.GetComponent<VRTK_InteractableObject>().isGrabbable = true;
        pullingController = null;

        yield return null;

        secondLevelMenuWrap.SetActive(false);
    }



    ///// <summary>
    ///// Animation that put the menu item back on its original position on the first level menu
    ///// </summary>
    ///// <returns></returns>
    //public IEnumerator ReturnMenuItem()
    //{
    //    Vector3 endPullRelativePosition = pullCenter.transform.InverseTransformPoint(transform.position);

    //    for (float t = 0; t < 0.5f; t += Time.deltaTime)
    //    {
    //        transform.position = pullCenter.transform.TransformPoint(Vector3.Lerp(endPullRelativePosition, startRelativePosition, t));

    //        yield return null;
    //    }
    //    transform.position = pullCenter.transform.TransformPoint(startRelativePosition);

    //    isPulling = false;
    //    menuNavigator.GetComponent<VRTK_InteractableObject>().isGrabbable = true;
    //    pullingController = null;
    //    Destroy(pullCenter);
    //}

    ///// <summary>
    ///// Check if the menu item reaches close enough to the user to enter the next level menu
    ///// </summary>
    //public void CheckIfPulledToConfirm()
    //{
    //    if (!isSecondLevel && (transform.position - userHead.position).magnitude <= confirmDist)
    //    {
    //        EndPull(true);
    //    }
    //    else if (isSecondLevel && (transform.position - userHead.position).magnitude >= confirmDist)
    //    {
    //        EndPull(true);
    //    }
    //}
}
