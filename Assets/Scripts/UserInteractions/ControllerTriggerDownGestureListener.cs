using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Monitering the controller gesture when the trigger is holding down
/// Show an arrow when the user is making a gesture
/// Store the information of valid gestures
/// If the user's gesture already validates a gesture, then he can only choose to cancel that gesture,
/// if the user wants to do another gesture, he must release the trigger and start over
/// </summary>
public class ControllerTriggerDownGestureListener : MonoBehaviour
{
    public GestureInformation[] gesturesInformation; // The information for all gestures
    public GameObject gestureArrowHead; // The arrow head of the arrow that represents the gesture
    public GameObject gestureArrowLine;
    public Material normalArrowColor; // The color of arrow when it is not a valid gesture
    public Material validArrowColor; // The color of arrow when it is a valid gesture

    public static string lastGesture; // The last gesture the user just finished making
    public Transform userHeadTransform; // The user's head's transform
    public bool hasValidInCurrentGesture; // Has the current gesture matched a valid gesture
    public GestureInformation currentValidatedGesture; // The valid gesture that the user's current gesture matched first
    public bool isMakingGesture; // Is the user currently making a gesture
    public Transform currentGestureController; // The controller that is currently drawing a gesture
    public Vector3 gestureStartWorldPosition; // The world coordinate where the gesture started
    public float currentGestureLength; // The length of the current gesture
    public Vector3 currentGestureVector; // The vector of the current gesture from the start position to the controller's current position
    public static ControllerTriggerDownGestureListener sControllerTriggerDownGestureListener; // The static reference

    // Test
    public Transform testLine;
    public string testLastGesture;

    // Use this for initialization
    void Start()
    {
        sControllerTriggerDownGestureListener = this;
        lastGesture = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (isMakingGesture)
        {
            OnMakingGesture();
        }

        testLastGesture = lastGesture;
    }

    /// <summary>
    /// Update the reference transform for each gesture
    /// </summary>
    public void UpdateGestureReferenceTransform(Transform currentNonUserRefTransform)
    {
        foreach (GestureInformation g in gesturesInformation)
        {
            if (g.refUser)
            {
                g.referenceTransform = userHeadTransform;
            }
            else
            {
                g.referenceTransform = currentNonUserRefTransform;
            }
        }
    }

    /// <summary>
    /// Draw an arrow represents user's current gesture
    /// </summary>
    public void DrawCurrentGesture()
    {
        currentGestureLength = CalculateCurrentGestureLength();
        currentGestureVector = currentGestureController.position - gestureStartWorldPosition;

        gestureArrowLine.transform.position = gestureStartWorldPosition + currentGestureVector / 2f;
        gestureArrowLine.transform.LookAt(currentGestureController);
        gestureArrowLine.transform.localScale = new Vector3(1, 1, currentGestureLength / 2f);
        gestureArrowHead.transform.position = currentGestureController.position;
        gestureArrowHead.transform.rotation = gestureArrowLine.transform.rotation;
        //gestureArrowHead.transform.LookAt(gestureArrowHead.transform.position + gestureArrowHead.transform.forward, ())

        if (lastGesture == "")
        {
            gestureArrowHead.GetComponentInChildren<MeshRenderer>().material = normalArrowColor;
            gestureArrowLine.GetComponentInChildren<MeshRenderer>().material = normalArrowColor;
        }
        else
        {
            gestureArrowHead.GetComponentInChildren<MeshRenderer>().material = validArrowColor;
            gestureArrowLine.GetComponentInChildren<MeshRenderer>().material = validArrowColor;
        }

        if (!gestureArrowHead.activeInHierarchy)
        {
            gestureArrowHead.SetActive(true);
            gestureArrowHead.GetComponentInChildren<MeshRenderer>().material = normalArrowColor;
            gestureArrowLine.SetActive(true);
            gestureArrowLine.GetComponentInChildren<MeshRenderer>().material = normalArrowColor;
        }
        // Test
        //Debug.DrawLine(gestureStartWorldPosition, currentGestureController.position);
        //Debug.DrawLine(Vector3.zero, currentGestureController.position);
        //print(Vector3.Distance(Vector3.zero, currentGestureController.position));
        //testLine.transform.position = gestureStartWorldPosition + currentGestureVector / 2f;
        //testLine.transform.LookAt(currentGestureController);
        //testLine.transform.localScale = new Vector3(0.03f, 0.03f, currentGestureLength);
    }

    /// <summary>
    /// Calculate the length of the user's current gesture
    /// </summary>
    /// <returns></returns>
    public float CalculateCurrentGestureLength()
    {
        float gestureLength = 0;

        gestureLength = Vector3.Distance(currentGestureController.position, gestureStartWorldPosition);

        return gestureLength;
    }

