using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events; 

public class InteractionController : MonoBehaviour
{
    [SerializeField] string TagToCheck = "Player";
    [SerializeField] UnityEvent OnBoxCollide;
    [SerializeField] UnityEvent OnBoxExit;
    [SerializeField] UnityEvent OnItemInteraction;
    [SerializeField] UnityEvent OnItemDisable;

    [SerializeField] KeyCode KEYBIND = KeyCode.E;

    private bool isColliding = false;
    private bool isInteracting = false;

    // Check for player collision
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(TagToCheck))
        {
            isColliding = true;
            OnBoxCollide.Invoke();
        }
    }

    // Check for end of player collision
    void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag(TagToCheck))
        {
            isColliding = false;
            OnBoxExit.Invoke();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (!gameObject.GetComponent("BoxCollider2D"))
        {
            Debug.Log(gameObject.name + " does not have a box collider. InteractionController will not work as intended.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isColliding && Input.GetKeyDown(KEYBIND))
        {
            Interact();
        }
    }

    public void Interact()
    {
        isInteracting = !isInteracting;
        if (isInteracting)
        {
            OnItemInteraction.Invoke();
        }
        else
        {
            OnItemDisable.Invoke();
        }
    }
}