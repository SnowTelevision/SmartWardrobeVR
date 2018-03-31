using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealWorldObject : MonoBehaviour
{
    public bool alwaysMirror; // Should this object always have a mirror? i.e. the floor or ceiling of the room
    public GameObject replacingMirrorObject; // An object used in the mirror world if the original object is not "left&right" symmetry

    public GameObject mirrorCopy; // The copy on the mirror side

    // Use this for initialization
    void Start()
    {
        // If this object is not a virtual menu item
        if (gameObject.layer != LayerMask.NameToLayer("MenuItem"))
        {
            // Change the layer of the mirror object
            foreach (Transform t in GetComponentsInChildren<Transform>())
            {
                if (t.name != "GazeDetector")
                {
                    t.gameObject.layer = LayerMask.NameToLayer("RealWorld");
                }
            }
            //gameObject.layer = LayerMask.NameToLayer("RealWorld"); // Change the layer of this object to "RealWorld"
        }

        // If the object is a light source, then make it not shine on mirror objects
        if (GetComponent<Light>())
        {
            GetComponent<Light>().cullingMask = ~512;
        }

        // If the object is "left&right" symmetry, then use itself as the object used in the mirror world
        if (replacingMirrorObject == null)
        {
            replacingMirrorObject = gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If the object is on the mirror's reflective side but doesn't have a copy in the mirror world
        if (mirrorCopy == null && ShouldHaveReflection())
        {
            Mirror.staticMirrorRef.InstantiateMirrorWorldObject(gameObject, replacingMirrorObject);
        }

        // If the object is on the mirror's non-reflective side but has a copy in the mirror world
        else if (mirrorCopy != null && !ShouldHaveReflection())
        {
            Mirror.staticMirrorRef.DestroyMirrorWorldObject(mirrorCopy);
        }
    }

    /// <summary>
    /// If the object is in the front side of the mirror then it should have a reflection
    /// </summary>
    /// <returns></returns>
    public bool ShouldHaveReflection()
    {
        // If the object should always have a mirror
        if (alwaysMirror)
        {
            return true;
        }

        // If the object is on the mirror's reflective side
        if (Vector3.Dot(Mirror.mirrorNormal, Mirror.staticMirrorRef.transform.InverseTransformPoint(transform.position)) > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDestroy()
    {
        // Destroy the mirror copy if the real copy is destroyed
        if (mirrorCopy != null)
        {
            Mirror.staticMirrorRef.DestroyMirrorWorldObject(mirrorCopy);
        }
    }
}
