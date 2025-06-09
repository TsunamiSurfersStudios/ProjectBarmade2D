using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class DrinkController : MonoBehaviour
{
    public float percentage;
    public GameObject drink;

    private Transform holdSpot;
    private LayerMask pickUpMask;
    private LayerMask npcMask;
    public Vector3 Direction { get; set; }
    private GameObject itemHolding;
    public bool touchingDrink = false;
    private GameObject Player;
    private PlayerMovement playerMovement;

    void Start()
    {
        Debug.Log("Start was hit!");
        Player = GameObject.FindWithTag("Player");
        playerMovement = Player.GetComponent<PlayerMovement>();
        holdSpot = Player.transform.Find("boxHolder");

        if (percentage > 1)
        {
            Debug.Log(name + " alcohol percentage exceeds 100%. Scripts may not work as intended.");
        }
    }

    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (itemHolding)
            {
                Collider2D NPC = Physics2D.OverlapCircle(transform.position + Direction, 1f, npcMask);
                if (NPC)
                {
                    HandleCollisionWithNpc();
                }
                else if (playerMovement.collidingWithDishwasher == false)
                {
                    //needs to be commented out for the glass to be a child of the dishwasher gameobject.
                    DropItem();
                }

            }
            else if (touchingDrink)
            {

                Debug.Log("Picked up: " + gameObject.name);
                itemHolding = gameObject;
                itemHolding.transform.position = holdSpot.position;
                itemHolding.transform.parent = holdSpot;
                Rigidbody2D rb = itemHolding.GetComponent<Rigidbody2D>(); // makes it so that it follows player wherever it goes
                if (rb != null)
                {
                    rb.isKinematic = true;
                    rb.velocity = Vector2.zero;
                    rb.angularVelocity = 0f;
                }
            }

        }
        */
    }

    public void SpawnDrink()
    {
        GameObject clone = GameObject.Instantiate(drink);
        clone.transform.position = holdSpot.position;
        clone.transform.parent = holdSpot;
        Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
        clone.SetActive(true);
        DestroySelf destroySelf = clone.GetComponent<DestroySelf>();
        destroySelf.isClone = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("touching drink");
            touchingDrink = true;
        }
        else if (collision.gameObject.CompareTag("Sink"))
        {

        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            touchingDrink = false;
        }
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
        if (npcCollider != null)
        {

            GiveItemToNpc(npcCollider);
        }
    }

    void GiveItemToNpc(Collider2D npc) // Needs to evolve to handle individual NPCs and their specific drink needs`
    {
        //toxicBar.AddDrink(10);
        Debug.Log("Giving item to NPC.");
        //Destroy(itemHolding);

        itemHolding.transform.position = new Vector3(1000, 1000, 0);
        Destroy(itemHolding);
        //itemHolding.SetActive(false);
        itemHolding = null;

        npc.GetComponent<NPCIteract>().AddDrink(10);
    }
}
