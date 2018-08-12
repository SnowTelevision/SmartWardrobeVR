using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

/// <summary>
/// Controls the AR glasses's behavior when it is not weared by the user
/// </summary>
public class AR_GlassesBehavior : MonoBehaviour
{
    public Transform playerEye;
    public Rigidbody arGlassesFollowee; // The object the glasses will be attached with a joint
    public Vector3 jointConnectedAnchor;
    public float minSpring;
    public float minDamper;
    public float springFactor;
    public float damperFactor;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<VRTK_InteractableObject>().IsGrabbed())
        {
            if (GetComponent<SpringJoint>())
            {
                Destroy(GetComponent<SpringJoint>());
            }
        }
        else // If it is not grabbed
        {
            transform.LookAt(playerEye);
            transform.eulerAngles += new Vector3(0, 45, 0);

            if (!GetComponent<SpringJoint>())
            {
                gameObject.AddComponent<SpringJoint>();
                GetComponent<SpringJoint>().connectedBody = arGlassesFollowee;
                GetComponent<SpringJoint>().autoConfigureConnectedAnchor = false;
                GetComponent<SpringJoint>().anchor = Vector3.zero;
                GetComponent<SpringJoint>().connectedAnchor = jointConnectedAnchor;
            }

            UpdateSpringJoint();
        }
    }

    /// <summary>
    /// Calculate the spring and damper for the spring joint
    /// </summary>
    public void UpdateSpringJoint()
    {
        float dist = Vector3.Distance(transform.position, arGlassesFollowee.position);
        GetComponent<SpringJoint>().spring = minSpring + dist * springFactor;
        GetComponent<SpringJoint>().damper = minDamper + dist * damperFactor;
    }
}
