using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Event")]
public class GameEvent : ScriptableObject
{
    public event Action OnRaised;

    public void Raise()
    {
        OnRaised?.Invoke();
    }
}