using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class RealBodyWearCloth : MonoBehaviour
{


    public List<ClothInfo> clothes; // The cloth(es) currently wearing

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
        if (other.GetComponent<ClothInfo>() && !other.GetComponent<VRTK_InteractableObject>().IsGrabbed() && !clothes.Contains(other.GetComponent<ClothInfo>()))
        {
            WearCloth(other.GetComponent<ClothInfo>());
        }
    }

    /// <summary>
    /// Wearing a cloth on the body
    /// </summary>
    /// <param name="newCloth"></param>
    public void WearCloth(ClothInfo newCloth)
    {
        clothes.Add(newCloth);
        newCloth.transform.parent = gameObject.transform;
        newCloth.GetComponent<Rigidbody>().useGravity = false;
        newCloth.GetComponent<Rigidbody>().isKinematic = true;
        newCloth.GetComponent<Rigidbody>().velocity = Vector3.zero;
        newCloth.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        newCloth.isWeared = true;

        if (!newCloth.freelyWear)
        {
            newCloth.transform.localPosition = newCloth.clothLocalPosition;
            newCloth.transform.localEulerAngles = newCloth.clothLocalEulerAngles;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If it is a cloth, and is "weared" on the body yet
        if (other.GetComponent<ClothInfo>() && clothes.Contains(other.GetComponent<ClothInfo>()))
        {
            TakeOffCloth(other.GetComponent<ClothInfo>());
        }
    }

    /// <summary>
    /// Take of a cloth from the body
    /// </summary>
    /// <param name="thisCloth"></param>
    public void TakeOffCloth(ClothInfo thisCloth)
    {
        clothes.Remove(thisCloth);
        thisCloth.transform.parent = null;
        thisCloth.GetComponent<Rigidbody>().useGravity = true;
        thisCloth.GetComponent<Rigidbody>().isKinematic = false;
        thisCloth.isWeared = false;
    }
}
