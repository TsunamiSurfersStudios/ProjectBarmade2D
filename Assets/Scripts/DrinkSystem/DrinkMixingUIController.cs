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

    // Current selections
    private string selectedSpirit = "";
    private string selectedMixer = "";
    private string selectedGarnish = "";
    private bool iceSelected = false;

    //Drink Controller
    DrinkController drinkController;
    void Start()
    {
        // Setup all spirits buttons
        SetupUIElements(spiritsButtons, OnSpiritClick);
        
        // Setup all mixer buttons
        SetupUIElements(mixerButtons, OnMixerClick);

        // Setup garnish buttons
        SetupUIElements(garnishButtons, OnGarnishClick);

        // Setup ice container
        if (iceContainer != null && iceContainer.uiObject != null)
        {
            SetupSingleElement(iceContainer, OnIceClick);
        }
        // Setup create button
        if (createButton != null && createButton.uiObject != null)
        {
            SetupSingleElement(createButton, OnCreateDrinkClick);
        }

        // Get DrinkController reference
        drinkController = GetComponent<DrinkController>();
    }

    void SetupUIElements(List<UIElement> elements, UnityAction<UIElement> clickCallback)
    {
        foreach (var element in elements)
        {
            if (element.uiObject != null)
            {
                SetupSingleElement(element, () => clickCallback(element));
            }
        }
    }

    void SetupSingleElement(UIElement element, UnityAction clickCallback)
    {
        // Add event trigger component if it doesn't exist
        EventTrigger trigger = element.uiObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = element.uiObject.AddComponent<EventTrigger>();
        }

        // Clear existing triggers to avoid duplicates
        trigger.triggers.Clear();

        // Add click event
        EventTrigger.Entry clickEntry = new EventTrigger.Entry();
        clickEntry.eventID = EventTriggerType.PointerClick;
        clickEntry.callback.AddListener((data) => { clickCallback.Invoke(); });
        trigger.triggers.Add(clickEntry);

        // Add hover enter event
        EventTrigger.Entry enterEntry = new EventTrigger.Entry();
        enterEntry.eventID = EventTriggerType.PointerEnter;
        enterEntry.callback.AddListener((data) => { element.OnHoverEnter.Invoke(); });
        trigger.triggers.Add(enterEntry);

        // Add hover exit event
        EventTrigger.Entry exitEntry = new EventTrigger.Entry();
        exitEntry.eventID = EventTriggerType.PointerExit;
        exitEntry.callback.AddListener((data) => { element.OnHoverExit.Invoke(); });
        trigger.triggers.Add(exitEntry);
    }

    void OnSpiritClick(UIElement element)
    {
        // Deselect all other spirit buttons
        foreach (var spirit in spiritsButtons)
        {
            spirit.isInteracting = false;
        }

        // Select spirit
        element.isInteracting = true;
        selectedSpirit = element.elementName;
        
        element.OnClick.Invoke();
        Debug.Log($"Selected spirit: {element.elementName}");


    }

    void OnMixerClick(UIElement element)
    {
        foreach (var mixer in mixerButtons)
        {
            mixer.isInteracting = false;
        }
        selectedMixer = "";
        
        element.isInteracting = true;
        selectedMixer = element.elementName;
        
        element.OnClick.Invoke();
        Debug.Log($"Selected mixer: {selectedMixer}");
    }

    void OnGarnishClick(UIElement element)
    {
        foreach (var garnish in garnishButtons)
        {
            garnish.isInteracting = false;
        }
        
        element.isInteracting = true;
        selectedGarnish = element.elementName;
        
        element.OnClick.Invoke();
        Debug.Log($"Selected garnish: {element.elementName}");
    }

    void OnIceClick()
    {
        iceSelected = !iceSelected;
        iceContainer.isInteracting = iceSelected;
        
        iceContainer.OnClick.Invoke();
        Debug.Log($"Ice selected: {iceSelected}");
    }

    void OnCreateDrinkClick()
    {
        if (string.IsNullOrEmpty(selectedSpirit) && string.IsNullOrEmpty(selectedMixer))
        {
            Debug.LogWarning("No spirit or mixer selected!");
            return;
        }

        createButton.OnClick.Invoke();
        
        Debug.Log("=== Creating Drink ===");
        Debug.Log($"Spirit: {selectedSpirit}");
        Debug.Log($"Mixer: {selectedMixer}");
        Debug.Log($"Ice: {iceSelected}");
        Debug.Log($"Garnish: {selectedGarnish}");
        Debug.Log("======================");

        // Create the drink
        //CreateDrink(selectedSpirit, selectedMixer, iceSelected);
    }

    void CreateDrink(string spirit, string mixer, string ice)
    {
        // TODO: Call drink creating from drink controller
    }
    public string GetSelectedSpirit() => selectedSpirit;
    public string GetSelectedMixer() => selectedMixer;
    public string GetSelectedGarnish() => selectedGarnish;
    public bool GetIceSelected() => iceSelected;


    //!Might not need these functions on the bottom. Decide before pushing to main
    // check if a specific element is selected
    private bool IsElementSelected(string elementName)
    {
        // Check spirits
        var spirit = spiritsButtons.Find(e => e.elementName == elementName);
        if (spirit != null) return spirit.isInteracting;

        // Check mixers
        var mixer = mixerButtons.Find(e => e.elementName == elementName);
        if (mixer != null) return mixer.isInteracting;

        // Check ice
        var garnish = garnishButtons.Find(e => e.elementName == elementName);
        if (garnish != null) return garnish.isInteracting;

        var ice = iceContainer;
        if (ice != null && ice.elementName == elementName) return ice.isInteracting;

        return false;
    }

    // Reset all selections
    private void ResetSelections()
    {
        selectedSpirit = "";
        selectedMixer = "";
        selectedGarnish = "";

        foreach (var spirit in spiritsButtons)
            spirit.isInteracting = false;
        
        foreach (var mixer in mixerButtons)
            mixer.isInteracting = false;
        
        foreach (var garnish in garnishButtons)
            garnish.isInteracting = false;

        Debug.Log("All selections reset");
    }
}