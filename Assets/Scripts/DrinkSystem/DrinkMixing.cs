using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DrinkMixing : MonoBehaviour
{
    [Header("Ice Values")]
    [SerializeField] private GameObject iceTray;
    [SerializeField] private GameObject iceSprite;
    [SerializeField] private Transform iceTrayUI;
    [SerializeField] private float spriteOffset = 30f;//Margin between individual lime/ice sprites positions
    private float iceTrayVolume;

    [Header("Ice Values")]
    [SerializeField] private int limesVolume;
    [SerializeField] private GameObject limeSprite;
    [SerializeField] private Transform limeTrayUI;

    [SerializeField] private DrinkComponent selectedDrink;
    [SerializeField] private Ingredient selectedGarnish;
    [SerializeField] private bool iceSelected;

    // Start is called before the first frame update
    void Start()
    {
        drawSprites();
    }

    void OnEnable()
    {
        drawSprites();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSelectedDrink(DrinkComponent drink) { selectedDrink = drink; }
    public DrinkComponent GetSelectedDrink() { return selectedDrink; }
    public void SetSelectedGarnish(Ingredient garnish) { selectedGarnish = garnish; }
    public Ingredient GetSelectedGarnish() { return selectedGarnish; }
    public bool GetIceSelected() { return iceSelected; }
    public void SetIceSelected(bool ice) { iceSelected = ice; }

    //Draw the ice and lime sprites in their respective trays
    public void drawSprites()
    {
        iceTrayVolume = iceTray.GetComponent<IceTray>().GetVolume();
        //Spawn ice cubes in the tray based on the ice tray volume
        populateTray(iceTrayVolume, iceTrayUI, iceSprite);

        //Spawn limes in the tray based on the lime tray volume
        populateTray(limesVolume, limeTrayUI, limeSprite);
    }

    //Populate the ice tray UI with ice cube sprites based on the volume of ice in the tray
    private void populateTray(float volume, Transform UI, GameObject sprite)
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
}
