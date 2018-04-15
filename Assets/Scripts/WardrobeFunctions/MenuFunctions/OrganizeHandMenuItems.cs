using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Organize the history or saved menu that will show up on user's wrist
/// </summary>
public class OrganizeHandMenuItems : MonoBehaviour
{
    public GameObject historyClothMenuWrap; // The history cloth menu
    public GameObject savedClothMenuWrap; // The saved cloth menu
    public Transform userBody; // The user's body's transform
    public OrganizeHandMenuItems otherHandMenu; // The menu on the other hand
    public float menuDistance; // How far the hand menu items is away from the hand
    public float menuChangeAnimationDuration; // How long is the animation for open or close menu

    public GameObject userHead; // The head object of the user
    public Coroutine expandSavedMenuAnimation; // The animation expands saved menu 
    public Coroutine expandHistoryMenuAnimation; // The animation expands history menu 
    //public bool savedInner; // Is the saved menu in the inner side relative to the user or the outter side?

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (userHead == null)
        {
            userHead = FindObjectOfType<SteamVR_Camera>().gameObject;
        }

        CheckControllerRelativeRotation();

        //if (historyAndSavedClothMenuWrap.activeInHierarchy)
        //{

        //}
    }

    /// <summary>
    /// Checks the relative rotation of the controller with the head
    /// </summary>
    public void CheckControllerRelativeRotation()
    {
        //print("up: " + Vector3.Angle(userHead.transform.up, transform.up) + ", forward: " + Vector3.Angle(userHead.transform.forward, transform.forward));

        // If the controller is upside-down relative to the head and is pointing to the head's relative forward direction
        if (Vector3.Angle(userHead.transform.up, transform.up) >= 150 &&
            Vector3.Angle(userHead.transform.forward, transform.forward) <= 30)
        {
            if (!historyClothMenuWrap.activeInHierarchy)
            {
                historyClothMenuWrap.SetActive(true);
                savedClothMenuWrap.SetActive(true);
                //print("open menu");
                OpenMenu();
            }
        }
        else
        {
            historyClothMenuWrap.SetActive(false);
            savedClothMenuWrap.SetActive(false);
        }
    }

    ///// <summary>
    ///// Check if open either of the second level menus (history or saved)
    ///// </summary>
    //public void CheckOpenSecondMenu()
    //{
    //    Vector3 controllerToBodyVec = userBody.position - transform.position;

    //    // If the controller it rotated pass the open menu threshold
    //    if (Vector3.Angle(transform.forward, controllerToBodyVec) < 150)
    //    {
    //        // If the controller is rotating towards left
    //        if (Vector3.Angle(transform.right, controllerToBodyVec) < 90)
    //        {
    //            // Show menu on right side
    //            OpenMenu(true);
    //        }
    //        else
    //        {
    //            // Show menu on left side
    //        }
    //    }
    //}

    public void OpenMenu()
    {
        if (expandHistoryMenuAnimation != null)
        {
            StopCoroutine(expandHistoryMenuAnimation);
        }
        if (expandSavedMenuAnimation != null)
        {
            StopCoroutine(expandSavedMenuAnimation);
        }

        expandHistoryMenuAnimation = StartCoroutine(ExpandMenu(WardrobeDatabase.database.tryHistory, true));
        expandSavedMenuAnimation = StartCoroutine(ExpandMenu(WardrobeDatabase.database.choseCloth, false));
        //if (right)
        //{
        //    if (name == "LeftController")
        //    {
        //        if (savedInner)
        //        {
        //            savedClothMenuWrap.SetActive(true);
        //        }
        //        else
        //        {
        //            historyClothMenuWrap.SetActive(true);
        //        }
        //    }
        //    else
        //    {
        //        if (savedInner)
        //        {
        //            historyClothMenuWrap.SetActive(true);
        //        }
        //        else
        //        {
        //            savedClothMenuWrap.SetActive(true);
        //        }
        //    }
        //}
        //else
        //{
        //    if (name == "LeftController")
        //    {
        //        if (savedInner)
        //        {
        //            historyClothMenuWrap.SetActive(true);
        //        }
        //        else
        //        {
        //            savedClothMenuWrap.SetActive(true);
        //        }
        //    }
        //    else
        //    {
        //        if (savedInner)
        //        {
        //            savedClothMenuWrap.SetActive(true);
        //        }
        //        else
        //        {
        //            historyClothMenuWrap.SetActive(true);
        //        }
        //    }
        //}
    }

    public IEnumerator ExpandMenu(List<InteractableClothInfo> itemList, bool isHistory)
    {
        List<Vector3> initialLocalPositions = new List<Vector3>();
        List<Vector3> targetLocalPositions = new List<Vector3>();

        for (int i = 0; i < itemList.Count; i++)
        {
            if (isHistory && !historyClothMenuWrap.transform.Find(itemList[i].gameObject.name + "(Clone)"))
            {
                GameObject newHistoryCloth = Instantiate(itemList[i].gameObject, historyClothMenuWrap.transform);
                newHistoryCloth.GetComponent<InteractableClothInfo>().menuOwner = transform;
            }
            if (!isHistory && savedClothMenuWrap.transform.Find(itemList[i].gameObject.name + "(Clone)"))
            {
                GameObject newSavedCloth = Instantiate(itemList[i].gameObject, savedClothMenuWrap.transform);
                newSavedCloth.GetComponent<InteractableClothInfo>().menuOwner = transform;
            }

            initialLocalPositions.Add(Vector3.zero);
            targetLocalPositions.Add(CalculateRelativePosition(i, itemList));
        }

        for (float t = 0; t < 1; t += Time.deltaTime / menuChangeAnimationDuration)
        {
            int i = 0;

            if (isHistory)
            {
                List<Transform> tFl = new List<Transform>(historyClothMenuWrap.GetComponentsInChildren<Transform>());
                tFl.Remove(historyClothMenuWrap.transform);

                foreach (Transform tF in tFl)
                {
                    //print(tF.name);
                    tF.localPosition = Vector3.Lerp(initialLocalPositions[i], targetLocalPositions[i], t);
                    i++;
                }
            }
            else
            {
                List<Transform> tFl = new List<Transform>(savedClothMenuWrap.GetComponentsInChildren<Transform>());
                tFl.Remove(savedClothMenuWrap.transform);

                foreach (Transform tF in tFl)
                {
                    tF.localPosition = Vector3.Lerp(initialLocalPositions[i], targetLocalPositions[i], t);
                    i++;
                }
            }

            yield return null;
        }

        if (true) // Wrap to create local int i
        {
            int i = 0;

            if (isHistory)
            {
                List<Transform> tFl = new List<Transform>(historyClothMenuWrap.GetComponentsInChildren<Transform>());
                tFl.Remove(historyClothMenuWrap.transform);

                foreach (Transform tF in tFl)
                {
                    tF.transform.localPosition = targetLocalPositions[i];
                    i++;
                }
            }
            else
            {
                List<Transform> tFl = new List<Transform>(savedClothMenuWrap.GetComponentsInChildren<Transform>());
                tFl.Remove(savedClothMenuWrap.transform);

                foreach (Transform tF in tFl)
                {
                    tF.transform.localPosition = targetLocalPositions[i];
                    i++;
                }
            }
        }
    }

    /// <summary>
    /// Calculate the relative position for a menu item
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    public Vector3 CalculateRelativePosition(int itemIndex, List<InteractableClothInfo> itemList)
    {
        Vector3 relativePosi = Vector3.zero;
        float angle = 180f / (float)itemList.Count * itemIndex + 22.5f; // Calculate the angle between the item and the menu's forward direction

        relativePosi.x = Mathf.Sin(Mathf.Deg2Rad * angle) * menuDistance;
        relativePosi.z = Mathf.Cos(Mathf.Deg2Rad * angle) * menuDistance;


        return relativePosi;
    }
}
