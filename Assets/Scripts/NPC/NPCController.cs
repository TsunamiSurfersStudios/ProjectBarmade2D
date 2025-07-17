using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    // Movement variables
    bool moveHorizontally, moveVertically;
    GameObject[] chairs;
    GameObject leavePoint;
    Vector2 destination, position;

    private GameObject seat;

    [SerializeField] private Animator animator;
    private SpriteRenderer spriteRenderer;

    // Interaction variables
    private float sobering = 1f;
    private float NPCTolerance = 0f;
    private float soberSeconds = 10f; // Time in seconds to sober up
    private float soberTimer = 0f;
    private float currentDrunkness = 0;
    private float maxDrunk = 100;
    private GameObject drunkMeter;
    private ToxicBar toxicBar;
    private NPCDialogue dialogue;


    // Start is called before the first frame update
    void Start()
    {
        leavePoint = GameObject.Find("LeavePoint"); 
        spriteRenderer = GetComponent<SpriteRenderer>();
        drunkMeter = gameObject.transform.Find("DrunkMeter").gameObject; 
        toxicBar = drunkMeter.transform.Find("ToxicBar").GetComponent<ToxicBar>();
        dialogue = gameObject.GetComponent<NPCDialogue>(); 
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "LeavePoint")
        {
            Destroy(gameObject);
        }
    }

    void Update() 
    {
        if (!seat) { return; }

        DetermineDirection();
        MoveNPC();
        HandleIntoxication();   
    }
    
    private void DetermineDirection()
    {
        if (!moveVertically && Mathf.Round(position.x) != Mathf.Round(destination.x))
        {
            moveHorizontally = true;
            moveVertically = false;
        }
        else if (!moveHorizontally && Mathf.Round(position.y) != Mathf.Round(destination.y))
        {
            moveVertically = true;
            moveHorizontally = false;
        }

        if (Mathf.Round(position.x) == Mathf.Round(destination.x)) moveHorizontally = false;
        if (Mathf.Round(position.y) == Mathf.Round(destination.y)) moveVertically = false;
    }
    private void MoveNPC()
    {
        if (moveHorizontally)
        {
            animator.SetBool("isDown", false);
            animator.SetBool("isUp", false);
            if (position.x > destination.x)
            {
                position.x = position.x - 0.01f;
                spriteRenderer.flipX = false;
                animator.SetBool("isHorizontal", true);
            }
            else
            {
                position.x = position.x + 0.01f;
                spriteRenderer.flipX = true;
                animator.SetBool("isHorizontal", true);
            }
        }
        else if (moveVertically)
        {
            animator.SetBool("isHorizontal", false);
            if (position.y > destination.y)
            {
                position.y = position.y - 0.01f;
                animator.SetBool("isDown", true);
            }
            else
            {
                position.y = position.y + 0.01f;
                animator.SetBool("isUp", true);
            }
        }

        if (Mathf.Round(position.x) == Mathf.Round(destination.x) && Mathf.Round(destination.y) == Mathf.Round(position.y))
        {
            animator.SetBool("isHorizontal", false);
            animator.SetBool("isDown", false);
            animator.SetBool("isUp", false);
        }

        transform.position = position;
    }
    private void HandleIntoxication()
    {
        if (!moveVertically && !moveVertically) // Do not sober up while moving
        {
            drunkMeter.SetActive(true);
            if (currentDrunkness > 0 && toxicBar)
            {
                if (soberTimer >= soberSeconds)
                {
                    currentDrunkness -= sobering;
                    soberTimer = 0f;
                }
                else
                {
                    soberTimer += Time.deltaTime;
                }
                toxicBar.SetDrunkness(currentDrunkness);
            }
        }
    }
    public void SetSeat(GameObject seat)
    {
        this.seat = seat;
        destination = seat.transform.position;
        position = transform.position; 
    }

    public void Leave()
    {
        if (seat != null)
        {
            seat.GetComponent<NPCObjects>().SetOccupied(false);
            seat = leavePoint;
            destination = seat.transform.position;
        }
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButton(0)) {
            Leave();
        }
    }

    public void Interact()
    {
        ItemHolder holder = GameObject.FindWithTag("Player").GetComponentInChildren<ItemHolder>();
        if (holder.IsEmpty())
        {
            dialogue.StartConversation();
        }
        else
        {
            GiveDrink(holder.TakeObject());
        }
    }

    public void GiveDrink(GameObject drink)
    {
        DrinkController drinkController = drink.GetComponent<DrinkController>();
        if (drinkController)
        {
            float alcoholPercentage = drinkController.GetAlcoholPercentage();
            float initalIntoxication = Random.Range(5, alcoholPercentage);
            float reducedIntoxication = initalIntoxication * NPCTolerance; 
            float finalIntoxication = initalIntoxication - reducedIntoxication;

            currentDrunkness = Mathf.Clamp(currentDrunkness + finalIntoxication, 0, maxDrunk);
            toxicBar.SetDrunkness(currentDrunkness);
            Destroy(drink);
        }
        else
        {
            ItemHolder holder = GameObject.FindWithTag("Player").GetComponentInChildren<ItemHolder>();
            holder.GiveObject(drink);
        }
    }

    public void SetDrunkMeter(GameObject meter)
    {
        drunkMeter = meter;
        meter.transform.parent = gameObject.transform; 
        toxicBar = drunkMeter.GetComponent<ToxicBar>();
    }
}
