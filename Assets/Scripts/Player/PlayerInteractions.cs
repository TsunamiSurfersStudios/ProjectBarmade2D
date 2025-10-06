using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Keybind
{
    [SerializeField] public KeyCode keybind;
    [SerializeField] public UnityEvent actions;
}
public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] Keybind[] interactions;
    KeyCode disabledKeybind; // TODO: Only one for now, but future implementation could allow more -- dtroupe

    void Update()
    {
        foreach (Keybind interaction in interactions)
        {
            if (Input.GetKeyDown(interaction.keybind) && interaction.keybind != disabledKeybind)
            {
                interaction.actions.Invoke();
            }
        }
    }

    public void DisableKeybind(KeyCode keybind)
    {
        disabledKeybind = keybind;
    }

    public void EnableKeybind(KeyCode keybind)
    {
        if (disabledKeybind == keybind)
        {
            disabledKeybind = KeyCode.None;
        }
    }
}