using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    public Shader shaderForMirroredObjects; // The special stencil shader that will be applied to the objects in the mirror which can only be seen through the mirror

    public List<GameObject> objectsToReflect; // What object should be reflected by the mirror
    public List<GameObject> reflectionObjects; // The objects for what is reflected by the mirror
    public static Vector3 mirrorNormal; // The normal of the mirror
    public static Mirror staticMirrorRef; // The static reference of this mirror
    public static Shader sShaderForMirroredObjects; // The static reference of the mirror shader
    //public Vector3 mirrorPlaneNormal; // The normal of the mirror
    //public Material tempMaterial; // The temporary material for switching shader for the mirror objects

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
        sShaderForMirroredObjects = shaderForMirroredObjects;
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

        // Change the RealWorldObject component to MirrorWorldObject
        Destroy(newMirrorObject.GetComponent<RealWorldObject>());

        if (newMirrorObject.GetComponent<ClothInfo>())
        {
            Destroy(newMirrorObject.GetComponent<ClothInfo>());
        }

        //if (newMirrorObject.GetComponent<SteamVR_RenderModel>())
        //{
        //    newMirrorObject.GetComponent<SteamVR_RenderModel>().shader = sShaderForMirroredObjects;
        //}

        newMirrorObject.AddComponent<MirrorWorldObject>();

        newMirrorObject.transform.localScale = realObject.transform.lossyScale; // Re-scale the new mirror object respect to the world

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
    public static void CalculateReflectedTransform(Transform real, Transform reflected)
    {
        // Calculate position
        Vector3 realPosition = Mirror.staticMirrorRef.transform.InverseTransformPoint(real.position);
        Vector3 mirrorPosition = realPosition - 2 * Mirror.mirrorNormal
                                                  * Vector3.Dot(realPosition, Mirror.mirrorNormal)
                                                  / Vector3.Dot(Mirror.mirrorNormal, Mirror.mirrorNormal);
        reflected.position = Mirror.staticMirrorRef.transform.TransformPoint(mirrorPosition);

        // Calculate forward direction
        Vector3 realForward = Mirror.staticMirrorRef.transform.InverseTransformDirection(real.TransformDirection(Vector3.forward));
        Vector3 mirrorForward = realForward - 2 * Mirror.mirrorNormal
                                                * Vector3.Dot(realForward, Mirror.mirrorNormal)
                                                / Vector3.Dot(Mirror.mirrorNormal, Mirror.mirrorNormal);

        // Calculate up direction
        Vector3 realUp = Mirror.staticMirrorRef.transform.InverseTransformDirection(real.TransformDirection(Vector3.up));
        Vector3 mirrorUp = realUp - 2 * Mirror.mirrorNormal
                                      * Vector3.Dot(realUp, Mirror.mirrorNormal)
                                      / Vector3.Dot(Mirror.mirrorNormal, Mirror.mirrorNormal);

        // Test
        //if (real.name == "Sport (1)")
        //{
        //    print("real copy up: " + staticMirrorRef.transform.InverseTransformPoint(real.TransformPoint(Vector3.up)) + ", real copy forward: " + staticMirrorRef.transform.InverseTransformPoint(real.TransformPoint(Vector3.forward)));
        //    print("forward: " + staticMirrorRef.transform.TransformPoint(mirrorForward) + ", up: " + staticMirrorRef.transform.TransformPoint(mirrorUp) + ", calculated local up: " + reflected.InverseTransformPoint(staticMirrorRef.transform.TransformPoint(mirrorUp)) + ", real local up: " + reflected.InverseTransformPoint(reflected.up));
        //    print("forward: " + Mirror.staticMirrorRef.transform.TransformPoint(mirrorForward) + ", up: " + reflected.TransformDirection(reflected.InverseTransformPoint(Mirror.staticMirrorRef.transform.TransformPoint(mirrorUp))));

        //    print("mirror up: " + mirrorUp);
        //    print("Mirror.staticMirrorRef.transform.TransformPoint(mirrorUp): " + Mirror.staticMirrorRef.transform.TransformPoint(mirrorUp));
        //    print(reflected.InverseTransformPoint(Mirror.staticMirrorRef.transform.TransformPoint(mirrorUp)));

        //}

        reflected.LookAt(reflected.transform.position + Mirror.staticMirrorRef.transform.TransformDirection(mirrorForward), reflected.TransformDirection(reflected.InverseTransformDirection(Mirror.staticMirrorRef.transform.TransformDirection(mirrorUp))));

        // Test
        //if (real.name == "Sport (1)")
        //{
        //    //print("real copy up: " + staticMirrorRef.transform.InverseTransformPoint(real.TransformPoint(Vector3.up)) + ", real copy forward: " + staticMirrorRef.transform.InverseTransformPoint(real.TransformPoint(Vector3.forward)));
        //    print("forward: " + staticMirrorRef.transform.TransformPoint(mirrorForward) + ", up: " + staticMirrorRef.transform.TransformPoint(mirrorUp) + ", calculated local up: " + reflected.InverseTransformPoint(staticMirrorRef.transform.TransformPoint(mirrorUp)) + ", real local up: " + reflected.InverseTransformPoint(reflected.up));
        //}
    }
}
