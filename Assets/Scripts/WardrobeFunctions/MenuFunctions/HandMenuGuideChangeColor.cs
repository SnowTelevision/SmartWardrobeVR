using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Change the ring's color when they match together
/// </summary>
public class HandMenuGuideChangeColor : MonoBehaviour
{
    public GameObject outerRing;
    public GameObject innerRing;
    public GameObject outerRingValid;
    public GameObject innerRingValid;
    public bool isLeftController;

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
        if ((isLeftController && other.transform.parent.parent.parent.name == "LeftController") ||
            (!isLeftController && other.transform.parent.parent.parent.name == "RightController"))
        {
            if (other.name == "InsideTrigger")
            {
                outerRing.SetActive(false);
                innerRing.SetActive(false);
                outerRingValid.SetActive(true);
                innerRingValid.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((isLeftController && other.transform.parent.parent.parent.name == "LeftController") ||
            (!isLeftController && other.transform.parent.parent.parent.name == "RightController"))
        {
            if (other.name == "InsideTrigger")
            {
                outerRing.SetActive(true);
                innerRing.SetActive(true);
                outerRingValid.SetActive(false);
                innerRingValid.SetActive(false);
            }
        }
    }
}
