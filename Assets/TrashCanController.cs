using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCanController : MonoBehaviour
{
    [SerializeField] int MAXFILL = 100;
    [SerializeField] int TRASHPERINTERACTION = 10;
    [SerializeField] GameObject trashBag;

    int currentFill = 0;
    Animator mAnimator;
    ItemHolder itemHolder;
    void Start()
    {
        mAnimator = GetComponent<Animator>();
        if (!mAnimator)
        {
            Debug.LogWarning("TrashCanController: No animator found on " + gameObject.name);
        }
        itemHolder = GameObject.FindFirstObjectByType<ItemHolder>();
        if (!itemHolder)
        {
            Debug.LogWarning("TrashCanController: No ItemHolder found in scene.");
        }

    }

    public void UseTrash()
    {
        if (itemHolder && itemHolder.IsEmpty())
        {
            EmptyTrash();
        }
        else
        {
            AddTrash();
        }
    }

    void AddTrash()
    {
        itemHolder.DestroyObject();
        currentFill += TRASHPERINTERACTION;
        if (currentFill >= MAXFILL)
        {
            mAnimator.SetBool("isFull", true);
            Debug.Log("Trash can is full");
        }
    }
    
    void EmptyTrash()
    {
        itemHolder.Spawn(trashBag);
        currentFill = 0;
        mAnimator.SetBool("isFull", false);
    }
}
