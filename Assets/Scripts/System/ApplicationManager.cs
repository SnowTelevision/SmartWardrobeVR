using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    public GameObject defaultActiveScene; // The default scene that is active when the application starts
    public GameObject openFirstMenuTutorial;
    public GameObject startMarker;

    public static GameObject currentScene;
    public static bool started;

    // Use this for initialization
    void Start()
    {
        currentScene = defaultActiveScene;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!started && other.GetComponent<RealBodyWearCloth>())
        {
            started = true;
            startMarker.SetActive(false);
            openFirstMenuTutorial.SetActive(true);
        }
    }
}
