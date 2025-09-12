using UnityEngine;

public class HoldableObject : MonoBehaviour
{
    protected ItemHolder itemHolder;
    protected GameObject item;

    protected void Start()
    {
        if (itemHolder == null)
        {
            itemHolder = GameObject.FindWithTag("Player").GetComponentInChildren<ItemHolder>();
        }
    }

    protected void Give()
    {  
        itemHolder.GiveObject(gameObject);
    }

    // Overload with the setupAction parameter
    protected void Spawn(System.Action<GameObject> drinkData)
    {
        GameObject clone = GameObject.Instantiate(item);
        drinkData?.Invoke(clone);
        itemHolder.GiveObject(clone);
        clone.SetActive(true);
    }
}