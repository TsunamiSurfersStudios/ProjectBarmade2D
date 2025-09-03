using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Ice : MonoBehaviour, IPointerClickHandler
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void OnPointerClick(PointerEventData eventData)
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
