using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    public KeyCode KEYCODE = KeyCode.E;
    public bool isClone = false;

    // Update is called once per frame
    void Update()
    {
        GameObject Player = GameObject.FindWithTag("Player");
        PlayerMovement playerMovement = Player.GetComponent<PlayerMovement>();
        if (playerMovement.touchingSink && Input.GetKeyDown(KEYCODE))
        {
            Destroy(gameObject);
        }
    }
}
