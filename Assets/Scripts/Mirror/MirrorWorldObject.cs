using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorWorldObject : MonoBehaviour
{
    public GameObject realCopy; // The corresponding real object

    // Use this for initialization
    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("MirrorWorld"); // Change the layer of this object to "MirrorWorld"

        // If the object is a light source, then make it only shine on mirror objects
        if (GetComponent<Light>())
        {
            GetComponent<Light>().cullingMask = 512;
        }

        // Disable any colliders in the mirror object
        foreach (Collider c in GetComponentsInChildren<Collider>())
        {
            c.enabled = false;
        }

        // Remove any rigidbody in the mirror object
        foreach (Rigidbody r in GetComponentsInChildren<Rigidbody>())
        {
            Destroy(r);
        }

        // Change the layer of the mirror object
        foreach (Transform t in GetComponentsInChildren<Transform>())
        {
            t.gameObject.layer = LayerMask.NameToLayer("MirrorWorld");

            if (t.GetComponent<CheckIfGazed>())
            {
                Destroy(t.GetComponent<CheckIfGazed>());
            }
        }

        // Change the shaders in the mirror object so the mirror object only appears in the mirror
        foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
        {
            mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; // Don't let the mirror object cast shadows

            foreach (Material m in mr.materials)
            {
                m.shader = Mirror.sShaderForMirroredObjects;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Destroy the mirror copy is the real copy is inactive
        if (!realCopy.activeInHierarchy)
        {
            Mirror.staticMirrorRef.DestroyMirrorWorldObject(gameObject);
        }
    }
}
