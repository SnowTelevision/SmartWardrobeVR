using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Control the wardrobe info animation by user's gaze direction
/// </summary>
public class ShowWardrobeInfoOnUserGaze : MonoBehaviour
{
    public CheckIfGazed gazeDetector;
    public Animator animationController;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (gazeDetector.isBeingGazed)
        {
            animationController.SetBool("PlayerEye", true);
        }
        else
        {
            animationController.SetBool("PlayerEye", false);
        }
    }
}
