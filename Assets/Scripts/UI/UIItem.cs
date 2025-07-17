using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItem : MonoBehaviour
{
    [SerializeField] private bool isPersistable = true; // Can only edit value in Unity Editor

    // Update is called once per frame
    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public bool GetPersistableState()
    { return isPersistable; }   
}
