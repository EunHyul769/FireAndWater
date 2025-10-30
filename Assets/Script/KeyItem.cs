using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : MonoBehaviour
{
    public string keyType = "Fire"; // "Fire" or "Water"
    private Rigidbody2D rb;
    private Collider2D col;
    private PlayerController owner;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    public void PickUp(PlayerController player)
    {
        owner = player;
        rb.isKinematic = true;
        col.enabled = false;
        transform.SetParent(player.keyHoldPosition);
    }

    public void Drop()
    {
        transform.SetParent(null);
        rb.isKinematic = false;
        col.enabled = true;
        owner = null;
    }

    public void TransferTo(PlayerController newOwner)
    {
        transform.SetParent(null);
        owner = newOwner;
        newOwner.heldKey = this;
        transform.position = newOwner.keyHoldPosition.position;
    }
}
