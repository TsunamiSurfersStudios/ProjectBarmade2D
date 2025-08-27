using UnityEngine;

public class HoldableObject : MonoBehaviour
{
    protected ItemHolder itemHolder;
    protected GameObject item;

    protected void Give()
    {
        FindItemHolder();
        itemHolder.GiveObject(gameObject);
    }

    protected void Spawn()
    {
        FindItemHolder();
        GameObject clone = GameObject.Instantiate(item);
        itemHolder.GiveObject(clone);
        clone.SetActive(true);
    }

    private void FindItemHolder()
    {
        if (itemHolder == null)
        {
            itemHolder = GameObject.FindWithTag("Player").GetComponentInChildren<ItemHolder>();
        }
    }
}