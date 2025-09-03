using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    // Movement variables
    bool moveHorizontally, moveVertically;
    [SerializeField] private float movementSpeed = 0.01f;
    GameObject[] chairs;
    GameObject leavePoint;
    Vector2 destination, position;

    private GameObject seat;

    [SerializeField] private Animator animator;
    private SpriteRenderer spriteRenderer;

    // Interaction variables
    [SerializeField] private float sobering = 0.01f;
    [SerializeField] private float NPCTolerance = 0f;
    [SerializeField] private float soberSeconds = 50f; // Time in seconds to sober up
    private float soberTimer = 0f;
    [SerializeField] private float currentDrunkness = 0f;
    [SerializeField] private float maxDrunk = 100f;
    private GameObject drunkMeter;
    private ToxicBar toxicBar;
    private NPCDialogue dialogue;
    private NPCOrdering ordering;


    // Start is called before the first frame update
    void Start()
    {
        leavePoint = GameObject.Find("LeavePoint");
        spriteRenderer = GetComponent<SpriteRenderer>();
        drunkMeter = gameObject.transform.Find("DrunkMeter").gameObject;
        toxicBar = drunkMeter.transform.Find("ToxicBar").GetComponent<ToxicBar>();
        dialogue = gameObject.GetComponent<NPCDialogue>();
        ordering = GetComponent<NPCOrdering>(); 
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
                position.x -= movementSpeed;
                spriteRenderer.flipX = false;
                animator.SetBool("isLeft", true);
                animator.SetBool("isRight", false);
            }
            else
            {
                position.x += movementSpeed;
                spriteRenderer.flipX = true;
                animator.SetBool("isRight", true);
                animator.SetBool("isLeft", false);
            }
        }
        else if (moveVertically)
        {
            animator.SetBool("isLeft", false);
            animator.SetBool("isRight", false);
            if (position.y > destination.y)
            {
                position.y -= movementSpeed;
                animator.SetBool("isDown", true);
            }
            else
            {
                position.y += movementSpeed;
                animator.SetBool("isUp", true);
            }
        }

        if (Mathf.Round(position.x) == Mathf.Round(destination.x) && Mathf.Round(destination.y) == Mathf.Round(position.y))
        {
            animator.SetBool("isLeft", false);
            animator.SetBool("isRight", false);
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
            Debug.Log(ordering.GetRecipeAccuracy(ordering.GetOrder(), holder.GetComponentInChildren<DrinkController>()));
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

    public void SetDrunkMeter(GameObject meter)//Pretty sure this is an unused function, can be removed ? 
    {
        drunkMeter = meter;
        meter.transform.parent = gameObject.transform; 
        toxicBar = drunkMeter.GetComponent<ToxicBar>();
    }
}
