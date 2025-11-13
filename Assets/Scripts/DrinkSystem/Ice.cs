using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Ice : MonoBehaviour
{   
    public void SelectIce()
    {
        DrinkMixing drinkMakingStation = FindObjectOfType<DrinkMixing>(true);
        IceTray iceTray= FindObjectOfType<IceTray>(true);
        if (iceTray.GetVolume() > 0)
        {
            Debug.Log("Took some ice");
            drinkMakingStation.GetComponent<DrinkMixing>().SetIceSelected(true);
            iceTray.EmptyTray(10f);
        }
        else
        {
            Debug.Log("Not enough ice");
        }
    }
}
