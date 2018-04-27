using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedAddComponent : MonoBehaviour
{
    public Vector3 boxColliderSize;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(DelayedAddBoxCollider());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator DelayedAddBoxCollider()
    {
        yield return new WaitForSeconds(1);

        gameObject.AddComponent<BoxCollider>();
        GetComponent<BoxCollider>().isTrigger = true;
        GetComponent<BoxCollider>().size = boxColliderSize;
    }
}
