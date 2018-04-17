using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardrobeDatabase : MonoBehaviour
{
    public List<WardrobeClothOrganizer> clothLocations; // Different locations that store clothes
    public Transform leftDoor;
    public Transform rightDoor;
    public Coroutine rotateDoorCoroutine;
    public float rotateDoorDuration;
    public float openDoorAngle;

    public List<StoredClothInfo> storedClothInfo; // All the cloth that has their info stored in the wardrobe database
    public static WardrobeDatabase database; // The static reference of the database
    public List<InteractableClothInfo> tryHistory; // What are the clothes that the user have tried
    public List<InteractableClothInfo> choseCloth; // The clothes that the user selected to be highlighted in the wardrobe
    public bool isOpeningDoor;
    public bool isClosingDoor;

    // Use this for initialization
    void Start()
    {
        database = this;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSavedClothHighlights();

        if (choseCloth.Count > 0 && !isOpeningDoor)
        {
            isOpeningDoor = true;
            isClosingDoor = false;
            if (rotateDoorCoroutine != null)
            {
                StopCoroutine(rotateDoorCoroutine);
            }
            rotateDoorCoroutine = StartCoroutine(RotateDoor(true));
        }
        else if (choseCloth.Count == 0 && !isClosingDoor)
        {
            isOpeningDoor = false;
            isClosingDoor = true;
            if (rotateDoorCoroutine != null)
            {
                StopCoroutine(rotateDoorCoroutine);
            }
            rotateDoorCoroutine = StartCoroutine(RotateDoor(false));
        }
    }

    public IEnumerator RotateDoor(bool open)
    {
        if (open)
        {
            Vector3 leftDoorInitialEuler = leftDoor.eulerAngles;
            Vector3 leftDoorTargetEuler = Vector3.zero;
            leftDoorTargetEuler.y = openDoorAngle;

            for (float t = 0; t < 1; 
                 t += Time.deltaTime / rotateDoorDuration / ((openDoorAngle - leftDoor.eulerAngles.y) / openDoorAngle))
            {
                leftDoor.eulerAngles = Vector3.Lerp(leftDoorInitialEuler, leftDoorTargetEuler, t);
                rightDoor.eulerAngles = Vector3.Lerp(-leftDoorInitialEuler, -leftDoorTargetEuler, t);
                yield return null;
            }

            leftDoor.eulerAngles = leftDoorTargetEuler;
            rightDoor.eulerAngles = -leftDoorTargetEuler;
        }
        else
        {
            Vector3 leftDoorInitialEuler = leftDoor.eulerAngles;
            Vector3 leftDoorTargetEuler = Vector3.zero;

            for (float t = 0; t < 1; 
                 t += Time.deltaTime / rotateDoorDuration / (leftDoor.eulerAngles.y / openDoorAngle))
            {
                leftDoor.eulerAngles = Vector3.Lerp(leftDoorInitialEuler, leftDoorTargetEuler, t);
                rightDoor.eulerAngles = Vector3.Lerp(-leftDoorInitialEuler, -leftDoorTargetEuler, t);
                yield return null;
            }

            leftDoor.eulerAngles = leftDoorTargetEuler;
            rightDoor.eulerAngles = -leftDoorTargetEuler;
        }

        rotateDoorCoroutine = null;
    }

    public void UpdateSavedClothHighlights()
    {
        foreach (StoredClothInfo sC in storedClothInfo)
        {
            sC.isSaved = false;
            if (sC.clothInfo == null)
            {
                continue;
            }

            if (choseCloth.Exists(
                c => c.name == sC.clothInfo.name))
            {
                sC.isSaved = true;
            }
        }

        foreach (WardrobeClothOrganizer wO in clothLocations)
        {
            wO.highLight.SetActive(false);
            foreach (StoredClothInfo sC in storedClothInfo)
            {
                if (sC.isSaved &&
                    sC.clothMarkerColor == wO.highLight.GetComponent<MeshRenderer>().material.color)//.r == wO.highLight.GetComponent<MeshRenderer>().material.color.r &&
                                                                                                    //GetComponent<MeshRenderer>().material.color.g == wO.highLight.GetComponent<MeshRenderer>().material.color.g &&
                                                                                                    //GetComponent<MeshRenderer>().material.color.b == wO.highLight.GetComponent<MeshRenderer>().material.color.b)
                {
                    wO.highLight.SetActive(true);
                }
            }
        }
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
    public bool isSaved; // Is this cloth saved for take out of the wardrobe later
}
