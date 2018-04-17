using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class InteractableClothInfo : ClothInfo
{
    public GameObject thisClothModel; // The game object to be created each time the user want to wear this cloth
    public Transform menuOwner; // The owner of the menu that has this item
    public bool isHistory;

    public GameObject currentCreatedModel; // The model that is currently created

    // Use this for initialization
    public override void Start()
    {
        if (isHistory)
        {
            base.Start();
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (isHistory)
        {
            base.Update();
        }
    }

    public override void OnInteractableObjectTouched(InteractableObjectEventArgs e)
    {
        // Prevent the same controller that opens the menu spawn the menu item
        if (menuOwner.name == e.interactingObject.name)
        {
            return;
        }

        if (isHistory)
        { 
            //GetComponent<MeshRenderer>().enabled = true;
            if (currentCreatedModel == null)
            {
                currentCreatedModel = Instantiate(thisClothModel, transform.position, transform.rotation);
                currentCreatedModel.GetComponent<DisposableClothInfo>().spawner = this;
            }

            base.BaseOnInteractableObjectTouched(e);
        }
        else
        {

        }
    }

    public override void OnInteractableObjectUntouched(InteractableObjectEventArgs e)
    {
        if (isHistory)
        {
            base.BaseOnInteractableObjectUntouched(e);
        }
        else
        {

        }
    }
}
