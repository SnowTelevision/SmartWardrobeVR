using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    public GameObject defaultActiveScene; // The default scene that is active when the application starts
    public GameObject openFirstMenuTutorial;
    public GameObject startMarker;
    public GameObject arGlasses; // The AR glasses that enables the wardrobe's AR UI
    public GameObject playerBody;
    public Vector3 arGlassesRelativeAppearPosition; // The relative position of the AR glasses when it appears after the user stand on the starting position

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
            //openFirstMenuTutorial.SetActive(true);
            arGlasses.transform.SetParent(playerBody.transform);
            arGlasses.transform.localPosition = arGlassesRelativeAppearPosition;
            arGlasses.transform.localEulerAngles = new Vector3(0, 180, 0);
            arGlasses.transform.SetParent(null);
            arGlasses.SetActive(true);
        }
    }
}
