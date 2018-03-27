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

    public Vector3 menuLastLocalEuler; // The eulerangles of the menu on the last frame
    public Vector3 followPositionOffset; // The offset of the position it should follow

    // Use this for initialization
    void Start()
    {
        ArrangeMenuItems();
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
    /// Arrange the position of menu items in the menu
    /// </summary>
    public void ArrangeMenuItems()
    {
        for (int i = 0; i < menuItems.Count; i++)
        {
            menuItems[i].transform.localPosition = CalculateRelativePosition(i);
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
