using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyGlass : MonoBehaviour 
{
    private bool isDirty = true; 

    public bool IsDirty()
    {
        return isDirty;
    }
    public void SetDirty(bool dirty)
    {
        isDirty = dirty;
    }
}

// TODO: This should inherit from a HoldableObject class and have behavior allowing it to interact with player holder