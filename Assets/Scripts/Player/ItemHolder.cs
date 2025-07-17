using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    private GameObject heldObject;

    public void GiveObject(GameObject obj)
    {
        if (heldObject == null && obj != null)
        {
            heldObject = obj;

            obj.transform.position = gameObject.transform.position;
            obj.transform.parent = gameObject.transform;

            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }
    }

    public void DropItem()
    {
        heldObject.transform.position = transform.position;
        heldObject.transform.parent = null;
        Rigidbody2D rb = heldObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.bodyType = RigidbodyType2D.Static;
        }
        heldObject = null;
    }

    public GameObject TakeObject()
    {
        GameObject obj = heldObject;
        heldObject = null;
        return obj;
    }

    public void DestroyObject()
    {
        Destroy(heldObject); 
        heldObject = null;
    }

    public bool IsEmpty()
    {
        return heldObject == null;
    }
}
