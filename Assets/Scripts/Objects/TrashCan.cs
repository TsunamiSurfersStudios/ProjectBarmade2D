using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    public float fullness;
    [SerializeField] private Animator animator;
    
    void Start()
    {
        
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
    }

    public void addFullness(int num){
        fullness += num;
        Debug.Log("Trash Filled");
    }
}
