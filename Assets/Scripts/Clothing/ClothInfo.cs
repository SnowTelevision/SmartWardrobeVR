using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ClothInfo : VRTK_InteractableObject
{
    public bool isRealCloth; // If the cloth is a "real" cloth that's in the "real world", or a "virtual" one that's in the virtual menu
    public Vector3 clothLocalPosition; // Where the cloth should be relate to the body part
    public Vector3 clothLocalEulerAngles; // What's the cloth's rotation relate to the body part
    public bool freelyWear; // Does the cloth has a fixed position on the body, or can be put on any part of the body
    public string clothName; // Name of the cloth
    public Transform belongingItemWrap; // The "matching" second level menu item for this unreal cloth
    public TryOnCloth secondMenuWrapTryOn; // The TryOnCloth attached to its second level menu wrap
    public InteractableClothInfo userListMenuItemObject; // The version of this object to be put into the saved or history menu

    public bool isWeared; // If the cloth is currently weared
    public bool isTouchingUserBody; // If the cloth is currently touching the user's body and will be weared if the user ungrab it
    public WardrobeClothOrganizer clothLocation; // Where is the cloth put inside the wardrobe? (If it is not in the wardrobe then this will be null)
    public GameObject virtualAvatar; // The gameobject for its virtual model in the wardrobe database
    public Vector3 originalScale; // The scale the cloth should keeps
    public Coroutine returningMenuCoroutine; // The animation that is returning menu

    // Use this for initialization
    public virtual void Start()
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
    protected override void Update()
    {
        if (isRealCloth && !isWeared && transform.parent != null)
        {
            transform.parent = null;
        }
        //else if (!isRealCloth)
        //{
        //    if (!isWeared)
        //    {

        //    }
        //}

        base.Update();
    }

    public void BaseOnInteractableObjectGrabbed(InteractableObjectEventArgs e)
    {
        base.OnInteractableObjectGrabbed(e);
    }

    public override void OnInteractableObjectGrabbed(InteractableObjectEventArgs e)
    {
        if (!isRealCloth)
        {
            if (secondMenuWrapTryOn != null && gameObject == secondMenuWrapTryOn.currentFacingCloth)
            {
                GetComponentInParent<TryOnCloth>().currentTryOnClothOnMenu = belongingItemWrap.gameObject;
                GetComponentInParent<TryOnCloth>().currentTryOnCloth = gameObject;
            }
        }

        base.OnInteractableObjectGrabbed(e);
    }

    public void BaseOnInteractableObjectUngrabbed(InteractableObjectEventArgs e)
    {
        base.OnInteractableObjectUngrabbed(e);
    }

    public override void OnInteractableObjectUngrabbed(InteractableObjectEventArgs e)
    {
        if (!isRealCloth && !isTouchingUserBody)
        {
            ReturnToMenuWrap();
        }

        base.OnInteractableObjectUngrabbed(e);
    }

    public void BaseOnInteractableObjectTouched(InteractableObjectEventArgs e)
    {
        base.OnInteractableObjectTouched(e);
    }

    public override void OnInteractableObjectTouched(InteractableObjectEventArgs e)
    {
        GetComponent<MeshRenderer>().enabled = true;

        base.OnInteractableObjectTouched(e);
    }

    public void BaseOnInteractableObjectUntouched(InteractableObjectEventArgs e)
    {
        base.OnInteractableObjectUntouched(e);
    }

    public override void OnInteractableObjectUntouched(InteractableObjectEventArgs e)
    {
        if (isWeared)
        {
            GetComponent<MeshRenderer>().enabled = false;
        }

        base.OnInteractableObjectTouched(e);
    }

    /// <summary>
    /// Put the virtual cloth back to menu position
    /// </summary>
    public void ReturnToMenuWrap()
    {
        if (returningMenuCoroutine == null)
        {
            returningMenuCoroutine = StartCoroutine(ReturnToMenuAnimation());
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IEnumerator ReturnToMenuAnimation()
    {
        GetComponent<Collider>().enabled = false;
        transform.localScale = originalScale;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        transform.parent = belongingItemWrap;
        Vector3 startLocalPosition = transform.localPosition;
        Vector3 startLocalEuler = Vector3.zero;
        if (Mathf.Repeat(transform.localEulerAngles.x, 360) > 180)
        {
            startLocalEuler.x = 180 - Mathf.Repeat(transform.localEulerAngles.x, 360);
        }
        else
        {
            startLocalEuler.x = Mathf.Repeat(transform.localEulerAngles.x, 360);
        }
        if (Mathf.Repeat(transform.localEulerAngles.y, 360) > 180)
        {
            startLocalEuler.y = 180 - Mathf.Repeat(transform.localEulerAngles.y, 360);
        }
        else
        {
            startLocalEuler.y = Mathf.Repeat(transform.localEulerAngles.y, 360);
        }
        if (Mathf.Repeat(transform.localEulerAngles.z, 360) > 180)
        {
            startLocalEuler.z = 180 - Mathf.Repeat(transform.localEulerAngles.z, 360);
        }
        else
        {
            startLocalEuler.z = Mathf.Repeat(transform.localEulerAngles.z, 360);
        }

        for (float t = 0; t < 1; t += Time.deltaTime / 0.5f)
        {
            transform.localPosition = Vector3.Lerp(startLocalPosition, Vector3.zero, t);
            transform.localEulerAngles = Vector3.Lerp(startLocalEuler, Vector3.zero, t);
            yield return null;
        }
        
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
        GetComponent<Collider>().enabled = true;
        returningMenuCoroutine = null;
        GetComponent<Collider>().enabled = true;
    }
}
