using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    private int executeTime; // TODO: Unclear variable name
    
    public int minSpawnWait;
    public int maxSpawnWait;
    public GameObject NPCObject;
    public GameObject spawnPoint;

    void Start()
    {
        // Set inital start spawn time
        executeTime = Mathf.RoundToInt(Time.time) + Random.Range(minSpawnWait, maxSpawnWait);
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.RoundToInt(Time.time) == executeTime){
            // Check for open seat
            foreach (GameObject seat in GameObject.FindGameObjectsWithTag("Seat")) // TODO: This could be a function bool isSeatAvaliable()
            {
                // Check if the seat is occupied
                if (!seat.GetComponent<NPCObjects>().GetOccupied())
                {
                    // TODO: This should be a function
                    Vector2 spawnPosition = spawnPoint.transform.position; // Get the spawn position from the spawn point
                    GameObject NPC = Instantiate(NPCObject, spawnPosition, Quaternion.identity);
                    NPCController behavior = NPC.GetComponent<NPCController>();
                    
                    behavior.SetSeat(seat); // Give NPC seat property
                    NPC.SetActive(true); // Show NPC
                    seat.GetComponent<NPCObjects>().SetOccupied(true); // Set seat as occupied

                    break;
                }
            }
            executeTime = executeTime + Random.Range(minSpawnWait, maxSpawnWait);
        }
    }
}