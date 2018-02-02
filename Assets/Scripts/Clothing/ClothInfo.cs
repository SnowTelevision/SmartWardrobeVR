using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothInfo : MonoBehaviour
{
    public bool isRealCloth; // If the cloth is a "real" cloth that's in the "real world", or a "virtual" one that's in the mobile app
    public Vector3 clothLocalPosition; // Where the cloth should be relate to the body part
    public Vector3 clothLocalEulerAngles; // What's the cloth's rotation relate to the body part
    public bool freelyWear; // Does the cloth has a fixed position on the body, or can be put on any part of the body

    public bool isWeared; // If the cloth is currently weared

    // Use this for initialization
    void Start()
    {
        // If it is a "real" cloth, then add the RealWorldObject component
        if (isRealCloth)
        {
            gameObject.AddComponent<RealWorldObject>();
            GetComponent<RealWorldObject>().alwaysMirror = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWeared && transform.parent != null)
        {
            transform.parent = null;
        }
    }
}
