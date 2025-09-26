using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class BrokenGlass : MonoBehaviour
{
    public Transform holdSpot;
    public LayerMask pickUpMask;
    public LayerMask npcMask;
    public Vector3 Direction { get; set; }
    private GameObject itemHolding;
    //bool touchingDrink = false; TODO: Remove line
    private GameObject Player;
    private PlayerMovement playerMovement;
    private GameObject trashCan;
    
    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
        playerMovement = Player.GetComponent<PlayerMovement>();
        holdSpot = Player.transform.Find("boxHolder");
    }

    void DropItem()
    {
        Debug.Log("Dropping item.");
        itemHolding.transform.position = transform.position + Direction;
        itemHolding.transform.parent = null;
        Rigidbody2D rb = itemHolding.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.bodyType = RigidbodyType2D.Static;
        }
        itemHolding = null;
        //Destroy(rb);
    }
    void HandleCollisionWithNpc()
    {
        Debug.Log("Handling collision with NPC.");
        Collider2D npcCollider = Physics2D.OverlapCircle(transform.position + Direction, 1f, npcMask);
        if (npcCollider != null && npcCollider.GetComponent<TrashCan>().fullness < 100f)
        {
            GiveItemToNpc(npcCollider);
        }
    }

    void GiveItemToNpc(Collider2D npc)
    {
        //toxicBar.AddDrink(10);
        Debug.Log("Giving item to Trash.");
        //Destroy(itemHolding);
        
        itemHolding.transform.position = new Vector3(1000, 1000, 0);
        Destroy(itemHolding);
        //itemHolding.SetActive(false);
        itemHolding = null;

        npc.GetComponent<TrashCan>().addFullness(10);
    }
}