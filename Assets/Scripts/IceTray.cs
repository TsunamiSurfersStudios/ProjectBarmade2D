using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTray : MonoBehaviour
{
    float trayVolume = 0f; 
    float trayCapacity = 100f;
    [SerializeField] private Animator animator;


    void Update()
    {
        if (!animator)
        {
            return;
        }
        ShowAnimatorState();
    }

    void ShowAnimatorState()
    {
        if (trayVolume <= 100f && trayVolume > 50f)
        {
            animator.SetBool("isHalfEmpty", false); // TODO: This can definetley be optimized. Also this should be in a function not in the update function
            animator.SetBool("isFull", true);
        }
        else if (trayVolume <= 50f && trayVolume > 0f)
        {
            animator.SetBool("isFull", false);
            animator.SetBool("isHalfEmpty", true);
        }
        else
        {
            animator.SetBool("isHalfEmpty", false);
            animator.SetBool("isFull", false);
        }
    }
    public float GetVolume()
    { 
        return trayVolume; 
    }

    public void CheckForIce()
    {
        ItemHolder holder = GameObject.FindWithTag("Player").GetComponentInChildren<ItemHolder>();
        GameObject ice = holder.TakeObject();
        if (ice && ice.name == "Ice" )
        {
            Destroy(ice);
            RefillTray();
        }
        else
        {
            holder.GiveObject(ice);
        }
           
    }
    public void RefillTray(float amount = 100) 
    {
        trayVolume += amount;
        if (trayVolume > trayCapacity)
        {
            trayVolume = trayCapacity;
        }
        Debug.Log("Ice Tray Refilled");
    }

    public void EmptyTray(float amount = 100)
    {
        trayVolume -= amount;
        if (trayVolume < 0)
        {
            trayVolume = 0;
        }
    }
}
