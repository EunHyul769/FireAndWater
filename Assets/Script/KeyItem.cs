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
        col.isTrigger = true;
        col.enabled = false;
        transform.SetParent(player.keyHoldPosition);
    }

    public void Drop()
    {
        if (owner == null) return;

        // 플레이어 바라보는 방향 계산
        float dropOffset = 1f;
        Vector3 dropPos = owner.transform.position;

        bool isLeft = owner.GetComponent<SpriteRenderer>()?.flipX ?? false;
        float dir = isLeft ? -1f : 1f;

        dropPos += new Vector3(dir * dropOffset, 0.5f, 0); // 살짝 위

        // 부모 해제 후 위치 재설정
        transform.SetParent(null);
        transform.position = dropPos;

        // 물리 활성화
        rb.isKinematic = false;
        col.isTrigger = false;
        rb.velocity = Vector2.zero;

        rb.WakeUp();

        owner.heldKey = null;
        owner = null;

        // 1초간 다시 줍지 않게
        //StartCoroutine(TemporarilyDisablePickup(1f));
    }

    public void TransferTo(PlayerController newOwner)
    {
        transform.SetParent(null);
        owner = newOwner;
        newOwner.heldKey = this;
        transform.position = newOwner.keyHoldPosition.position;
    }

    IEnumerator TemporarilyDisablePickup(float duration)
    {
        col.isTrigger = true;   // 1초 동안 감지만 막기
        yield return new WaitForSeconds(duration);
        col.isTrigger = false;  // 다시 감지 가능
    }
}
