using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardrobeDatabase : MonoBehaviour
{


    public List<StoredClothInfo> storedClothInfo; // All the cloth that has their info stored in the wardrobe database
    public static WardrobeDatabase database; // The static reference of the database
    public List<InteractableClothInfo> tryHistory; // What are the clothes that the user have tried
    public List<InteractableClothInfo> choseCloth; // The clothes that the user selected to be highlighted in the wardrobe

    // Use this for initialization
    void Start()
    {
        database = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

/// <summary>
/// Stores the information of a cloth stored in the wardrobe in the database,
/// the clothInfo attribute should be the virtual cloth's ClothInfo
/// </summary>
[Serializable]
public class StoredClothInfo
{
    public ClothInfo clothInfo;
    // public Vector3 clothPositionInWardrobe; // The cloth's position in the wardrobe
    public Color clothMarkerColor; // The marker color of the cloth

    public bool isInWardrobe; // Is this cloth currently in the wardrobe?
}
