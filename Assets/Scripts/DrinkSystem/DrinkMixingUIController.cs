using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DrinkMakingUIController : MonoBehaviour
{
    [System.Serializable]
    public class UIElement
    {
        public string elementName;
        public int elementMlValue;
        public GameObject uiObject;
        public UnityEvent OnClick;
        public UnityEvent OnHoverEnter;
        public UnityEvent OnHoverExit;
        [HideInInspector] public bool isInteracting = false;
    }

    [Header("UI Elements")]
    [SerializeField] List<UIElement> spiritsButtons = new List<UIElement>();
    [SerializeField] List<UIElement> mixerButtons = new List<UIElement>();
    [SerializeField] List<UIElement> garnishButtons = new List<UIElement>();
    [SerializeField] UIElement iceContainer;  
    [SerializeField] UIElement createButton;
    [SerializeField] UIElement resetButton;

    //TODO: Make all these get populated on their own
    [Header("Ice Values")]
    [SerializeField] private GameObject iceTray;
    [SerializeField] private GameObject iceSprite;
    [SerializeField] private Transform iceTrayUI;
    [SerializeField] private float spriteOffset = 30f;//Margin between individual lime/ice sprites positions
    private float iceTrayVolume;

    [Header("Lime Values")]
    [SerializeField] private int limesVolume;
    [SerializeField] private GameObject limeSprite;
    [SerializeField] private Transform limeTrayUI;
    [SerializeField] private bool iceSelected;

    //Drink Controller
    DrinkController drinkController;
    void Start()
    {
        //Draw ice and lime tray sprites
        DrawSprites();
        // Setup all spirits buttons
        SetupUIElements(spiritsButtons, OnSpiritHoverEnter, OnSpiritHoverExit, "spirit");
        
        // Setup all mixer buttons
        SetupUIElements(mixerButtons, OnMixerHoverEnter, OnMixerHoverExit, "mixer");

        // Setup garnish buttons
        SetupUIElements(garnishButtons, OnGarnishHoverEnter, OnGarnishHoverExit, "garnish");

        // Setup ice container
        if (iceContainer != null && iceContainer.uiObject != null)
        {
            SetupSingleElement(iceContainer, () => OnIceHoverEnter(iceContainer), () => OnIceHoverExit(iceContainer), "ice");
        }

        // Setup create button
        if (createButton != null && createButton.uiObject != null)
        {
            SetupSingleElement(createButton, null, null, "create");
        }

        // Setup reset button
        if (resetButton != null && resetButton.uiObject != null)
        {
            SetupSingleElement(resetButton, null, null, "reset");
        }

        // Get DrinkController reference
        drinkController = GetComponent<DrinkController>();
    }

    void OnEnable()
    {
        // Redraw sprites when UI is enabled
        DrawSprites();
    }

    //Draw the ice and lime sprites in their respective trays
    public void DrawSprites()
    {
        iceTrayVolume = iceTray.GetComponent<IceTray>().GetVolume();
        //Spawn ice cubes in the tray based on the ice tray volume
        PopulateTray(iceTrayVolume, iceTrayUI, iceSprite);

        //Spawn limes in the tray based on the lime tray volume
        PopulateTray(limesVolume, limeTrayUI, limeSprite);
    }

    //Populate the ice tray UI with ice cube sprites based on the volume of ice in the tray
    private void PopulateTray(float volume, Transform UI, GameObject sprite)
    {
        float containerWidth = UI.GetComponent<RectTransform>().rect.width;
        float containerHeight = UI.GetComponent<RectTransform>().rect.height;
        float xCounter = 0f;
        float yCounter = 0f;
        Vector3 position = sprite.transform.position;
        Vector3 ogPosition = position;
        //Generate as many ice cubes as there are ice in ice tray
        for (float i = 0f; i < volume; i += 2.5f)
        {
            if (xCounter < containerWidth)
            {//Fill line with ice
                float randomRotation = Random.Range(0f, 360f);
                GameObject obj = Instantiate(sprite, position, Quaternion.Euler(0, 0, randomRotation), UI);
                position.x += spriteOffset;
                obj.SetActive(true);
                xCounter += spriteOffset;
            }
            else if (yCounter < containerHeight)
            {//Start generating ice on a new line
                yCounter += spriteOffset;
                position.y += spriteOffset;
                position.x = ogPosition.x;
                xCounter = 0f;
            }
        }
    }

    void SetupUIElements(List<UIElement> elements, UnityAction<UIElement> enterCallback, UnityAction<UIElement> exitCallback, string type)
    {
        foreach (var element in elements)
        {
            if (element.uiObject != null)
            {
                SetupSingleElement(element, () => enterCallback(element), () => exitCallback(element), type);
            }
        }
    }

    void SetupSingleElement(UIElement element, UnityAction enterCallback, UnityAction exitCallback, string type = "")
    {
        // Add event trigger component if it doesn't exist
        EventTrigger trigger = element.uiObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = element.uiObject.AddComponent<EventTrigger>();
        }

        if(type != "create" && type != "reset")
        {
            // Add hover enter event
            EventTrigger.Entry enterEntry = new EventTrigger.Entry();
            enterEntry.eventID = EventTriggerType.PointerEnter;
            enterEntry.callback.AddListener((data) => { enterCallback.Invoke(); });
            trigger.triggers.Add(enterEntry);

            // Add hover exit event
            EventTrigger.Entry exitEntry = new EventTrigger.Entry();
            exitEntry.eventID = EventTriggerType.PointerExit;
            exitEntry.callback.AddListener((data) => { exitCallback.Invoke(); });
            trigger.triggers.Add(exitEntry);
        }

        if (type == "spirit")
        {
            // Load spirit ingredient from Resources and add to drink
            Ingredient ingredient = Resources.Load<Ingredient>($"Bar/Ingredients/Spirits/{element.elementName}");
            if (ingredient != null)
            {
                int ml = element.elementMlValue;
                AddClickTriger(element, () => drinkController.AddIngredient(ingredient, ml));
            }
            else
            {
                Debug.LogWarning($"Spirit ingredient not found: Bar/Ingredients/Spirits/{element.elementName}");
            }
        }
        else if (type == "mixer")
        {
            // Load mixer ingredient from Resources and add to drink
            Ingredient ingredient = Resources.Load<Ingredient>($"Bar/Ingredients/Mixers/{element.elementName}");
            if (ingredient != null)
            {
                int ml = element.elementMlValue;
                AddClickTriger(element, () => drinkController.AddIngredient(ingredient, ml));
            }
            else
            {
                Debug.LogWarning($"Mixer ingredient not found: Bar/Ingredients/Mixers/{element.elementName}");
            }
        }
        else if (type == "garnish")
        {
            // Load garnish ingredient from Resources and add to drink
            Ingredient ingredient = Resources.Load<Ingredient>($"Bar/Ingredients/Garnishes/{element.elementName}");
            if (ingredient != null)
            {
                AddClickTriger(element, () => drinkController.AddGarnish(ingredient));
            }
            else
            {
                Debug.LogWarning($"Garnish ingredient not found: Bar/Ingredients/Garnishes/{element.elementName}");
            }
        } else if (type == "ice")
        {
            // Add ice to drink on click
            AddClickTriger(element, () => drinkController.AddIce());
        } else if (type == "create") 
        {
            // Create drink on click
            AddClickTriger(element, () => drinkController.SpawnDrink());
        } else if (type == "reset")
        {
            // Reset drink on click
            AddClickTriger(element, () => drinkController.ResetDrink());
        }
    }

    //TODO: Buy all in 1 shader from unity asset store to highlight buttons on hover

    void AddClickTriger(UIElement element, UnityAction clickCallback)
    {
        if (element == null || element.uiObject == null) return;

        // Add event trigger component if it doesn't exist
        EventTrigger trigger = element.uiObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = element.uiObject.AddComponent<EventTrigger>();
        }

        // Add click event
        EventTrigger.Entry clickEntry = new EventTrigger.Entry();
        clickEntry.eventID = EventTriggerType.PointerClick;
        clickEntry.callback.AddListener((data) => { clickCallback.Invoke(); });
        trigger.triggers.Add(clickEntry);
    }
    
    void SetHighlight(UIElement element, bool highlight)
    {
        if (element == null || element.uiObject == null) return;
        
        // Highlight bottle images by changing their color
        var image = element.uiObject.GetComponent<UnityEngine.UI.Image>();
        
        if (image != null)
        {
            image.color = highlight ? Color.yellow : Color.white;
        }
        else
        {
            // Highlight all images inside of the containers
            var images = element.uiObject.GetComponentsInChildren<UnityEngine.UI.Image>();
            foreach (var img in images)
            {
                img.color = highlight ? Color.yellow : Color.white;
            }
        }
    }
    
    void OnSpiritHoverEnter(UIElement element)
    {
        SetHighlight(element, true);
    }

    void OnSpiritHoverExit(UIElement element)
    {
        SetHighlight(element, false);
    }

    void OnMixerHoverEnter(UIElement element)
    {
        SetHighlight(element, true);
    }

    void OnMixerHoverExit(UIElement element)
    {
        SetHighlight(element, false);
    }
    void OnGarnishHoverEnter(UIElement element)
    {
        SetHighlight(element, true);
    }
    void OnGarnishHoverExit(UIElement element)
    {
        SetHighlight(element, false);
    }
    void OnIceHoverEnter(UIElement element)
    {
        SetHighlight(element, true);
    }
    void OnIceHoverExit(UIElement element)
    {
        SetHighlight(element, false);
    }
}