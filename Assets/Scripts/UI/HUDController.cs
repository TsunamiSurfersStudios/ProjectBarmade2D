using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] GameObject interactionText;

    public void ShowInteractionText(string keybind = "E")
    {
        TextMeshProUGUI textMeshPro = interactionText.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = "Press " + keybind + " to use station";
        interactionText.SetActive(true);
    }

    public void HideInteractionText()
    {
        interactionText.SetActive(false);
    }
}
