using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class InteractableClothInfo : ClothInfo
{
    public GameObject thisClothModel; // The game object to be created each time the user want to wear this cloth
    public Transform menuOwner; // The owner of the menu that has this item

    public GameObject currentCreatedModel; // The model that is currently created

    // Use this for initialization
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void OnInteractableObjectTouched(InteractableObjectEventArgs e)
    {
        if(menuOwner.name == e.interactingObject.name)
        {
            return;
        }

        //GetComponent<MeshRenderer>().enabled = true;
        if (currentCreatedModel == null)
        {
            currentCreatedModel = Instantiate(thisClothModel, transform.position, transform.rotation);
            currentCreatedModel.GetComponent<DisposableClothInfo>().spawner = this;
        }

        base.BaseOnInteractableObjectTouched(e);
    }

    public override void OnInteractableObjectUntouched(InteractableObjectEventArgs e)
    {
        //GetComponent<MeshRenderer>().enabled = false;

        base.BaseOnInteractableObjectUntouched(e);
    }
}
