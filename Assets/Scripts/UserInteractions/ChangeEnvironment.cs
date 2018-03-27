using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEnvironment : MonoBehaviour
{
    public GameObject environment; // The entire environment to be changed

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Change the environment
    /// </summary>
    public void ChangeEnrironment()
    {
        // Hide the current environment
        ApplicationManager.currentScene.SetActive(false);

        // Change the current environment to be this environment
        ApplicationManager.currentScene = environment;

        // Show this environment
        ApplicationManager.currentScene.SetActive(true);
    }
}
