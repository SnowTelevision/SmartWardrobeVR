using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    public GameObject defaultActiveScene; // The default scene that is active when the application starts

    public static GameObject currentScene;

    // Use this for initialization
    void Start()
    {
        currentScene = defaultActiveScene;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
