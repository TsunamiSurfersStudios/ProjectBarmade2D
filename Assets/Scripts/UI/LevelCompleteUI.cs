using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject ScreenDayComplete;
    [SerializeField] RectTransform unlockedIngredientsSection;
    [SerializeField] RectTransform unlockedRecipesSection;
    [SerializeField] TextMeshProUGUI TaglineText;
    [SerializeField] TextMeshProUGUI DrinkAccuracyText;
    [SerializeField] TextMeshProUGUI DrinksMadeText;
    [SerializeField] TextMeshProUGUI MoneyMadeText;

    [Header("Dependencies")]
    [SerializeField] RecipeController recipeController;

    [Header("Spacing Tweaks")]
    [SerializeField] float leftStartingPosX = -50f;
    [SerializeField] float leftStartingPosY = -30f;
    [SerializeField] float spritesPadding = 50f;
    [SerializeField] float spritesScale = 0.10f;

    void Awake()
    {
        GameEventManager.Instance.Subscribe(GameEventManager.GameEvent.LevelComplete, Show);
        ScreenDayComplete.SetActive(false);
    }

    void Show()
    {
        ScreenDayComplete.SetActive(true);
        int tier = GameManager.Instance.currentLevel;
        List<Ingredient> unlockedIngredients = recipeController.GetIngredientsUnlockedAtLevel(tier);
        var unlockedRecipes = recipeController.GetNewlyUnlockedRecipes(tier).ToList<Recipe>();

        PopulateSpritesUI(
            unlockedIngredients.Select(i => i.sprite).ToList(),
            unlockedIngredientsSection,
            leftStartingPosX,
            leftStartingPosY,
            spritesPadding
        );

        PopulateSpritesUI(
            unlockedRecipes.Select(r => r.sprite).ToList(),
            unlockedRecipesSection,
            leftStartingPosX,
            leftStartingPosY,
            spritesPadding
        );
    }

    private void PopulateSpritesUI(
        IList<Sprite> sprites,
        RectTransform parent,
        float startX,
        float startY,
        float padding)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Destroy(parent.GetChild(i).gameObject);
        }

        for (int i = 0; i < sprites.Count; i++)
        {
            GameObject go = new GameObject($"Sprite-{i}", typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false); 

            Image img = go.GetComponent<Image>();
            img.sprite = sprites[i];
            img.preserveAspect = true;

            RectTransform rt = go.GetComponent<RectTransform>();

            Vector2 pos = rt.anchoredPosition;
            pos.x = startX + (padding * i);
            pos.y = startY;
            rt.anchoredPosition = pos;
            rt.localScale = Vector3.one * spritesScale;

        }
    }
}
