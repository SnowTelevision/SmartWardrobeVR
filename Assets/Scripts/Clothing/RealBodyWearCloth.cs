using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class RealBodyWearCloth : MonoBehaviour
{


    public List<ClothInfo> clothes; // The cloth(es) currently wearing
    public ClothInfo currentVirtualCloth; // The virtual cloth that is currently trying on in the mirror

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        // If it is a cloth, and the cloth is released, and is not "weared" on the body yet
        if (other.GetComponent<ClothInfo>())
        {
            other.GetComponent<ClothInfo>().isTouchingUserBody = true;
            ClothInfo collidingClothInfo = other.GetComponent<ClothInfo>();

            if (!other.GetComponent<VRTK_InteractableObject>().IsGrabbed())
            {
                if (collidingClothInfo.isRealCloth && clothes.Contains(collidingClothInfo))
                {
                    return;
                }
                else if (!collidingClothInfo.isRealCloth && currentVirtualCloth == collidingClothInfo)
                {
                    return;
                }

                WearCloth(other.GetComponent<ClothInfo>());
            }
        }
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
                currentVirtualCloth.GetComponent<ClothInfo>().ReturnToMenuWrap();
            }

            currentVirtualCloth = newCloth;
            newCloth.transform.parent = gameObject.transform;
            newCloth.GetComponent<Rigidbody>().isKinematic = true;
            newCloth.GetComponent<Rigidbody>().velocity = Vector3.zero;
            newCloth.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            newCloth.isWeared = true;
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

    private void OnTriggerExit(Collider other)
    {
        // If it is a cloth, and is "weared" on the body yet
        if (other.GetComponent<ClothInfo>())
        {
            other.GetComponent<ClothInfo>().isTouchingUserBody = false;

            if (clothes.Contains(other.GetComponent<ClothInfo>()) || 
                currentVirtualCloth == other.GetComponent<ClothInfo>())
            {
                TakeOffCloth(other.GetComponent<ClothInfo>());
            }
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
