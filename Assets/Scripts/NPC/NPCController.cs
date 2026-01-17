using Codice.CM.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    // Movement variables
    bool moveHorizontally, moveVertically;
    [SerializeField] private float movementSpeed = 0.01f;
    GameObject leavePoint;
    Vector2 destination, position;

    private GameObject seat;

    [SerializeField] private Animator animator;
    private SpriteRenderer spriteRenderer;
    private NavMeshAgent navAgent;

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

    private const string HORIZONTAL = "HorizontalVal";
    private const string VERTICAL = "VerticalVal";
    private const string SPEED = "Speed";

    // Start is called before the first frame update
    void Start()
    {
        leavePoint = GameObject.Find("LeavePoint");
        spriteRenderer = GetComponent<SpriteRenderer>();
        drunkMeter = gameObject.transform.Find("DrunkMeter").gameObject;
        toxicBar = drunkMeter.transform.Find("ToxicBar").GetComponent<ToxicBar>();
        dialogue = gameObject.GetComponent<NPCDialogue>();
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.updateRotation = false;
        navAgent.updateUpAxis = false;
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
        navAgent.SetDestination(destination);
        Vector3 velocity = navAgent.velocity;

        if (velocity.magnitude > 0)
        {
            Vector3 direction = velocity.normalized;
            animator.SetFloat(SPEED, 1f);

            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
            {
                animator.SetFloat(HORIZONTAL, direction.x);
                animator.SetFloat(VERTICAL, 0);
                spriteRenderer.flipX = direction.x > 0;
            }
            else
            {
                animator.SetFloat(HORIZONTAL, 0);
                animator.SetFloat(VERTICAL, direction.z);
            }
        }
        else
        {
            animator.SetFloat(HORIZONTAL, 0);
            animator.SetFloat(VERTICAL, 0);
            animator.SetFloat(SPEED, 0f);
        }
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
            float initalIntoxication = UnityEngine.Random.Range(5, alcoholPercentage);
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
