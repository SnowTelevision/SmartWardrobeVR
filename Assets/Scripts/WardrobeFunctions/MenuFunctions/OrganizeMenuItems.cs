using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Use to place menu items in correct positions
/// </summary>
public class OrganizeMenuItems : MonoBehaviour
{
    public bool localFollow; // Does this menu follows an object in a local coordinate system
    public Transform objectToFollow; // The object that the menu will follow its position and rotation
    public bool followRotation; // If the object follows rotation
    public float menuItemSpacing; // How far away should menu items be from each other if the menu is straight aligned
    public bool isCircle; // If the menu is circle or not (which will be a straight line)
    public bool rotateMenuItems; // If menu items rotate with the menu
    public float turningSpeedRatio; // How fast each menu item should turn according to the entire menu
    public float menuDistance; // The distance from the center of the menu to the menu items (it will be the radius of the menu if the menu is a circle)
    public List<GameObject> menuItems; // The items currently in the menu
    public float menuChangeAnimationDuration; // How long is the animation for changing between different menus
    public bool playExpandAnimation; // Should this menu play the expand animation when it is just opened
    public float itemTurningBackSpeed; // How fast an item turn back to its original rotation if it is not in front of the user

    public Vector3 menuLastLocalEuler; // The eulerangles of the menu on the last frame
    public Vector3 followPositionOffset; // The offset of the position it should follow

    // Use this for initialization
    void OnEnable()
    {
        if (playExpandAnimation)
        {
            StartCoroutine(ExpandMenuItems());
        }

        // If this is first level menu
        if (isCircle)
        {
            for (int i = 0; i < menuItems.Count; i++)
            {
                menuItems[i].transform.localPosition = CalculateRelativePosition(i);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Follows an object's local position and rotation
        if (localFollow)
        {
            transform.localPosition = objectToFollow.localPosition;
        }
        else
        {
            transform.position = objectToFollow.position + followPositionOffset;
        }

        if (followRotation)
        {
            transform.localRotation = objectToFollow.localRotation;
        }

        if (rotateMenuItems)
        {
            RotateMenuItems();
            menuLastLocalEuler = transform.localEulerAngles;
        }
    }

    private void FixedUpdate()
    {
        if (!rotateMenuItems)
        {
            RotateMenuItemsToOriginal();
        }
    }

    public void RotateMenuItemsToOriginal()
    {
        for (int i = 0; i < menuItems.Count; i++)
        {
            if (menuItems[i] != GetComponent<TryOnCloth>().currentFacingCloth &&
                Mathf.Abs(menuItems[i].transform.localEulerAngles.y) >= 1)
            {
                menuItems[i].transform.localEulerAngles =
                    new Vector3(menuItems[i].transform.localEulerAngles.x,
                                Mathf.Repeat(menuItems[i].transform.localEulerAngles.y - Mathf.Sign(
                                        180 - Mathf.Repeat(menuItems[i].transform.localEulerAngles.y, 360)) *
                                    Time.fixedDeltaTime * itemTurningBackSpeed, 360),
                                menuItems[i].transform.localEulerAngles.z);
            }
        }
    }

    /// <summary>
    /// Rotate menu items with the menu itself
    /// </summary>
    public void RotateMenuItems()
    {
        for (int i = 0; i < menuItems.Count; i++)
        {
            menuItems[i].transform.localEulerAngles =
                new Vector3(Mathf.Repeat(menuItems[i].transform.localEulerAngles.x + (transform.localEulerAngles.x - menuLastLocalEuler.x) * turningSpeedRatio, 360),
                            Mathf.Repeat(menuItems[i].transform.localEulerAngles.y + (transform.localEulerAngles.y - menuLastLocalEuler.y) * turningSpeedRatio, 360),
                            Mathf.Repeat(menuItems[i].transform.localEulerAngles.z + (transform.localEulerAngles.z - menuLastLocalEuler.z) * turningSpeedRatio, 360));
        }
    }

    /// <summary>
    /// Animation that expands menu items when a menu is just opened
    /// </summary>
    public IEnumerator ExpandMenuItems()
    {
        List<Vector3> initialLocalPositions = new List<Vector3>();
        List<Vector3> targetLocalPositions = new List<Vector3>();

        for (int i = 0; i < menuItems.Count; i++)
        {
            initialLocalPositions.Add(menuItems[i].transform.localPosition);
            targetLocalPositions.Add(CalculateRelativePosition(i));
        }

        for (float t = 0; t < 1; t += Time.deltaTime / menuChangeAnimationDuration)
        {
            for (int i = 0; i < menuItems.Count; i++)
            {
                menuItems[i].transform.localPosition = Vector3.Lerp(initialLocalPositions[i], targetLocalPositions[i], t);
            }

            yield return null;
        }

        for (int i = 0; i < menuItems.Count; i++)
        {
            menuItems[i].transform.localPosition = targetLocalPositions[i];
        }
    }

    /// <summary>
    /// Animation that retracts menu items when a menu is just closed
    /// </summary>
    public IEnumerator RetractMenuItems()
    {
        List<Vector3> initialLocalPositions = new List<Vector3>();

        for (int i = 0; i < menuItems.Count; i++)
        {
            initialLocalPositions.Add(menuItems[i].transform.localPosition);
        }

        for (float t = 0; t < 1; t += Time.deltaTime / menuChangeAnimationDuration)
        {
            for (int i = 0; i < menuItems.Count; i++)
            {
                menuItems[i].transform.localPosition = Vector3.Lerp(initialLocalPositions[i], Vector3.zero, t);
            }

            yield return null;
        }

        for (int i = 0; i < menuItems.Count; i++)
        {
            menuItems[i].transform.localPosition = Vector3.zero;
        }
    }

    /// <summary>
    /// Calculate the relative position for a menu item
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    public Vector3 CalculateRelativePosition(int itemIndex)
    {
        Vector3 relativePosi = Vector3.zero;

        if (isCircle)
        {
            float angle = 360f / (float)menuItems.Count * itemIndex; // Calculate the angle between the item and the menu's forward direction

            relativePosi.x = Mathf.Sin(Mathf.Deg2Rad * angle) * menuDistance;
            relativePosi.z = Mathf.Cos(Mathf.Deg2Rad * angle) * menuDistance;


        }
        else
        {
            relativePosi.x += menuItemSpacing * itemIndex;
        }

        return relativePosi;
    }
}
