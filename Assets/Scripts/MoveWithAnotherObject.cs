using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithAnotherObject : MonoBehaviour
{
    public Transform objectToFollow; // Which object to follow
    public bool followLocalPosi; // Does this move in followed object's local coordinate
    public bool followLocalEuler; // Does this rotate in followed object's local coordinate
    public bool followPosiX; // Follow the X coordinate?
    public bool followPosiY; // Follow the Y coordinate?
    public bool followPosiZ; // Follow the Z coordinate?
    public bool followEulerX; // Follow the X euler angle?
    public bool followEulerY; // Follow the Y euler angle?
    public bool followEulerZ; // Follow the Z euler angle?
    public bool revertEulerX; // Does this reverse the followed object's rotation
    public bool revertEulerY; // Does this reverse the followed object's rotation
    public bool revertEulerZ; // Does this reverse the followed object's rotation
    public Vector3 positionOffset; // The offset for position
    public Vector3 eulerAngleOffset; // The offset for eulerAngle;

    public Vector3 newPosition; // The position for next frame;
    public Vector3 newEulerAngle; // The eulerAngle for next frame;

    // Use this for initialization
    void Start()
    {
        if (eulerAngleOffset == Vector3.zero)
        {
            eulerAngleOffset = transform.eulerAngles;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If this is the mirrored object
        if (GetComponent<MirrorWorldObject>())
        {
            return;
        }

        FollowPosition();
        FollowEulerAngle();
    }

    /// <summary>
    /// Move the object's position
    /// </summary>
    public void FollowPosition()
    {
        if (followLocalPosi)
        {
            newPosition = objectToFollow.TransformPoint(positionOffset);
        }
        else
        {
            newPosition = objectToFollow.position + positionOffset;
        }

        if (!followPosiX)
        {
            newPosition.x = transform.position.x;
        }
        if (!followPosiY)
        {
            newPosition.y = transform.position.y;
        }
        if (!followPosiZ)
        {
            newPosition.z = transform.position.z;
        }

        transform.position = newPosition;
    }

    /// <summary>
    /// Move the object's euler angles
    /// </summary>
    public void FollowEulerAngle()
    {
        if (followLocalEuler)
        {
            //if (!revertEuler)
            {
                newEulerAngle = objectToFollow.localEulerAngles + eulerAngleOffset;
            }
            //else
            //{
            //    newEulerAngle = -objectToFollow.localEulerAngles + eulerAngleOffset;
            //}
        }
        else
        {
            //if (!revertEuler)
            {
                newEulerAngle = objectToFollow.eulerAngles + eulerAngleOffset;
            }
            //else
            //{
            //    newEulerAngle = -objectToFollow.eulerAngles + eulerAngleOffset;
            //}
        }

        if (revertEulerX)
        {
            newEulerAngle.x += 180;
        }
        if (revertEulerY)
        {
            newEulerAngle.y += 180;
        }
        if (revertEulerZ)
        {
            newEulerAngle.z += 180;
        }

        if (!followEulerX)
        {
            newEulerAngle.x = transform.eulerAngles.x;
        }
        if (!followEulerY)
        {
            newEulerAngle.y = transform.eulerAngles.y;
        }
        if (!followEulerZ)
        {
            newEulerAngle.z = transform.eulerAngles.z;
        }

        transform.eulerAngles = newEulerAngle;
    }
}