    /// <summary>
    /// Calculate the angle between the current gesture vector and another vector
    /// </summary>
    /// <param name="refWorldVector"></param>
    /// <returns></returns>
    public float CalculateCurrentGestureDirection(Vector3 refWorldVector)
    {
        float angleDiff = 0;

        angleDiff = Vector3.Angle(refWorldVector, currentGestureVector);

        return angleDiff;
    }

    /// <summary>
    /// Check if the current gesture the user is making matches one of the valid gestures
    /// </summary>
    public void CheckGestures()
    {
        GestureInformation newValidGesture = null; // Stores the information of the first valid gesture the user made within the current gesture

        foreach (GestureInformation g in gesturesInformation)
        {
            if (CheckSingleGesture(g) != "")
            {
                newValidGesture = g;
                break;
            }
        }

        if (newValidGesture != null)
        {
            hasValidInCurrentGesture = true;
            currentValidatedGesture = newValidGesture;
        }
    }

    /// <summary>
    /// Check a gesture
    /// </summary>
    /// <param name="g"></param>
    /// <returns></returns>
    public string CheckSingleGesture(GestureInformation g)
    {
        // Positive X (right relate to reference transform)
        if (g.referenceTransform != null && g.alongX && currentGestureLength >= g.validDistance &&
            CalculateCurrentGestureDirection(g.referenceTransform.right) < 30 && g.gestureDirection == 1)
        {
            g.validated = true;
            return g.gestureName;
        }
        // Negative X (left relate to reference transform)
        if (g.referenceTransform != null && g.alongX && currentGestureLength >= g.validDistance &&
            CalculateCurrentGestureDirection(g.referenceTransform.right) > 150 && g.gestureDirection == -1)
        {
            g.validated = true;
            return g.gestureName;
        }
        // Positive Y (up relate to reference transform)
        if (g.referenceTransform != null && g.alongY && currentGestureLength >= g.validDistance &&
            CalculateCurrentGestureDirection(g.referenceTransform.up) < 30 && g.gestureDirection == 1)
        {
            g.validated = true;
            return g.gestureName;
        }
        // Negative Y (down relate to reference transform)
        if (g.referenceTransform != null && g.alongY && currentGestureLength >= g.validDistance &&
            CalculateCurrentGestureDirection(g.referenceTransform.up) > 150 && g.gestureDirection == -1)
        {
            g.validated = true;
            return g.gestureName;
        }
        // Positive Z (forward relate to reference transform)
        if (g.referenceTransform != null && g.alongZ && currentGestureLength >= g.validDistance &&
            CalculateCurrentGestureDirection(g.referenceTransform.forward) < 59 && g.gestureDirection == 1)
        {
            g.validated = true;
            return g.gestureName;
        }
        // Negative Z (backward relate to reference transform)
        if (g.referenceTransform != null && g.alongZ && currentGestureLength >= g.validDistance &&
            CalculateCurrentGestureDirection(g.referenceTransform.forward) > 121 && g.gestureDirection == -1)
        {
            g.validated = true;
            return g.gestureName;
        }

        return "";
    }

    /// <summary>
    /// Runs when the user is making a gesture 
    /// </summary>
    public void OnMakingGesture()
    {
        DrawCurrentGesture();

        if (!hasValidInCurrentGesture)
        {
            CheckGestures();
        }
        else
        {
            lastGesture = CheckSingleGesture(currentValidatedGesture);
        }
    }

    /// <summary>
    /// When the user stop making a gesture
    /// </summary>
    public void GestureStop()
    {
        currentGestureController = null;
        hasValidInCurrentGesture = false;
        currentValidatedGesture = null;
        currentGestureLength = 0;
        lastGesture = "";

        gestureArrowHead.SetActive(false);
        gestureArrowLine.SetActive(false);

        foreach (GestureInformation g in gesturesInformation)
        {
            g.validated = false;
        }

        isMakingGesture = false;
    }

    /// <summary>
    /// When the user start making a gesture
    /// </summary>
    /// <param name="currentNonUserRefTransform"></param>
    public void GestureStart(Transform currentNonUserRefTransform, Transform gestureControllerTransform)
    {
        UpdateGestureReferenceTransform(currentNonUserRefTransform);
        currentGestureController = gestureControllerTransform;
        gestureStartWorldPosition = gestureControllerTransform.position;

        isMakingGesture = true;
    }
}

/// <summary>
/// Stores the informations of a gesture
/// </summary>
[Serializable]
public class GestureInformation
{
    public bool refUser; // Is this gesture relative to the user or a non-user transform?
    public Transform referenceTransform; // The object that this gesture's direction is refered to
    public float validDistance; // How long the gesture needs to be valid
    public bool alongX; // Is this gesture along relative X axis
    public bool alongY; // Is this gesture along relative Y axis
    public bool alongZ; // Is this gesture along relative Z axis
    public int gestureDirection; // The direction of the gesture along the checking axis (-1 or 1)
    public string gestureName; // The name of this gesture

    public bool validated; // Is this gesture validated with the user's current gesture?
}
