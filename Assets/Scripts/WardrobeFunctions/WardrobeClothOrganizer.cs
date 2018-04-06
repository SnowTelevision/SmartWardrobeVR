using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

/// <summary>
/// This script should be attached to every region of the wardrobe
/// </summary>
public class WardrobeClothOrganizer : MonoBehaviour
{
    public GameObject highLight; // The highlight gameobject for this wardrobe region

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
        // If it is a cloth, and the cloth is released, and it's not already "put in" this wardrobe region
        if (other.GetComponent<ClothInfo>() && !other.GetComponent<VRTK_InteractableObject>().IsGrabbed() && other.GetComponent<ClothInfo>().clothLocation != this)
        {
            PutInCloth(other.GetComponent<ClothInfo>());

            //// If the wardrobe database currently doesn't have this cloth's info (the cloth is put in the wardrobe for the first time
            //if (!WardrobeDatabase.database.storedClothInfo.Contains(other.GetComponent<ClothInfo>()))
            //{
            //    WardrobeDatabase.database.storedClothInfo.Add(other.GetComponent<ClothInfo>()); // Adds the cloth info in the database
            //}
        }
    }

    /// <summary>
    /// Taking cloth outside the wardrobe region
    /// </summary>
    /// <param name="cloth"></param>
    public void TakeOutCloth(ClothInfo cloth)
    {
        cloth.clothLocation = null;
        cloth.GetComponent<Rigidbody>().useGravity = true;
        cloth.GetComponent<Rigidbody>().isKinematic = false;
    }

    /// <summary>
    /// Putting cloth inside the wardrobe region
    /// </summary>
    /// <param name="cloth"></param>
    public void PutInCloth(ClothInfo cloth)
    {
        cloth.clothLocation = this;
        cloth.GetComponent<Rigidbody>().useGravity = false;
        cloth.GetComponent<Rigidbody>().isKinematic = true;
        cloth.GetComponent<Rigidbody>().velocity = Vector3.zero;
        cloth.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}
