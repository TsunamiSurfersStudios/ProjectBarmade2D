using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DrinkSystem
{
    [Serializable]
    public class BeerInfo
    {
        public GameObject image;
        public Ingredient ingredient;
    }
    public class BeerMachineUIController : MonoBehaviour
    {
        DrinkMixingService drinkMixingService;
        [SerializeField] List<BeerInfo> beerInfos;
        [SerializeField] UnityEvent OnClickComplete;
        void Start()
        {
            drinkMixingService = gameObject.GetComponent<DrinkMixingService>();
            if (drinkMixingService == null)
            {
                Debug.LogError("DrinkMixingService component not found on BeerMachineUIController Gameobj.");
            }

            foreach (BeerInfo beerInfo in beerInfos)
            {
                AddClickTrigger(beerInfo.image, () => OnClick(beerInfo.ingredient));
            }
        }

        void OnClick(Ingredient ingredient)
        {
            drinkMixingService.StartNewDrink();
            drinkMixingService.AddIngredient(ingredient);
            drinkMixingService.FinishDrink();
            OnClickComplete.Invoke();
        }

        void AddClickTrigger(GameObject obj, UnityAction callback)
        {
            EventTrigger trigger = obj.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = obj.AddComponent<EventTrigger>();
            }

            // Create a new PointerClick entry
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((data) => { callback.Invoke(); });
            trigger.triggers.Add(entry);
        }
    }
}