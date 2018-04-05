using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TryOnCloth : MonoBehaviour
{
    public Transform secondMenuWheel; // The wheel for second menu
    public Vector3 secondMenuWheelRelativePosition; // The relative position of the second level wheel to the user

    public GameObject currentFacingCloth; // The cloth that is currently in front of the user
    public GameObject currentTryOnCloth; // The cloth that the user is currently trying out
    public GameObject currentTryOnClothOnMenu; // The cloth in the menu that is currently tried on by the user
    public Vector3 secondMenuWheelLastEuler; // What's the euler angles of second menu wheel in the last frame
    public Vector3 userBodyLastEuler; // What's the euler angles of the user body in the last frame

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //secondMenuWheel.position = GetComponent<OrganizeMenuItems>().objectToFollow.TransformPoint(secondMenuWheelRelativePosition);
        RotateFacingItemWithWheel();
    }

    /// <summary>
    /// Rotates the second menu item that is currently facing the user when the user turns the second menu wheel
    /// </summary>
    public void RotateFacingItemWithWheel()
    {
        currentFacingCloth.transform.localEulerAngles +=
            (secondMenuWheel.eulerAngles - secondMenuWheelLastEuler) -
            (GetComponent<OrganizeMenuItems>().objectToFollow.eulerAngles - userBodyLastEuler);

        secondMenuWheelLastEuler = secondMenuWheel.eulerAngles;
        userBodyLastEuler = GetComponent<OrganizeMenuItems>().objectToFollow.eulerAngles;
    }
}
