using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class TrashBag : MonoBehaviour
{
    private ItemHolder itemHolder;
    private PlayerStats playerStats;
    [SerializeField] GameObject bag; // This contains the trash bag sprite
    bool touchingBag;
    bool touchingDumpster;

    void Start()
    {
        itemHolder = GameObject.FindWithTag("Player").GetComponentInChildren<ItemHolder>();
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && touchingBag && playerStats.getHolding() == false)
        {
            GiveBag();
            playerStats.changeHolding();
        }
        else if(Input.GetKeyDown(KeyCode.E) && touchingDumpster && playerStats.getHolding())
        {
            DestroyBag();
            playerStats.changeHolding();
        }        
        else if(Input.GetKeyDown(KeyCode.F) && playerStats.getHolding())
        {
            DropBag();
            playerStats.changeHolding();
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Dumpster"))
        {
            touchingDumpster = true;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        touchingBag = false;
        touchingDumpster = false;
    }
    
    public void GiveBag()
    {
        itemHolder.GiveObject(gameObject); // This assume script is attatched to current bag
    }

    public void DropBag()
    {
        itemHolder.DropItem();
    }

    public void SpawnBag()
    {
        GameObject clone = GameObject.Instantiate(bag);
        itemHolder.GiveObject(clone);
        clone.SetActive(true);
    }

    public void DestroyBag()
    {
        itemHolder.DestroyObject();
    }
}