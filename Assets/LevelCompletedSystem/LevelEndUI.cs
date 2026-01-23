using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndUI : MonoBehaviour
{
    public bool success = true;
    public Level level;
    public GameEvent levelComplete;

    public void completeLevel()
    {
        Debug.Log("Raising Event");
        levelComplete.Raise();
    }

    public void toString()
    {
        Debug.Log("Success: " + success);
        if (success)
        {
            level.print();
        }
    }
}
