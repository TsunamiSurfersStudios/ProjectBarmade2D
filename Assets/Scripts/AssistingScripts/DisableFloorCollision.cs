using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableFloorCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform) { // TODO: If this is specifically to be used on floor object, variable names (child) should reflect that
            BoxCollider2D collider = child.gameObject.GetComponent<BoxCollider2D>();
            if(collider != null) {
                collider.enabled = false;
            }
        }
    }
}
