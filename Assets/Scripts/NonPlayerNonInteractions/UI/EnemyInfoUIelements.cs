using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfoUIelements : MonoBehaviour
{
    /// <summary>
    /// This script is only used to locate the child game objects of enemy's info on player's HUD UI
    /// </summary>

    public GameObject detail; // The UI game object with enemy's detailed info if the enemy is within the HUD display area
    public GameObject arrow; // The UI game object which is an arrow showing the enemy's relative position if the enemy is out of the HUD display area
    public GameObject score; // The score to show when the enemy is killed;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
