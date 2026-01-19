using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public enum UIElementType
{
    None,
    Spirit,
    Mixer,
    Garnish,
    Ice,
    Create,
    Reset
}

public class DrinkMixingUIController : MonoBehaviour
{
    [System.Serializable]
    public class UIElement
    {
        public string elementName;
        public int elementMlValue;
        public GameObject uiObject;
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
        SetupUIElements(spiritsButtons, OnHoverEnter, OnHoverExit, UIElementType.Spirit);
        
        // Setup all mixer buttons
        SetupUIElements(mixerButtons, OnHoverEnter, OnHoverExit, UIElementType.Mixer);

        // Setup garnish buttons
        SetupUIElements(garnishButtons, OnHoverEnter, OnHoverExit, UIElementType.Garnish);

        // Setup ice container
        if (iceContainer != null && iceContainer.uiObject != null)
        {
            SetupSingleElement(iceContainer, () => OnHoverEnter(iceContainer), () => OnHoverExit(iceContainer), UIElementType.Ice);
        }

        // Setup create button
        if (createButton != null && createButton.uiObject != null)
        {
            SetupSingleElement(createButton, null, null, UIElementType.Create);
        }

        // Setup reset button
        if (resetButton != null && resetButton.uiObject != null)
        {
            SetupSingleElement(resetButton, null, null, UIElementType.Reset);
        }

        // Get DrinkController reference
        drinkController = GetComponent<DrinkController>();

        if (drinkController == null)
        {
            Debug.LogError("DrinkController component not found on GameObject '" + gameObject.name + "'. DrinkMakingUIController requires a DrinkController to function correctly.", this);
        }
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
        //Clear existing sprites
        foreach (Transform child in UI)
        {
            Destroy(child.gameObject);
        }

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

    void SetupUIElements(List<UIElement> elements, UnityAction<UIElement> enterCallback, UnityAction<UIElement> exitCallback, UIElementType type)
    {
        foreach (var element in elements)
        {
            if (element.uiObject != null)
            {
                SetupSingleElement(element, () => enterCallback(element), () => exitCallback(element), type);
            }
        }
    }

    void SetupSingleElement(UIElement element, UnityAction enterCallback, UnityAction exitCallback, UIElementType type = UIElementType.None)
    {
        // Add event trigger component if it doesn't exist
        EventTrigger trigger = element.uiObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = element.uiObject.AddComponent<EventTrigger>();
        }

        if (type != UIElementType.Create && type != UIElementType.Reset)
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

        switch (type)
        {
            case UIElementType.Spirit:
            {
                // Load spirit ingredient from Resources and add to drink
                Ingredient ingredient = Resources.Load<Ingredient>($"Bar/Ingredients/Spirits/{element.elementName}");
                if (ingredient != null)
                {
                    int ml = element.elementMlValue;
                    AddClickTrigger(element, () => drinkController.AddIngredient(ingredient, ml));
                }
                else
                {
                    Debug.LogWarning($"Spirit ingredient not found: Bar/Ingredients/Spirits/{element.elementName}");
                }
                break;
            }
            case UIElementType.Mixer:
            {
                // Load mixer ingredient from Resources and add to drink
                Ingredient ingredient = Resources.Load<Ingredient>($"Bar/Ingredients/Mixers/{element.elementName}");
                if (ingredient != null)
                {
                    int ml = element.elementMlValue;
                    AddClickTrigger(element, () => drinkController.AddIngredient(ingredient, ml));
                }
                else
                {
                    Debug.LogWarning($"Mixer ingredient not found: Bar/Ingredients/Mixers/{element.elementName}");
                }
                break;
            }
            case UIElementType.Garnish:
            {
                // Load garnish ingredient from Resources and add to drink
                Ingredient ingredient = Resources.Load<Ingredient>($"Bar/Ingredients/Garnishes/{element.elementName}");
                if (ingredient != null)
                {
                    AddClickTrigger(element, () => drinkController.AddGarnish(ingredient));
                }
                else
                {
                    Debug.LogWarning($"Garnish ingredient not found: Bar/Ingredients/Garnishes/{element.elementName}");
                }
                break;
            }
            case UIElementType.Ice:
                // Add ice to drink on click
                AddClickTrigger(element, () => drinkController.AddIce());
                break;
            case UIElementType.Create:
                // Create drink on click
                AddClickTrigger(element, () => drinkController.SpawnDrink());
                break;
            case UIElementType.Reset:
                // Reset drink on click
                AddClickTrigger(element, () => drinkController.ResetDrink());
                break;
        }
    }

    //TODO: Buy all in 1 shader from unity asset store to highlight buttons on hover

    void AddClickTrigger(UIElement element, UnityAction clickCallback)
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
    
    void OnHoverEnter(UIElement element)
    {
        SetHighlight(element, true);
    }

    void OnHoverExit(UIElement element)
    {
        SetHighlight(element, false);
    }
}