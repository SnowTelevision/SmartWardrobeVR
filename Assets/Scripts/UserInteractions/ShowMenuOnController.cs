using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMenuOnController : MonoBehaviour
{
    public GameObject controllerMenu; // The UI canvas gameobject that has the controller menu

    public GameObject userHead; // The head object of the user

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
    }

    /// <summary>
    /// Checks the relative rotation of the controller with the head
    /// </summary>
    public void CheckControllerRelativeRotation()
    {
        //print("up: " + Vector3.Angle(userHead.transform.up, transform.up) + ", forward: " + Vector3.Angle(userHead.transform.forward, transform.forward));

        // If the controller is upside-down relative to the head and is pointing to the head's relative forward direction
        if (Vector3.Angle(userHead.transform.up, transform.up) >= 150 &&
            Vector3.Angle(userHead.transform.forward, transform.forward) <= 30)
        {
            controllerMenu.SetActive(true);
        }
        else
        {
            controllerMenu.SetActive(false);
        }
    }
}
