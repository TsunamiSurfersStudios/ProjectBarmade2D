using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ItemHolder : MonoBehaviour
{
    private static ItemHolder instance;
    public static ItemHolder Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ItemHolder>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("ItemHolder");
                    instance = singletonObject.AddComponent<ItemHolder>();
                }
            }
            return instance;
        }
    }

    private GameObject heldObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void GiveObject(GameObject obj)
    {
        if (heldObject == null && obj != null)
        {
            heldObject = obj;
            obj.transform.position = transform.position;
            obj.transform.parent = transform;

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
        if (heldObject == null)
        {
            return;
        }

        heldObject.transform.parent = null;

        Rigidbody2D rb = heldObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.bodyType = RigidbodyType2D.Dynamic;
        }

        heldObject = null;
    }

    public GameObject TakeObject()
    {
        if (heldObject == null)
        {
            return null;
        }

        GameObject obj = heldObject;
        heldObject = null;

        obj.transform.parent = null;

        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.bodyType = RigidbodyType2D.Dynamic;
        }

        return obj;
    }

    public void DestroyObject()
    {
        if (heldObject != null)
        {
            Destroy(heldObject);
            heldObject = null;
        }
    }

    public bool IsEmpty()
    {
        return heldObject == null;
    }

    public GameObject GetHeldObject()
    {
        return heldObject;
    }

    public void Spawn(GameObject itemPrefab)
    {
        if (heldObject != null || itemPrefab == null)
        {
            return;
        }

        GameObject clone = Instantiate(itemPrefab);
        clone.SetActive(true);
        GiveObject(clone);
    }

    public void SwitchPosition(bool isForward = true)
    {
        if (heldObject != null)
        {
            SpriteRenderer renderer = heldObject.GetComponent<SpriteRenderer>();
            renderer.sortingLayerName = isForward ? "ObjectInFront" : "Object";
        }
    }
}