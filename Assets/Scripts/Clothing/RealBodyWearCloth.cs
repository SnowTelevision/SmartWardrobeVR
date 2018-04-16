using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class RealBodyWearCloth : MonoBehaviour
{


    public List<ClothInfo> clothes; // The cloth(es) currently wearing
    public ClothInfo currentVirtualCloth; // The virtual cloth that is currently trying on in the mirror
    public ClothInfo toBeWeared; // The cloth to be weared
    public List<ClothInfo> collidingCloth; // The cloth that is colliding with the save trigger

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Keep the wearing cloth's original scale
        if (currentVirtualCloth != null)
        {
            currentVirtualCloth.transform.localScale = currentVirtualCloth.originalScale;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Add each new cloth into the colliding list
        if (other.GetComponent<ClothInfo>() && !collidingCloth.Contains(other.GetComponent<ClothInfo>()))
        {
            collidingCloth.Add(other.GetComponent<ClothInfo>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If it is a cloth, and is "weared" on the body
        if (other.GetComponent<ClothInfo>())
        {
            collidingCloth.Remove(other.GetComponent<ClothInfo>());

            other.GetComponent<ClothInfo>().isTouchingUserBody = false;

            if (clothes.Contains(other.GetComponent<ClothInfo>()) ||
                currentVirtualCloth == other.GetComponent<ClothInfo>())
            {
                TakeOffCloth(other.GetComponent<ClothInfo>());
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        // If it is a cloth, and the cloth is released, and is not "weared" on the body yet
        if (other.GetComponent<ClothInfo>())
        {
            other.GetComponent<ClothInfo>().isTouchingUserBody = true;
            ClothInfo collidingClothInfo = other.GetComponent<ClothInfo>();
            //print("A, " + other.name);

            if (!other.GetComponent<VRTK_InteractableObject>().IsGrabbed())
            {
                if (collidingClothInfo.isRealCloth && clothes.Contains(collidingClothInfo))
                {
                    return;
                }
                else if (!collidingClothInfo.isRealCloth && currentVirtualCloth == collidingClothInfo)
                {
                    if (!currentVirtualCloth.freelyWear)
                    {
                        RepositionCloth(collidingClothInfo);
                    }
                    return;
                }
                //print("B, " + other.name);

                if (!collidingClothInfo.isRealCloth)
                {
                    // Add cloth into history if it is not already in there
                    if (collidingClothInfo.userHistoryMenuItemObject != null &&
                        !WardrobeDatabase.database.tryHistory.Exists(
                        c => c.thisClothModel.name == collidingClothInfo.userHistoryMenuItemObject.thisClothModel.name))
                    {
                        WardrobeDatabase.database.tryHistory.Add(collidingClothInfo.userHistoryMenuItemObject);
                    }
                }

                //print(other.GetComponent<ClothInfo>().name);
                WearCloth(other.GetComponent<ClothInfo>());
                //toBeWeared = other.GetComponent<ClothInfo>();
            }
            //else
            //{
            //    //foreach (ClothInfo c in collidingCloth)
            //    {
            //        //print(toBeWeared.name + ", " + other.GetComponent<ClothInfo>().name);
            //        // Wear the cloth if it is ungrabbed within the wear area
            //        if (toBeWeared != null && toBeWeared.name == other.GetComponent<ClothInfo>().name)
            //        {
            //            WearCloth(other.GetComponent<ClothInfo>());
            //            //print("wear cloth: " + other.name);
            //            toBeWeared = null;
            //        }
            //    }
            //}
        }
    }

    /// <summary>
    /// Reposition cloth to right position if it is already weared and is not free wear
    /// </summary>
    public void RepositionCloth(ClothInfo cloth)
    {
        cloth.transform.localPosition = cloth.clothLocalPosition;
        cloth.transform.localEulerAngles = cloth.clothLocalEulerAngles;
    }

    /// <summary>
    /// Wearing a cloth on the body
    /// </summary>
    /// <param name="newCloth"></param>
    public void WearCloth(ClothInfo newCloth)
    {
        if (newCloth.isRealCloth)
        {
            clothes.Add(newCloth);
            newCloth.transform.parent = gameObject.transform;
            newCloth.GetComponent<Rigidbody>().useGravity = false;
            newCloth.GetComponent<Rigidbody>().isKinematic = true;
            newCloth.GetComponent<Rigidbody>().velocity = Vector3.zero;
            newCloth.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            newCloth.isWeared = true;
        }
        else
        {
            if (currentVirtualCloth != null)
            {
                currentVirtualCloth.GetComponent<Collider>().enabled = false;

                // Only return the cloth if it is from the wardrobe menu and not from the hand history or saved menu
                if (!currentVirtualCloth.GetComponent<DisposableClothInfo>())
                {
                    currentVirtualCloth.GetComponent<ClothInfo>().ReturnToMenuWrap();
                }
                else
                {
                    Destroy(currentVirtualCloth.gameObject);
                }
            }

            currentVirtualCloth = newCloth;
            newCloth.transform.parent = gameObject.transform;
            newCloth.GetComponent<Rigidbody>().isKinematic = true;
            newCloth.GetComponent<Rigidbody>().velocity = Vector3.zero;
            newCloth.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            newCloth.isWeared = true;
            //print(newCloth.name + " is weared");
            newCloth.GetComponent<MeshRenderer>().enabled = false;
            newCloth.transform.localScale = newCloth.originalScale;
            //newCloth.GetComponent<Collider>().enabled = false;
        }

        if (!newCloth.freelyWear)
        {
            newCloth.transform.localPosition = newCloth.clothLocalPosition;
            newCloth.transform.localEulerAngles = newCloth.clothLocalEulerAngles;
        }
    }

    /// <summary>
    /// Take of a cloth from the body
    /// </summary>
    /// <param name="thisCloth"></param>
    public void TakeOffCloth(ClothInfo thisCloth)
    {
        clothes.Remove(thisCloth);
        thisCloth.isWeared = false;

        if (thisCloth.isRealCloth)
        {
            thisCloth.transform.parent = null;
            thisCloth.GetComponent<Rigidbody>().useGravity = true;
            thisCloth.GetComponent<Rigidbody>().isKinematic = false;
        }
        else
        {
            thisCloth.GetComponent<MeshRenderer>().enabled = true;
            thisCloth.transform.localScale = thisCloth.originalScale;
            currentVirtualCloth = null;
        }
    }
}
