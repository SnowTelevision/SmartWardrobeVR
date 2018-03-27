using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothInfo : MonoBehaviour
{
    public bool isRealCloth; // If the cloth is a "real" cloth that's in the "real world", or a "virtual" one that's in the virtual menu
    public Vector3 clothLocalPosition; // Where the cloth should be relate to the body part
    public Vector3 clothLocalEulerAngles; // What's the cloth's rotation relate to the body part
    public bool freelyWear; // Does the cloth has a fixed position on the body, or can be put on any part of the body
    public string clothName; // Name of the cloth

    public bool isWeared; // If the cloth is currently weared
    public WardrobeClothOrganizer clothLocation; // Where is the cloth put inside the wardrobe? (If it is not in the wardrobe then this will be null)
    public GameObject virtualAvatar; // The gameobject for its virtual model in the wardrobe database

    // Use this for initialization
    void Start()
    {
        gameObject.AddComponent<RealWorldObject>();
        GetComponent<RealWorldObject>().alwaysMirror = false;

        // If it is a "real" cloth, then add the RealWorldObject component
        //if (isRealCloth)
        //{
        //    gameObject.AddComponent<RealWorldObject>();
        //    GetComponent<RealWorldObject>().alwaysMirror = false;

        //    // If the cloth doesn't already have a model for its virtual avatar
        //    //if (virtualAvatar == null)
        //    //{
        //    //    virtualAvatar = Instantiate(gameObject);
        //    //    virtualAvatar.GetComponent<ClothInfo>().isRealCloth = false;
        //    //}
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (isRealCloth && !isWeared && transform.parent != null)
        {
            transform.parent = null;
        }
    }
}
