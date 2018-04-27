using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILookAtPlayer : MonoBehaviour
{
    public bool lookX;
    public bool lookY;
    public bool lookZ;

    public Vector3 originalEuler;
    public Transform playerEye;
    public Vector3 newEuler;

    // Use this for initialization
    void Start()
    {
        originalEuler = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerEye == null)
        {
            playerEye = FindObjectOfType<SteamVR_Camera>().transform;
        }

        transform.LookAt(playerEye);
        newEuler = originalEuler;

        if (lookX)
        {
            newEuler.x = transform.eulerAngles.x;
        }
        if (lookY)
        {
            newEuler.y = transform.eulerAngles.y;
        }
        if (lookZ)
        {
            newEuler.z = transform.eulerAngles.z;
        }

        transform.eulerAngles = newEuler;
    }
}
