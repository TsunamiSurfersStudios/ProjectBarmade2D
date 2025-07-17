using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCObjects : MonoBehaviour // TODO: Is there a better classname? I believe this is only being used on SeatObjects
{
    public bool occupied = false;

    public void SetOccupied(bool isOccupied)
    {
        occupied = isOccupied;
    }

    public bool GetOccupied()
    {
        return occupied;
    }
}