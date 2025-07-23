using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dishwasher : MonoBehaviour
{
    public GameObject Itemholder;

    private bool CollidingWithPlayer = false; // TODO: We do not need this
    private int ItemsHeld = 0; // TODO: We do not need this
    private bool washingDishes = false; // TODO: Potential rename: dishwasherRunning or dishwasherActive? 

    // Update is called once per frame
    void Update()
    {
        //if player is colliding with dishwasher and presses E
        if (Input.GetKeyDown(KeyCode.E) && CollidingWithPlayer) // TODO: Use InteractableController instead of testing for collision in here
        {
            ItemsHeld = Itemholder.transform.childCount; // TODO: Turn ItemHolder into a class so we can call itemholder.isEmpty();
            if (ItemsHeld > 0)
            {
                // check if glass is dirty
                if (Itemholder.transform.GetChild(0).GetComponent<EmptyGlass>().IsDirty()) // TODO: This should be a function instead of having publicly exposed variable. 
                {
                    washingDishes = true;
                    Debug.Log("Glass is dirty"); // TODO: Why do we have a Debug.Log at every step. Is this necessary? 
                    //use coroutine to simulate an actual dishwasher similar to threads
                    StartCoroutine(WashDishes()); // TODO: Why are we doing this??? 

                    //clean glass
                    Itemholder.transform.GetChild(0).GetComponent<EmptyGlass>().SetDirty(false); // TODO: This needs a setter
                }
                else
                {
                    Debug.Log("Glass is clean");
                }

                Debug.Log("placing item in dishwasher");
                //transfer glass from player to dishwasher
                Itemholder.transform.GetChild(0).SetParent(gameObject.transform);
            }
            else if(ItemsHeld == 0 && !washingDishes)
            {
                Debug.Log("Player not holding anything");
                if(gameObject.transform.childCount > 0)
                {
                    Debug.Log("picking item from dishwasher");
                    //transfer glass from dishwasher to player
                    gameObject.transform.GetChild(0).SetParent(Itemholder.transform);
                }
            }
                //move glass to the correct position
                //Itemholder.transform.GetChild(0).transform.localPosition = new Vector3(0, 0, 0);

        }
    }

    IEnumerator WashDishes()//similar to threads but this is unity version.
    { //the IEnumerator is important for coroutine.
        //this is a coroutine that simulates washing dishes
        Debug.Log("Washing dishes...");
        yield return new WaitForSeconds(5);//simialr to .Next(5000) which means 5 secs
        Debug.Log("Dishes are clean");
        washingDishes = false;
    } // TODO: Why are we doing it this way? Can it be done better? 

    // TODO: Everything below this should be replaced and substituted with InteractionController
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CollidingWithPlayer = true;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CollidingWithPlayer = false;
        }
    }
}

/* TODO: 
 * Remove functionality from Update function
 * Use InteractionController instead of controlling interactions from dishwasher script
 * Turn ItemHolder into a class so we can call itemholder.isEmpty();
 * dirtyGlass should have a getter/setter instead of being publicly exposed
 * We should create a HoldableObject class that 'Glass' can inherit from that has a function to 'hand object to player'
 * Revisit "coroutine" and examine peak otimized way to acheive this
 */