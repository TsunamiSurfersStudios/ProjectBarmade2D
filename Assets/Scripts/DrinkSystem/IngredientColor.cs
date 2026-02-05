using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IngredientColor : MonoBehaviour
{
    [SerializeField] Color32 ingredientColor;
    private Vector3 startPos;
    private DrinkBucket drinkBucket;
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().color = ingredientColor;
        startPos = transform.position;
        drinkBucket = FindObjectOfType<DrinkBucket>();
    }

    private void OnMouseDrag()
    {
        Vector3 mousePositionScreen = Input.mousePosition;
        Vector3 mousePositionWorld = Camera.main.ScreenToWorldPoint(mousePositionScreen);
        mousePositionWorld.z = 0f;
        transform.position = mousePositionWorld;
    }

    public void OnMouseUp()
    {
        CircleCollider2D boxCollider = gameObject.GetComponent<CircleCollider2D>();
        Collider2D[] overlap = Physics2D.OverlapAreaAll(boxCollider.bounds.min, boxCollider.bounds.max);
        foreach (Collider2D col in overlap)
        {
            if (col.gameObject.GetComponent<DrinkBucket>() != null)
            {
                drinkBucket = col.gameObject.GetComponent<DrinkBucket>();
                drinkBucket.AddColor(ingredientColor);
                break;
            }
        }
        transform.position = startPos;
    } 
}
