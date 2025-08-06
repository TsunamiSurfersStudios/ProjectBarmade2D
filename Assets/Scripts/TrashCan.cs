using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    public float fullness;
    [SerializeField] private Animator animator;
    private GameObject trashCan;
    private GameObject trashBag;
    private bool touchingTrashCan;
    
    void Start()
    {
        trashBag = GameObject.FindWithTag("TrashBag");
        trashCan = GameObject.FindWithTag("TrashCan");
    }

    
    void Update()
    {
        if (fullness <= 100f && fullness > 50f)
        {
            animator.SetBool("isFull", true);
        }
        else
        {
            animator.SetBool("isFull", false);
        }

        if (Input.GetKeyDown(KeyCode.E) && touchingTrashCan && fullness == 100f)
        {
            addTrashBag();
        }
    }

    public void addFullness(int num){
        fullness += num;
    }

    public void addTrashBag()
    {
        Vector2 spawnPosition = new Vector2(transform.position.x - 0.5f, transform.position.y - 1f);
        fullness = 0f;
        Instantiate(trashBag, spawnPosition, Quaternion.identity);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            touchingTrashCan = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            touchingTrashCan = false;
        }
    }
}
//future me: make boolean for if there is collision, when collision is true and key is pressed trash bag is generated :D