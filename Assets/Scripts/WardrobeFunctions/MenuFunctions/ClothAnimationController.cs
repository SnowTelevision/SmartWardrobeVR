using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the UI animations on the clothes in the first and second level menu
/// </summary>
public class ClothAnimationController : MonoBehaviour
{
    public CheckIfGazed firstLevelGazeDetector;
    public TryOnCloth secondLevelMenu;
    public Animator animationController;
    public RealBodyWearCloth body;
    public GameObject thisVirtualCloth;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (secondLevelMenu != null) // If this cloth is in second level menu
        {
            if (secondLevelMenu.currentFacingCloth != null)
            {
                if (secondLevelMenu.currentFacingCloth == gameObject && body.currentVirtualCloth != thisVirtualCloth.GetComponent<ClothInfo>())
                {
                    animationController.SetBool("Play", true);
                }
                else
                {
                    animationController.SetBool("Play", false);
                }
            }
        }

        if (firstLevelGazeDetector != null) // If this cloth is in first level menu
        {
            if (firstLevelGazeDetector.isBeingGazed)
            {
                animationController.SetBool("Play", true);
            }
            else
            {
                animationController.SetBool("Play", false);
            }
        }
    }
}
