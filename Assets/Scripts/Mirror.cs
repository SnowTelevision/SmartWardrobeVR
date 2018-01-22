using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    public List<GameObject> objectsToReflect; // What object should be reflected by the mirror
    public Shader shaderForMirroredObjects; // The special stencil shader that will be applied to the objects in the mirror which can only be seen through the mirror

    public List<GameObject> reflectionObjects; // The objects for what is reflected by the mirror
    public Vector3 mirrorNormal;
    public Vector3 mirrorPlaneNormal; // The normal of the mirror
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

    }

    // Update is called once per frame
    void Update()
    {
        mirrorNormal = GetComponent<MeshFilter>().mesh.normals[0];

        for (int i = 0; i < objectsToReflect.Count; i++)
        {
            if (reflectionObjects.Count <= i) // If a mirror object is not created for this object
            {
                reflectionObjects.Add(Instantiate(objectsToReflect[i]));

                // Disable any colliders in the mirror object
                foreach(Collider c in reflectionObjects[i].GetComponentsInChildren<Collider>())
                {
                    c.enabled = false;
                }

                // Remove any rigidbody in the mirror object
                foreach(Rigidbody r in reflectionObjects[i].GetComponentsInChildren<Rigidbody>())
                {
                    Destroy(r);
                }

                // Change the layer of the mirror object
                
                foreach(Transform t in reflectionObjects[i].GetComponentsInChildren<Transform>())
                {
                    t.gameObject.layer = LayerMask.NameToLayer("MirrorWorld");
                }

                // Change the shaders in the mirror object so the mirror object only appears in the mirror

                foreach (MeshRenderer mr in reflectionObjects[i].GetComponentsInChildren<MeshRenderer>())
                {
                    foreach (Material m in mr.materials)
                    {
                        m.shader = shaderForMirroredObjects;
                    }
                }
            }

            CalculateReflectedTransform(objectsToReflect[i].transform, reflectionObjects[i].transform);
        }

        //Test
        //CalculateReflectedTransform(testObject.transform, testMirrorObject.transform);
        //mirrorPosition = transform.position;
        //mirrorPlanePosition = mirrorPlane.GetDistanceToPoint(mirrorPosition);
        //Debug.DrawRay(transform.position, mirrorNormal, Color.red);
    }

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
