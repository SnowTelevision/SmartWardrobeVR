using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedDeactivation : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        StartCoroutine(DelayedDeactivate());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator DelayedDeactivate()
    {
        //yield return new WaitForSeconds(1);
        yield return new WaitForEndOfFrame();

        gameObject.SetActive(false);
    }
}
