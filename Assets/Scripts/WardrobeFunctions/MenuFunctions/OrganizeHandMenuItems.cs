using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Organize the history or saved menu that will show up on user's wrist
/// </summary>
public class OrganizeHandMenuItems : MonoBehaviour
{
    public GameObject historyAndSavedClothMenuWrap; // The gameobject that has the history cloth and saved cloth menu
    public GameObject historyClothMenuWrap; // The history cloth menu
    public GameObject savedClothMenuWrap; // The saved cloth menu
    public Transform userBody; // The user's body's transform
    public OrganizeHandMenuItems otherHandMenu; // The menu on the other hand

    public GameObject userHead; // The head object of the user
    public bool savedInner; // Is the saved menu in the inner side relative to the user or the outter side?

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (userHead == null)
        {
            userHead = FindObjectOfType<SteamVR_Camera>().gameObject;
        }

        CheckControllerRelativeRotation();

        if (historyAndSavedClothMenuWrap.activeInHierarchy)
        {

        }
    }

    /// <summary>
    /// Checks the relative rotation of the controller with the head
    /// </summary>
    public void CheckControllerRelativeRotation()
    {
        //print("up: " + Vector3.Angle(userHead.transform.up, transform.up) + ", forward: " + Vector3.Angle(userHead.transform.forward, transform.forward));

        // If the controller is upside-down relative to the head and is pointing to the head's relative forward direction
        if (Vector3.Angle(userHead.transform.up, transform.up) >= 150)
        {
            historyAndSavedClothMenuWrap.SetActive(true);
        }
        else
        {
            historyAndSavedClothMenuWrap.SetActive(false);
        }
    }

    /// <summary>
    /// Check if open either of the second level menus (history or saved)
    /// </summary>
    public void CheckOpenSecondMenu()
    {
        Vector3 controllerToBodyVec = userBody.position - transform.position;

        // If the controller it rotated pass the open menu threshold
        if (Vector3.Angle(transform.forward, controllerToBodyVec) < 150)
        {
            // If the controller is rotating towards left
            if (Vector3.Angle(transform.right, controllerToBodyVec) < 90)
            {
                // Show menu on right side
                OpenMenu(true);
            }
            else
            {
                // Show menu on left side
            }
        }
    }

    public void OpenMenu(bool right)
    {
        if (right)
        {
            if (name == "LeftController")
            {
                if (savedInner)
                {
                    savedClothMenuWrap.SetActive(true);
                }
                else
                {
                    historyClothMenuWrap.SetActive(true);
                }
            }
            else
            {
                if (savedInner)
                {
                    historyClothMenuWrap.SetActive(true);
                }
                else
                {
                    savedClothMenuWrap.SetActive(true);
                }
            }
        }
        else
        {
            if (name == "LeftController")
            {
                if (savedInner)
                {
                    historyClothMenuWrap.SetActive(true);
                }
                else
                {
                    savedClothMenuWrap.SetActive(true);
                }
            }
            else
            {
                if (savedInner)
                {
                    savedClothMenuWrap.SetActive(true);
                }
                else
                {
                    historyClothMenuWrap.SetActive(true);
                }
            }
        }
    }

    public IEnumerator ExpandMenu()
    {
        yield return null;
    }
}
