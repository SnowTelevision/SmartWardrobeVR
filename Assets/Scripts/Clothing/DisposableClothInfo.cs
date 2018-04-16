using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class DisposableClothInfo : ClothInfo
{
    public InteractableClothInfo spawner; // Which hand menu item spawned it
    public float enlargeDistance; // How far from its spawner will it become large model

    // Use this for initialization
    public override void Start()
    {
        transform.localScale = spawner.transform.localScale;
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        // Update size
        if (!isWeared)
        {
            ChangeSize();
        }

        base.Update();
    }

    /// <summary>
    /// Change the model size according to the distance from its spawner
    /// </summary>
    public void ChangeSize()
    {
        if (Vector3.Distance(transform.position, spawner.transform.position) > enlargeDistance)
        {
            transform.localScale = originalScale;
        }
        else
        {
            transform.localScale = spawner.transform.localScale;
        }
    }

    public override void OnInteractableObjectTouched(InteractableObjectEventArgs e)
    {
        base.OnInteractableObjectTouched(e);
    }

    public override void OnInteractableObjectUngrabbed(InteractableObjectEventArgs e)
    {
        StartCoroutine(UngrabProc(e));

        //if (!isWeared && !isTouchingUserBody)
        //{
        //    Destroy(gameObject);
        //}

        //spawner.currentCreatedModel = null;
        //base.BaseOnInteractableObjectUngrabbed(e);
    }

    public IEnumerator UngrabProc(InteractableObjectEventArgs e)
    {
        yield return new WaitForSeconds(0.1f);
        yield return null;

        if (!isWeared && !isTouchingUserBody)
        {
            Destroy(gameObject);
        }

        spawner.currentCreatedModel = null;
        base.BaseOnInteractableObjectUngrabbed(e);
    }

    public override void OnInteractableObjectUntouched(InteractableObjectEventArgs e)
    {
        // Prevent the newly created cloth get interacted by the controller that has the menu that created it
        if (e.interactingObject == spawner.menuOwner.gameObject ||
            isTouchingUserBody || isWeared)
        {
            return;
        }

        spawner.currentCreatedModel = null;
        //spawner.GetComponent<MeshRenderer>().enabled = true;
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        //print("destroy");
    }
}
