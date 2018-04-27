using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedEnable : MonoBehaviour
{
    public RealBodyWearCloth toEnable;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(DelayedEnablee());
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public IEnumerator DelayedEnablee()
    {
        yield return new WaitForSeconds(1);

        toEnable.enabled = true;
    }
}
