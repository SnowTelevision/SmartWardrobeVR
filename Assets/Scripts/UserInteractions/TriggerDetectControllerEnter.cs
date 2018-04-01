using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class TriggerDetectControllerEnter : MonoBehaviour
{


    public GameObject lastEnteredController; // The last controller that enters the trigger
    public float lastControllerEnterTime; // The last time the controller enters the trigger
    public GameObject lastExitedController; // The last controller that exits the trigger
    public float lastControllerExitTime; // The last time the controller exits the trigger

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        // Test
        //print(other.name);

        // If it is a controller
        if (other.transform.parent.name == "[VRTK][AUTOGEN][Controller][CollidersContainer]")
        {
            lastEnteredController = other.transform.parent.gameObject;
            lastControllerEnterTime = Time.time;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If it is a controller
        if (other.transform.parent.name == "[VRTK][AUTOGEN][Controller][CollidersContainer]" &&
            ControllerEventsListener.triggerClickedDown)
        {
            lastExitedController = other.transform.parent.gameObject;
            lastControllerExitTime = Time.time;
        }
    }
}
