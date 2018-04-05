using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapMenuItems : MonoBehaviour
{
    public TriggerDetectControllerEnter leftTrigger;
    public TriggerDetectControllerEnter rightTrigger;

    // Test
    public Vector3 objectToFollowInverseTransformPointtransformposition;
    public Vector3 transformInverseTransformPointGetComponentOrganizeMenuItemsobjectToFollowtransformposition;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //DetectIfSwap();

        // Test
        objectToFollowInverseTransformPointtransformposition = GetComponent<OrganizeMenuItems>().objectToFollow.InverseTransformPoint(transform.position);
        transformInverseTransformPointGetComponentOrganizeMenuItemsobjectToFollowtransformposition = 
            transform.InverseTransformPoint(GetComponent<OrganizeMenuItems>().objectToFollow.transform.position);
    }

    ///// <summary>
    ///// Detect if the user wave the controller from one trigger to the other
    ///// </summary>
    //public void DetectIfSwap()
    //{
    //    // Controller move from right trigger to left trigger
    //    if (leftTrigger.lastControllerEnterTime - rightTrigger.lastControllerExitTime > 0 &&
    //        leftTrigger.lastControllerEnterTime - rightTrigger.lastControllerExitTime <= 0.25f &&
    //        leftTrigger.lastEnteredController == rightTrigger.lastExitedController)
    //    {
    //        MoveMenu(true);
    //    }

    //    // Controller move from left trigger to right trigger
    //    if (rightTrigger.lastControllerEnterTime - leftTrigger.lastControllerExitTime > 0 &&
    //        rightTrigger.lastControllerEnterTime - leftTrigger.lastControllerExitTime <= 0.25f &&
    //        rightTrigger.lastEnteredController == leftTrigger.lastExitedController)
    //    {
    //        MoveMenu(false);
    //    }
    //}

    /// <summary>
    /// Move the menu
    /// </summary>
    /// <param name="moveLeft"></param>
    public void MoveMenu(bool moveLeft)
    {
        // Reset trigger status
        leftTrigger.lastControllerEnterTime = 0;
        leftTrigger.lastControllerExitTime = 0;
        leftTrigger.lastEnteredController = null;
        leftTrigger.lastExitedController = null;
        rightTrigger.lastControllerEnterTime = 0;
        rightTrigger.lastControllerExitTime = 0;
        rightTrigger.lastEnteredController = null;
        rightTrigger.lastExitedController = null;

        if (moveLeft)
        {
            // If the menu is already at far right
            if (Mathf.Abs(transform.InverseTransformPoint(GetComponent<OrganizeMenuItems>().objectToFollow.transform.position).x -
                          ((GetComponent<OrganizeMenuItems>().menuItems.Count - 1) *
                           GetComponent<OrganizeMenuItems>().menuItemSpacing)) <= GetComponent<OrganizeMenuItems>().menuItemSpacing * 0.2f)
            {
                return;
            }

            GetComponent<OrganizeMenuItems>().followPositionOffset -= GetComponent<OrganizeMenuItems>().menuItemSpacing * transform.right;
            leftTrigger.transform.localPosition =
                new Vector3(leftTrigger.transform.localPosition.x + GetComponent<OrganizeMenuItems>().menuItemSpacing,
                            leftTrigger.transform.localPosition.y, leftTrigger.transform.localPosition.z);
            rightTrigger.transform.localPosition =
                new Vector3(rightTrigger.transform.localPosition.x + GetComponent<OrganizeMenuItems>().menuItemSpacing,
                            rightTrigger.transform.localPosition.y, rightTrigger.transform.localPosition.z);

            GetComponent<TryOnCloth>().currentFacingCloth =
                GetComponent<OrganizeMenuItems>().menuItems[GetComponent<OrganizeMenuItems>().menuItems.
                    IndexOf(GetComponent<TryOnCloth>().currentFacingCloth) + 1];
        }
        else
        {
            // If the menu is already at far left
            if (Mathf.Abs(transform.InverseTransformPoint(GetComponent<OrganizeMenuItems>().objectToFollow.transform.position).x) <= 
                GetComponent<OrganizeMenuItems>().menuItemSpacing * 0.2f)
            {
                return;
            }

            GetComponent<OrganizeMenuItems>().followPositionOffset += GetComponent<OrganizeMenuItems>().menuItemSpacing * transform.right;
            leftTrigger.transform.localPosition =
                new Vector3(leftTrigger.transform.localPosition.x - GetComponent<OrganizeMenuItems>().menuItemSpacing,
                            leftTrigger.transform.localPosition.y, leftTrigger.transform.localPosition.z);
            rightTrigger.transform.localPosition =
                new Vector3(rightTrigger.transform.localPosition.x - GetComponent<OrganizeMenuItems>().menuItemSpacing,
                            rightTrigger.transform.localPosition.y, rightTrigger.transform.localPosition.z);

            GetComponent<TryOnCloth>().currentFacingCloth =
                GetComponent<OrganizeMenuItems>().menuItems[GetComponent<OrganizeMenuItems>().menuItems.
                    IndexOf(GetComponent<TryOnCloth>().currentFacingCloth) - 1];
        }
    }
}
