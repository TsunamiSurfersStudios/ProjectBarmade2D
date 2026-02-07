using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeatController : MonoBehaviour
{
    public bool occupied = false;
    [SerializeField] Direction direction;
    

    public enum Direction
    {
        Forward,
        Left,
        Right
    };                                                                                                              

    public void SetOccupied(bool isOccupied)
    {
        occupied = isOccupied;
    }

    public bool GetOccupied()
    {
        return occupied;
    }

    public Direction GetDirection()
    {
        //gets the direction the seat is facing
        return direction;
    }
}

