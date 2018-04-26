using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionAnimation : MonoBehaviour
{
    public float emissionBreathLength; // How long does it take for the emission goes from 0 to 1 and back
    public float emissionStrength; // The maximum intensity of the emission

    public List<MeshRenderer> clothMeshes;

    // Use this for initialization
    void Start()
    {
        foreach (MeshRenderer m in GetComponentsInChildren<MeshRenderer>())
        {
            clothMeshes.Add(m);
            m.material.EnableKeyword("_EMISSION");
        }

        emissionBreathLength /= 2f;
    }

    // Update is called once per frame
    void Update()
    {
        // print(Mathf.PingPong(Time.time / emissionBreathLength, 1));

        foreach (MeshRenderer m in clothMeshes)
        {
            if (!m.material.HasProperty("_EmissionColor"))
            {
                continue;
            }
            //Color emissionColor = m.material.GetColor("_EmissionColor");
            //emissionColor *= Mathf.PingPong(Time.time / emissionBreathLength, 1);// Mathf.LinearToGammaSpace(Mathf.PingPong(Time.time / emissionBreathLength, 1));
            //m.material.SetColor("_EmissionColor", emissionColor);
            m.material.SetColor("_EmissionColor", Color.white * Mathf.PingPong(Time.time / emissionBreathLength, 1) * emissionStrength); 
            //print(m.material.GetColor("_EmissionColor"));
        }
    }
}
