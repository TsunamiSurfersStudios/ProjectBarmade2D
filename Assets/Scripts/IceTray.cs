using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTray : MonoBehaviour
{
    public float iceTrayVolume = 0f;
    public float iceMachineVolume = 100f;
    [SerializeField] private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (iceTrayVolume <= 100f && iceTrayVolume > 50f)
        {
            animator.SetBool("isHalfEmpty", false);
            animator.SetBool("isFull", true);
        }
        else if (iceTrayVolume <= 50f && iceTrayVolume > 0f)
        {
            animator.SetBool("isFull", false);
            animator.SetBool("isHalfEmpty", true);
        }
        else
        {
            animator.SetBool("isHalfEmpty", false);
            animator.SetBool("isFull", false);
        }

        if (Input.GetKeyDown(KeyCode.E)){ //temp way to increase value
            if (iceTrayVolume < 100f)
            {
                iceTrayVolume += 1f;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q)){ //temp way to decrease value
            if (iceTrayVolume > 0f)
            {
                iceTrayVolume -= 1f;
            }
        }    

    }
}
