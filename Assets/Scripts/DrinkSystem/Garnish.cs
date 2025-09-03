using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Garnish : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Ingredient ingredientType;
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
        drinkMakingStation.GetComponent<DrinkMixing>().SetSelectedGarnish(ingredientType);
    }
}
