using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class SavedMenuSavingCloth : MonoBehaviour
{
    public InteractableClothInfo toBeAdded; // The cloth to be added into the saved menu
    public List<ClothInfo> collidingCloth; // The cloth that is colliding with the save trigger

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    // Add each new cloth into the colliding list
    //    if (other.GetComponent<ClothInfo>() && !collidingCloth.Contains(other.GetComponent<ClothInfo>()))
    //    {
    //        collidingCloth.Add(other.GetComponent<ClothInfo>());
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.GetComponent<ClothInfo>())
    //    {
    //        collidingCloth.Remove(other.GetComponent<ClothInfo>());
    //    }
    //}

    private void OnTriggerStay(Collider other)
    {
        // If it is a cloth, and the cloth is released, and is not "weared" on the body yet
        if (other.GetComponent<ClothInfo>())
        {
            other.GetComponent<ClothInfo>().isTouchingUserBody = true;
            ClothInfo collidingClothInfo = other.GetComponent<ClothInfo>();

            if (!other.GetComponent<VRTK_InteractableObject>().IsGrabbed())
            {
                if (collidingClothInfo.isRealCloth ||
                    other.GetComponent<DisposableClothInfo>() ||
                    other.GetComponent<InteractableClothInfo>())
                {
                    return;
                }
                // Save cloth into to be added if it is not already in there, and add it when it is ungrabbed
                if (!WardrobeDatabase.database.choseCloth.Exists(
                    c => c.thisClothModel.name == collidingClothInfo.userListMenuItemObject.thisClothModel.name))
                {
                    WardrobeDatabase.database.choseCloth.Add(collidingClothInfo.userListMenuItemObject);
                    //toBeAdded = collidingClothInfo.userListMenuItemObject;
                }
            }
            //else
            //{
            //    //foreach (ClothInfo c in collidingCloth)
            //    {
            //        // Add the cloth into saved menu if it is ungrabbed within the trigger area
            //        if (toBeAdded != null && toBeAdded == collidingClothInfo.userListMenuItemObject)
            //        {
            //            WardrobeDatabase.database.choseCloth.Add(collidingClothInfo.userListMenuItemObject);
            //            toBeAdded = null;
            //        }
            //    }
            //}
        }
    }
}
