using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothInfo : MonoBehaviour
{
    public bool isRealCloth; // If the cloth is a "real" cloth that's in the "real world", or a "virtual" one that's in the mobile app

    // Use this for initialization
    void Start()
    {
        // If it is a "real" cloth, then add the RealWorldObject component
        if(isRealCloth)
        {
            gameObject.AddComponent<RealWorldObject>();
            GetComponent<RealWorldObject>().alwaysMirror = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
