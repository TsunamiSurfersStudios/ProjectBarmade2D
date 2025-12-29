using UnityEngine;

public class HoldableObject : MonoBehaviour
{
    protected ItemHolder itemHolder;
    public GameObject item;

    protected void Start()
    {
        if (itemHolder == null)
        {
            itemHolder = GameObject.FindWithTag("Player").GetComponentInChildren<ItemHolder>();
        }
    }

    public void GiveToPlayer()
    {
        itemHolder.GiveObject(gameObject);
    }

    public void Spawn()
    {
        GameObject clone = GameObject.Instantiate(item);
        itemHolder.GiveObject(clone);
        clone.SetActive(true);
    }
}