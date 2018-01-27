using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealWorldObject : MonoBehaviour
{
    public GameObject mirrorCopy; // The copy on the mirror side

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // If the object is on the mirror's reflective side but doesn't have a copy in the mirror world
        if(mirrorCopy == null && ShouldHaveReflection())
        {
            Mirror.staticMirrorRef.InstantiateMirrorWorldObject(gameObject);
        }

        // If the object is on the mirror's non-reflective side but has a copy in the mirror world
        else if(mirrorCopy != null && !ShouldHaveReflection())
        {
            Mirror.staticMirrorRef.DestroyMirrorWorldObject(mirrorCopy);
        }
    }


    public bool ShouldHaveReflection()
    {
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
}
