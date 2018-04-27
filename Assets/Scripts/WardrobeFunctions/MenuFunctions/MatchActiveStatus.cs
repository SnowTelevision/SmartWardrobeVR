using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchActiveStatus : MonoBehaviour
{
    public GameObject matchee;
    public GameObject matcher;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (matchee.activeInHierarchy)
        {
            if (!matcher.activeInHierarchy)
            {
                matcher.SetActive(true);
            }
        }
        else
        {
            if (matcher.activeInHierarchy)
            {
                matcher.SetActive(false);
            }
        }
    }
}
