using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public enum UIElementType
{
    None,
    Spirit,
    Mixer,
    Garnish,
    Glass,
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

    [System.Serializable]
    public class ArrowPair
    {
        public UIElement leftArrow;
        public UIElement rightArrow;
        [HideInInspector] public int currentPageIndex = 0;
        public int itemsPerPage = 4;
    }

    [Header("UI Elements")]
    [SerializeField] List<UIElement> spiritsButtons = new List<UIElement>();
    [SerializeField] List<UIElement> mixerButtons = new List<UIElement>();
    [SerializeField] List<UIElement> garnishButtons = new List<UIElement>();
    [SerializeField] List<UIElement> glassButtons = new List<UIElement>();
    [SerializeField] UIElement iceContainer;
    [SerializeField] UIElement createButton;
    [SerializeField] UIElement resetButton;
    [SerializeField] private Text hoveredElementText;
    [SerializeField] private Text ingredientsInDrinkText;
    [SerializeField] private Text glassTypeText;

    [Header("Page Navigation")]
    [SerializeField] private ArrowPair spiritArrows = new ArrowPair { itemsPerPage = 4 };
    [SerializeField] private ArrowPair mixerArrows = new ArrowPair { itemsPerPage = 3 }; 
    [SerializeField] private ArrowPair glassArrows = new ArrowPair { itemsPerPage = 1}; 

    //TODO: Make all these get populated on their own
    [Header("Ice Values")]
    [SerializeField] private GameObject iceTray;
    [SerializeField] private GameObject iceSprite;
    [SerializeField] private Transform iceTrayUI;
    const float spriteOffset = 30f;//Margin between individual lime/ice sprites positions
    private float iceTrayVolume;

    [Header("Lime Values")]
    [SerializeField] private int limesVolume;
    [SerializeField] private GameObject limeSprite;
    [SerializeField] private Transform limeTrayUI;
    [SerializeField] private bool iceSelected;

    //Drink Controller
    DrinkMixingService drinkMixingService;

    private UIElement currentActiveGlass;
    private Glass currentActiveGlassType = Glass.MARTINI;
    string PATH = "Bar/Ingredients/";
    private Dictionary<UIElementType, string> subfolderMap = new Dictionary<UIElementType, string>()
    {
        { UIElementType.Spirit, "Spirits" },
        { UIElementType.Mixer, "Mixers" },
        { UIElementType.Garnish, "Garnishes" }
    };
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
        drinkMixingService = GetComponent<DrinkMixingService>();

        if (drinkMixingService == null)
        {
            Debug.LogError("DrinkMixingService component not found on GameObject '" + gameObject.name + "'. DrinkMakingUIController requires a DrinkController to function correctly.", this);
        }
        // Initalize new drink
        drinkMixingService.StartNewDrink();

        // Setup spirit arrows
        SetupArrowPair(spiritArrows, spiritsButtons);

        // Setup mixer arrows
        SetupArrowPair(mixerArrows, mixerButtons);

        //Setup glass arrows
        SetupArrowPair(glassArrows, glassButtons);

        // Initialize pages to show first page
        UpdatePage(spiritArrows, spiritsButtons);
        UpdatePage(mixerArrows, mixerButtons);
        UpdatePage(glassArrows, glassButtons);

        
    }

    #region UI Bottle Elements
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

        //Replace space to properly find ingredient in Resources
        var ingredientName = "";
        if (element.elementName.Contains(" ")) 
        {
            ingredientName = element.elementName.Replace(" ", "");
        } else {
            ingredientName = element.elementName;
        }

        switch (type)
        {
            case UIElementType.Spirit:
            case UIElementType.Mixer:
            case UIElementType.Garnish:
                string localPath = PATH + subfolderMap.GetValueOrDefault(type, "");
                Ingredient ingredient = Resources.Load<Ingredient>($"{localPath}/{ingredientName}");
                if (ingredient != null)
                {
                    int ml = element.elementMlValue;
                    AddClickTrigger(element, () => { ProcessClick(ingredient, ml); });
                }
                else
                {
                    Debug.LogWarning($"{type.ToString()} ingredient not found: {localPath}/{ingredientName}");
                }
                break;
            default:
                AddClickTrigger(element, () => { ProcessClick(type); });
                break;
        }
    }
    void ProcessClick(UIElementType type)
    {
        switch (type)
        {
            case UIElementType.Ice:
                drinkMixingService.AddIce();
                break;
            case UIElementType.Create:
                drinkMixingService.FinishDrink();
                break;
            case UIElementType.Reset:
                drinkMixingService.StartNewDrink();
                break;
        }
        RefreshIngredientsText();
    }
    void ProcessClick(Ingredient ingredient, int amount)
    {
        drinkMixingService.AddIngredient(ingredient, amount);
        RefreshIngredientsText();
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
    #endregion

    #region Page Navigation

    void SetupArrowPair(ArrowPair arrows, List<UIElement> items)
    {
        if (arrows == null) 
        {
            return;
        }

        // Setup left arrow with hover and click
        if (arrows.leftArrow != null && arrows.leftArrow.uiObject != null)
        {
            SetupSingleElement(arrows.leftArrow, () => OnHoverEnter(arrows.leftArrow), () => OnHoverExit(arrows.leftArrow), UIElementType.None);
            AddClickTrigger(arrows.leftArrow, () => OnArrowClick(arrows, -1, items));
        }

        // Setup right arrow with hover and click
        if (arrows.rightArrow != null && arrows.rightArrow.uiObject != null)
        {
            SetupSingleElement(arrows.rightArrow, () => OnHoverEnter(arrows.rightArrow), () => OnHoverExit(arrows.rightArrow), UIElementType.None);
            AddClickTrigger(arrows.rightArrow, () => OnArrowClick(arrows, 1, items));
        }
    }

    void OnArrowClick(ArrowPair arrows, int direction, List<UIElement> items)
    {
        if (arrows == null || items == null || items.Count == 0)
        {
            return;
        }

        int maxPageIndex = GetMaxPageIndex(items.Count, arrows.itemsPerPage);
        arrows.currentPageIndex = Mathf.Clamp(arrows.currentPageIndex + direction, 0, maxPageIndex);
        UpdatePage(arrows, items);
    }

    void UpdatePage(ArrowPair arrows, List<UIElement> items)
    {
        if (arrows == null || items == null) 
        {
            return;
        }

        int startIndex = arrows.currentPageIndex * arrows.itemsPerPage;
        int endIndex = startIndex + arrows.itemsPerPage;

        // Show/hide items based on current page
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null && items[i].uiObject != null)
            {
                bool shouldShow = i >= startIndex && i < endIndex;
                items[i].uiObject.SetActive(shouldShow);
            }
        }

        // Update arrow visibility
        UpdateArrowVisibility(arrows, items.Count);

        // Auto-select glass
        if (arrows == glassArrows && items == glassButtons)
        {
            // Get the currently visible glass
            for (int i = startIndex; i < endIndex && i < items.Count; i++)
            {
                if (items[i] != null && items[i].uiObject != null)
                {
                    SetActiveGlass(items[i]);
                    break; // Only one glass visible at a time
                }
            }
        }
    }

    void UpdateArrowVisibility(ArrowPair arrows, int totalItems)
    {
        if (arrows == null) return;

        int maxPageIndex = GetMaxPageIndex(totalItems, arrows.itemsPerPage);

        // Hide left arrow on first page
        if (arrows.leftArrow != null && arrows.leftArrow.uiObject != null)
        {
            arrows.leftArrow.uiObject.SetActive(arrows.currentPageIndex > 0);
        }

        // Hide right arrow on last page
        if (arrows.rightArrow != null && arrows.rightArrow.uiObject != null)
        {
            arrows.rightArrow.uiObject.SetActive(arrows.currentPageIndex < maxPageIndex);
        }
    }

    int GetMaxPageIndex(int totalItems, int itemsPerPage)
    {
        if (itemsPerPage <= 0) return 0;
        return Mathf.Max(0, Mathf.CeilToInt((float)totalItems / itemsPerPage) - 1);
    }

    #endregion

    #region Glass Selection

    public void SetActiveGlass(UIElement glass)
    {
        currentActiveGlass = glass;
        
        // Convert glass name string to Glass enum
        if (glass != null && System.Enum.TryParse<Glass>(glass.elementName.ToUpper(), out Glass glassType))
        {
            currentActiveGlassType = glassType;
        }
        else
        {
            currentActiveGlassType = Glass.MARTINI;
        }
        
        if (glassTypeText != null && glass != null)
        {
            glassTypeText.text = glass.elementName;
        }

        // Update DrinkMixingService with the selected glass
        if (drinkMixingService != null && glass != null)
        {
            drinkMixingService.SelectGlass(currentActiveGlassType);
        }
    }

    #endregion
    
    #region Highlighting UI Elements and text description
    void SetHighlight(UIElement element, bool highlight)
    {
        if (element == null || element.uiObject == null) return;
        
        // Highlight bottle images by changing their color
        var image = element.uiObject.GetComponent<UnityEngine.UI.Image>();
        
        if (image != null)
        {
            image.color = highlight ? Color.yellow : Color.white;
            hoveredElementText.text = highlight ? element.elementName : "";
        }
        else
        {
            // Highlight all images inside of the containers
            var images = element.uiObject.GetComponentsInChildren<UnityEngine.UI.Image>();
            foreach (var img in images)
            {
                img.color = highlight ? Color.yellow : Color.white;
            }
            hoveredElementText.text = highlight ? element.elementName : "";
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

    public void RefreshIngredientsText()
    {
        if (drinkMixingService != null && ingredientsInDrinkText != null)
        {
            ingredientsInDrinkText.text = "Ingredients: " + GetCurrentDrinkIngredients();
        }
    }

    void OnEnable()
    {
        // Redraw sprites when UI is enabled
        DrawSprites();
    }

    
    public string GetCurrentDrinkIngredients()
    {
        //TODO: Show amounts in a pretty way. Right now looks like ass
        string ingredients = "";
        DrinkController drink = drinkMixingService.GetDrink();
        if (drink == null)
        {
            return ingredients;
        }
        foreach (DrinkComponent spirit in drink.GetSpirits())
        {
            ingredients += spirit.GetIngredient().GetName() + "\n";
        }
        foreach (DrinkComponent mixer in drink.GetMixers())
        {
            ingredients += mixer.GetIngredient().GetName() + "\n";
        }
        foreach (Ingredient garnish in drink.GetGarnishes())
        {
            ingredients += garnish.GetName() + "\n";
        }
        if (drink.HasIce())
        {
            ingredients += "Ice\n";
        }

        return ingredients;
    }
    #endregion
    
    #region Ice and Lime Tray UI
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
    #endregion
}