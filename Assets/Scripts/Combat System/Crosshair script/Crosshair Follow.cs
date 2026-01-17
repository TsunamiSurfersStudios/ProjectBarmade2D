using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairFollow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false; // Hide the cursor
    }

    // Update is called once per frame
    void Update()
    {
        // Get the mouse position in screen space
        Vector2 mousePosition = Input.mousePosition;
        // Set the crosshair position to the mouse position
        transform.position = mousePosition;
    }
}
