using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    public List<GameObject> objectsToReflect; // What object should be reflected by the mirror
    public Shader shaderForMirroredObjects; // The special stencil shader that will be applied to the objects in the mirror which can only be seen through the mirror

    public List<GameObject> reflectionObjects; // The objects for what is reflected by the mirror
    public static Vector3 mirrorNormal; // The normal of the mirror
    public static Mirror staticMirrorRef; // The static reference of this mirror
    //public Vector3 mirrorPlaneNormal; // The normal of the mirror
    public Material tempMaterial; // The temporary material for switching shader for the mirror objects

    /// <summary>
    /// Testing
    /// </summary>
    //public GameObject testObject;
    //public GameObject testMirrorObject;
    public float mirrorPlanePosition;

    // Use this for initialization
    void Start()
    {
        staticMirrorRef = this;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMirrorWorldTransform();

        //Test
        //CalculateReflectedTransform(testObject.transform, testMirrorObject.transform);
        //mirrorPosition = transform.position;
        //mirrorPlanePosition = mirrorPlane.GetDistanceToPoint(mirrorPosition);
        //Debug.DrawRay(transform.position, mirrorNormal, Color.red);
    }

    /// <summary>
    /// Destroy the mirror copy of a real object when the real object is destroied or move to the mirror's non-reflective side
    /// </summary>
    /// <param name="mirrorObject"></param>
    public void DestroyMirrorWorldObject(GameObject mirrorObject)
    {
        reflectionObjects.Remove(mirrorObject); // Remove the mirror object from the mirrored object list
        objectsToReflect.Remove(mirrorObject.GetComponent<MirrorWorldObject>().realCopy); // Remove the corresponding real object from the real object list
        mirrorObject.GetComponent<MirrorWorldObject>().realCopy.GetComponent<RealWorldObject>().mirrorCopy = null; // Remove the reference of the mirror copy in the corresponding real object
        Destroy(mirrorObject);
    }

    /// <summary>
    /// Instantiates the copy of a real world object when the real object is just instantiated or move to the mirror's reflective side
    /// </summary>
    /// <param name="realObject"></param>
    public void InstantiateMirrorWorldObject(GameObject realObject)
    {
        GameObject newMirrorObject = Instantiate(realObject); // Instantiate the mirror copy for the real object

        // Disable any colliders in the mirror object
        foreach (Collider c in newMirrorObject.GetComponentsInChildren<Collider>())
        {
            c.enabled = false;
        }

        // Remove any rigidbody in the mirror object
        foreach (Rigidbody r in newMirrorObject.GetComponentsInChildren<Rigidbody>())
        {
            Destroy(r);
        }

        // Change the layer of the mirror object

        foreach (Transform t in newMirrorObject.GetComponentsInChildren<Transform>())
        {
            t.gameObject.layer = LayerMask.NameToLayer("MirrorWorld");
        }

        // Change the shaders in the mirror object so the mirror object only appears in the mirror
        foreach (MeshRenderer mr in newMirrorObject.GetComponentsInChildren<MeshRenderer>())
        {
            mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; // Don't let the mirror object cast shadows

            foreach (Material m in mr.materials)
            {
                m.shader = shaderForMirroredObjects;
            }
        }

        // Change the RealWorldObject component to MirrorWorldObject
        Destroy(newMirrorObject.GetComponent<RealWorldObject>());
        newMirrorObject.AddComponent<MirrorWorldObject>();

        objectsToReflect.Add(realObject); // Add this real object into the real object list
        reflectionObjects.Add(newMirrorObject); // Add this mirror object into the mirrored object list
        realObject.GetComponent<RealWorldObject>().mirrorCopy = newMirrorObject; // Refer the mirror copy in the real object
        newMirrorObject.GetComponent<MirrorWorldObject>().realCopy = realObject; // Refer the real copy in the mirror object
    }

    /// <summary>
    /// Updates mirror world's objects' transforms
    /// </summary>
    public void UpdateMirrorWorldTransform()
    {
        mirrorNormal = GetComponent<MeshFilter>().mesh.normals[0];

        for (int i = 0; i < reflectionObjects.Count; i++)
        {
            CalculateReflectedTransform(objectsToReflect[i].transform, reflectionObjects[i].transform);
        }
    }

    /// <summary>
    /// Calculates the transform of a real object's mirror copy
    /// </summary>
    /// <param name="real"></param>
    /// <param name="reflected"></param>
    public void CalculateReflectedTransform(Transform real, Transform reflected)
    {
        // Calculate position
        Vector3 realPosition = transform.InverseTransformPoint(real.position);
        Vector3 mirrorPosition = realPosition - 2 * mirrorNormal
                                                  * Vector3.Dot(realPosition, mirrorNormal)
                                                  / Vector3.Dot(mirrorNormal, mirrorNormal);
        reflected.position = transform.TransformPoint(mirrorPosition);

        // Calculate forward direction
        Vector3 realForward = transform.InverseTransformPoint(real.TransformPoint(Vector3.forward));
        Vector3 mirrorForward = realForward - 2 * mirrorNormal
                                                * Vector3.Dot(realForward, mirrorNormal)
                                                / Vector3.Dot(mirrorNormal, mirrorNormal);

        // Calculate up direction
        Vector3 realUp = transform.InverseTransformPoint(real.TransformPoint(Vector3.up));
        Vector3 mirrorUp = realUp - 2 * mirrorNormal
                                      * Vector3.Dot(realUp, mirrorNormal)
                                      / Vector3.Dot(mirrorNormal, mirrorNormal);

        reflected.LookAt(transform.TransformPoint(mirrorForward), reflected.TransformDirection(reflected.InverseTransformPoint(transform.TransformPoint(mirrorUp))));

        // Test
        //if (real.name == "Sphere")
        //{
        //    print("forward: " + transform.TransformPoint(mirrorForward) + ", up: " + transform.TransformPoint(mirrorUp) + ", calculated local up: " + reflected.InverseTransformPoint(transform.TransformPoint(mirrorUp)) + ", real local up: " + reflected.InverseTransformPoint(reflected.up));
        //}
    }
}
