using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HologramDisplayer : MonoBehaviour
{
    public Transform wheel;
    public GameObject wheelHandle; // The arc handle for the wheel

    public static GameObject currentHologram;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (currentHologram == null)
        {
            wheel.gameObject.SetActive(false);
            wheelHandle.SetActive(false);
        }
        else
        {
            wheel.gameObject.SetActive(true);
            wheelHandle.SetActive(true);
            transform.rotation = wheel.rotation;
            currentHologram.transform.position = transform.position;
            currentHologram.transform.rotation = transform.rotation;
        }
    }
}
