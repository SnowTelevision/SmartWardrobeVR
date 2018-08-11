using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class HeadWearHUD : MonoBehaviour
{
    public Vector3 hudPosition; //The localPosition for the HUD

    public bool isWearingHUD; //Is the player wearing HUD or not
    public GameObject playerHUD; //The HUD game object for the player. This will get assigned after the player wear the HUD for the first time.
    public GameObject playerHUDviewArea; //The view area of the HUD UI, set this active to make the HUD visible
    public Vector3 playerHUD_UIcanvas_position; //The local position that the UI canvas is going to adjust into according to the HUD's relative position to the player's head.

    // Use this for initialization
    void Start()
    {
        isWearingHUD = false;
        //playerHUDviewArea = FindObjectOfType<GameManager>().playerHUDviewArea;
        playerHUD_UIcanvas_position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        //if (isWearingHUD)
        //{
        //    if (!playerHUD.GetComponent<VRTK_InteractableObject>().IsGrabbed())
        //    {
        //        playerHUD.transform.localRotation = Quaternion.identity;
        //        playerHUD.transform.localPosition = hudPosition;
        //    }

        //    //playerHUDviewArea.transform.rotation = transform.rotation;
        //}

        if (isWearingHUD && 
            !GetComponentInParent<VRTK_HeadsetFade>().IsTransitioning() && 
            !playerHUDviewArea.activeInHierarchy &&
            !GetComponentInParent<VRTK_HeadsetFade>().IsFaded()) // If the HUD is on player's head but the UI is not turned on
        {
            playerHUDviewArea.SetActive(true);
        }

        //if (!isWearingHUD && playerHUDviewArea.activeInHierarchy) // If the HUD is not on player's head but the UI is not turned off
        //{
        //    playerHUDviewArea.SetActive(false);
        //}
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.name == "AR_Glasses" && !other.GetComponent<VRTK_InteractableObject>().IsGrabbed() && !isWearingHUD)
        {
            //print("HUD");

            //print(other.GetComponent<FixedJoint>());

            playerHUD = other.gameObject;
            playerHUD.transform.parent = FindObjectOfType<ApplicationManager>().playerBody.transform;
            playerHUD.transform.localRotation = Quaternion.identity;
            playerHUD.transform.localPosition = hudPosition;
            playerHUD.GetComponent<MeshRenderer>().enabled = false;

            playerHUD.GetComponent<Rigidbody>().useGravity = false;
            playerHUD.GetComponent<Rigidbody>().isKinematic = true;

            StartCoroutine(FadeCamera());
            isWearingHUD = true;
        }
    }

    /// <summary>
    /// Fade out then fade in camera when putting on the HUD
    /// </summary>
    public IEnumerator FadeCamera()
    {
        GetComponentInParent<VRTK_HeadsetFade>().Fade(Color.black, 0.5f);
        while (GetComponentInParent<VRTK_HeadsetFade>().IsTransitioning()) // Wait for fading transition
        {
            yield return null;
        }
        GetComponentInParent<VRTK_HeadsetFade>().Unfade(0.5f);
    }

    //public void OnTriggerExit(Collider other)
    //{
    //    if (other.tag == "HUDwhole")
    //    {
    //        //print("HUD exit");

    //        //print(other.GetComponent<FixedJoint>());

    //        playerHUD.transform.parent = null;

    //        playerHUD.GetComponent<Rigidbody>().useGravity = true;
    //        playerHUD.GetComponent<Rigidbody>().isKinematic = false;

    //        isWearingHUD = false;
    //    }
    //}

    public IEnumerator takeOffRoutine()
    {
        yield return new WaitForFixedUpdate();

        playerHUD.transform.parent = null;

        playerHUD.GetComponent<Rigidbody>().useGravity = true;
        playerHUD.GetComponent<Rigidbody>().isKinematic = false;

    }
}
