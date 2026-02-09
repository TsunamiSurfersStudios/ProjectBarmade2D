using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AudioSource))]
[RequireComponent (typeof(SpriteRenderer))]
[RequireComponent(typeof(NPCDialogue))]
[RequireComponent(typeof(NavMeshAgent))]
public class NPCController : MonoBehaviour
{
    // Movement variables
    GameObject leavePoint;
    Vector2 destination;

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

    [SerializeField] AudioClip rejectionClip; //TODO: Implement this through Sammy's audio system 

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

        MoveNPC();
        HandleIntoxication();   
    }
    
    private void MoveNPC()
    {
        navAgent.SetDestination(destination);
        Vector3 velocity = navAgent.velocity;

        if (velocity.magnitude > 0)
        {
            Vector3 direction = velocity.normalized;
            animator.SetFloat(SPEED, 1f);

            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                animator.SetFloat(HORIZONTAL, direction.x);
                animator.SetFloat(VERTICAL, 0);
                spriteRenderer.flipX = direction.x > 0;
            }
            else
            {
                animator.SetFloat(HORIZONTAL, 0);
                animator.SetFloat(VERTICAL, direction.y);
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
        if (navAgent.velocity.magnitude > 0) // Do not sober up while moving
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
    }

    public void Leave()
    {
        if (seat != null)
        {
            NPCObjects seatController = seat.GetComponent<NPCObjects>();
            if (seatController != null)
                seatController.SetOccupied(false);
            seat = leavePoint;
            destination = seat.transform.position;
        }
    }

    public void Interact()
    {
        ItemHolder holder = ItemHolder.Instance;
        NPCOrdering ordering = GetComponent<NPCOrdering>();
        GameEventManager.Instance.TriggerEvent(GameEventManager.GameEvent.CustomerInteracted);
        if (holder.IsEmpty())
        {
            dialogue.StartConversation();
        }
        else if (ordering != null && ordering.OrderActive())
        {
            GiveDrink(holder.TakeObject());
        }
        else
        {
            // Reject drink
            AudioSource audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
            audioSource.PlayOneShot(rejectionClip, 1.0f); // TODO: Do this through audio system
        }
    }

    public void GiveDrink(GameObject drink)
    {
        DrinkController drinkController = drink.GetComponent<DrinkController>();
        if (drinkController)
        {
            NPCOrdering ordering = GetComponent<NPCOrdering>();
            if (ordering != null && ordering.OrderActive())
            {
                ordering.CompleteOrder();

                //Pay up the tab after finishing all drinks
                if (ordering.HasFinishedAllDrinks())
                {
                    PlayerStats playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
                    if (playerStats != null)
                    {
                        playerStats.AddMoney(ordering.GetTab());
                    }
                }
            }

            float alcoholPercentage = drinkController.GetAlcoholPercentage();
            float initalIntoxication = UnityEngine.Random.Range(5, alcoholPercentage);
            float reducedIntoxication = initalIntoxication * NPCTolerance; 
            float finalIntoxication = initalIntoxication - reducedIntoxication;

            currentDrunkness = Mathf.Clamp(currentDrunkness + finalIntoxication, 0, maxDrunk);
            toxicBar.SetDrunkness(currentDrunkness);
            GameEventManager.Instance.TriggerEvent(GameEventManager.GameEvent.CustomerServed);
            Destroy(drink);

            //Leave the bar when done drinking
            if (ordering != null && ordering.HasFinishedAllDrinks())
            {
                Leave();
            }
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
