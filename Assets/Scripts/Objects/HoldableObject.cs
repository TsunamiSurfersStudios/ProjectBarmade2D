using UnityEngine;

public class HoldableObject : MonoBehaviour
{
    protected ItemHolder itemHolder;
    protected GameObject item;

    void Start()
    {
        itemHolder = GameObject.FindWithTag("Player").GetComponentInChildren<ItemHolder>();
    }

    protected void Give()
    {
        itemHolder.GiveObject(gameObject);
    }

    protected void Spawn()
    {
        GameObject clone = GameObject.Instantiate(item);
        itemHolder.GiveObject(clone);
        clone.SetActive(true);
    }
}