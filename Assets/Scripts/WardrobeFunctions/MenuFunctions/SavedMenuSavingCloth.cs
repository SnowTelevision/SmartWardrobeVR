using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class SavedMenuSavingCloth : MonoBehaviour
{
    public GameObject bubble; // The bubble that will wrap around a cloth if it is going to be saved

    //public InteractableClothInfo toBeAdded; // The cloth to be added into the saved menu
    //public List<ClothInfo> collidingCloth; // The cloth that is colliding with the save trigger
    public ClothInfo clothReadyToBeSaved; // The cloth that is ready to be saved into the menu
    public GameObject modelForclothReadyToBeSaved; // The model to show for the cloth to be saved

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

    private void OnTriggerEnter(Collider other)
    {
        // If it is a cloth, and the cloth is released, and is not "weared" on the body yet
        if (other.GetComponent<ClothInfo>())
        {
            ClothInfo collidingClothInfo = other.GetComponent<ClothInfo>();

            if (other.GetComponent<VRTK_InteractableObject>().IsGrabbed())
            {
                if (collidingClothInfo.isRealCloth ||
                    other.GetComponent<InteractableClothInfo>())
                {
                    return;
                }

                other.GetComponent<MeshRenderer>().enabled = false; // Hide the cloth that is grabbing by the user
                bubble.SetActive(true);
                modelForclothReadyToBeSaved = Instantiate(collidingClothInfo.savedItemModel); // Show the saved model for that grabbing cloth
                //modelForclothReadyToBeSaved.transform.localEulerAngles = Vector3.zero;
                clothReadyToBeSaved = collidingClothInfo;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // If it is a cloth, and the cloth is released, and is not "weared" on the body yet
        if (other.GetComponent<ClothInfo>())
        {
            ClothInfo collidingClothInfo = other.GetComponent<ClothInfo>();

            if (collidingClothInfo.isRealCloth ||
                other.GetComponent<InteractableClothInfo>() ||
                clothReadyToBeSaved == null)
            {
                return;
            }
            //print(other.GetComponent<VRTK_InteractableObject>().IsGrabbed());
            if (other.GetComponent<VRTK_InteractableObject>().IsGrabbed())
            {
                // If the cloth is not saved
                if (collidingClothInfo == clothReadyToBeSaved)
                {
                    if (!WardrobeDatabase.database.choseCloth.Exists(
                        c => c.thisClothModel.name == collidingClothInfo.userSavedMenuItemObject.thisClothModel.name))
                    {
                        other.GetComponent<MeshRenderer>().enabled = false; // Hide the cloth that is grabbing by the user
                        modelForclothReadyToBeSaved.transform.position = other.transform.position;
                        BubbleChaseCloth();
                    }
                    else
                    {
                        other.GetComponent<MeshRenderer>().enabled = false; // Hide the cloth that is grabbing by the user
                        modelForclothReadyToBeSaved.transform.position = other.transform.position;
                    }

                    //if (other.GetComponent<DisposableClothInfo>())
                    //{
                    //    if (!WardrobeDatabase.database.choseCloth.Exists(
                    //        c => c.thisClothModel.name == collidingClothInfo.name))
                    //    {
                    //        other.GetComponent<MeshRenderer>().enabled = false; // Hide the cloth that is grabbing by the user
                    //        modelForclothReadyToBeSaved.transform.position = other.transform.position;
                    //        BubbleChaseCloth();
                    //    }
                    //    else
                    //    {
                    //        other.GetComponent<MeshRenderer>().enabled = false; // Hide the cloth that is grabbing by the user
                    //        modelForclothReadyToBeSaved.transform.position = other.transform.position;
                    //    }
                    //}
                }
            }
            if (!other.GetComponent<VRTK_InteractableObject>().IsGrabbed())
            {
                if (collidingClothInfo == clothReadyToBeSaved)
                {
                    if (!WardrobeDatabase.database.choseCloth.Exists(
                        c => c.thisClothModel.name == collidingClothInfo.userSavedMenuItemObject.thisClothModel.name))
                    {
                        StartCoroutine(SavingNewCloth(collidingClothInfo.userSavedMenuItemObject));
                    }
                    else
                    {
                        bubble.SetActive(false);
                        clothReadyToBeSaved = null;
                        Destroy(modelForclothReadyToBeSaved); // Destroy the saved model for that grabbing cloth
                    }

                    //if (other.GetComponent<DisposableClothInfo>())
                    //{
                    //    if (!WardrobeDatabase.database.choseCloth.Exists(
                    //        c => c.thisClothModel.name == collidingClothInfo.userSavedMenuItemObject.thisClothModel.name))
                    //    {
                    //        StartCoroutine(SavingNewCloth(collidingClothInfo.userSavedMenuItemObject));
                    //    }
                    //    else
                    //    {
                    //        bubble.SetActive(false);
                    //        clothReadyToBeSaved = null;
                    //        Destroy(modelForclothReadyToBeSaved); // Destroy the saved model for that grabbing cloth
                    //    }
                    //}
                }
            }
        }
    }

    private void OnDisable()
    {
        if (modelForclothReadyToBeSaved != null)
        {
            bubble.SetActive(false);
            clothReadyToBeSaved = null;
            Destroy(modelForclothReadyToBeSaved); // Destroy the saved model for that grabbing cloth
        }
    }

    /// <summary>
    /// The animation played when saving new cloth
    /// </summary>
    /// <returns></returns>
    public IEnumerator SavingNewCloth(InteractableClothInfo toBeSave)
    {
        Vector3 bubbleInitialPosition = bubble.transform.localPosition;

        for (float t = 0; t < 0.3f; t += Time.deltaTime)
        {
            bubble.transform.localPosition = Vector3.Lerp(bubbleInitialPosition, Vector3.zero, t);
            modelForclothReadyToBeSaved.transform.position = bubble.transform.position;
            yield return null;
        }

        // Bounce menu

        bubble.SetActive(false);
        if (!WardrobeDatabase.database.choseCloth.Exists(
            c => c.thisClothModel.name == toBeSave.thisClothModel.name))
        {
            WardrobeDatabase.database.choseCloth.Add(toBeSave);
        }
        clothReadyToBeSaved = null;
        Destroy(modelForclothReadyToBeSaved); // Destroy the saved model for that grabbing cloth
    }

    /// <summary>
    /// Let the bubble seek the cloth
    /// </summary>
    public void BubbleChaseCloth()
    {
        bubble.transform.LookAt(modelForclothReadyToBeSaved.transform);
        if (Vector3.Distance(bubble.transform.position, modelForclothReadyToBeSaved.transform.position) > 0.01f)
        {
            bubble.GetComponent<Rigidbody>().velocity =
                bubble.transform.forward;// *
            Mathf.Pow(Vector3.Distance(bubble.transform.position, modelForclothReadyToBeSaved.transform.position) * 10,
                      4);
        }
        else
        {
            bubble.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If it is a cloth, and the cloth is released, and is not "weared" on the body yet
        if (other.GetComponent<ClothInfo>())
        {
            ClothInfo collidingClothInfo = other.GetComponent<ClothInfo>();

            if (collidingClothInfo == clothReadyToBeSaved)
            {
                if (other.GetComponent<VRTK_InteractableObject>().IsGrabbed())
                {
                    if (collidingClothInfo.isRealCloth ||
                        other.GetComponent<InteractableClothInfo>())
                    {
                        return;
                    }

                    other.GetComponent<MeshRenderer>().enabled = true; // Show the cloth that is grabbing by the user
                    bubble.SetActive(false);
                    bubble.transform.localPosition = Vector3.zero;
                    Destroy(modelForclothReadyToBeSaved); // Destroy the saved model for that grabbing cloth
                    clothReadyToBeSaved = null;
                }
                //else
                //{
                //    other.GetComponent<MeshRenderer>().enabled = true; // Show the cloth that is grabbing by the user
                //    bubble.SetActive(false);
                //    Destroy(modelForclothReadyToBeSaved); // Destroy the saved model for that grabbing cloth
                //    clothReadyToBeSaved = null;
                //}
            }
        }
    }
}
