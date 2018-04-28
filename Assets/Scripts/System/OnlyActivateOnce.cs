using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyActivateOnce : MonoBehaviour
{


    public bool hasOpened;

    // Use this for initialization
    void Start()
    {
        hasOpened = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(hasOpened)
        {
            gameObject.SetActive(false);
        }
    }
}
