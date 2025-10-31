using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : MonoBehaviour
{
    public string keyType = "Fire"; // "Fire" or "Water"
    public GameObject keyPrefab;    // 재생성용 프리팹

    private Rigidbody2D rb;
    private Collider2D[] colliders; // 여러 Collider 제어용
    private PlayerController owner;

    private bool isLocked = false;
    private bool isPickedUp = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        colliders = GetComponents<Collider2D>(); // 모든 콜라이더 가져오기
    }

    public void PickUp(PlayerController player)
    {
        if (isLocked || isPickedUp || owner != null)
            return;

        isLocked = true;
        StartCoroutine(UnlockAfterDelay(0.05f));

        isPickedUp = true;
        owner = player;

        // 모든 Collider 비활성화 (머리 위에서는 감지 안 되게)
        foreach (Collider2D c in colliders)
            c.enabled = false;

        // 물리 정지
        rb.isKinematic = true;
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;

        // 플레이어 머리 위로 이동
        transform.SetParent(player.keyHoldPosition);
        transform.localPosition = Vector3.zero;

        Debug.Log($"{player.playerType} picked up {keyType} key!");
    }

    private IEnumerator UnlockAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);
        isLocked = false;
    }

    public void Drop()
    {
        if (owner == null) return;

        // 방향 계산
        float dropOffset = 1.2f;
        Vector3 dropPos = owner.transform.position;
        float dir = owner.transform.rotation.y == 0 ? 1f : -1f;
        dropPos += new Vector3(dir * dropOffset, 0.5f, 0);

        // 새 열쇠 생성
        if (keyPrefab != null)
        {
            GameObject newKey = Instantiate(keyPrefab, dropPos, Quaternion.identity);

            Rigidbody2D newRb = newKey.GetComponent<Rigidbody2D>();
            if (newRb != null)
            {
                newRb.bodyType = RigidbodyType2D.Dynamic;
                newRb.gravityScale = 1f;
                newRb.AddForce(Vector2.down * 1.5f, ForceMode2D.Impulse);
            }

            // 새 열쇠의 모든 콜라이더 다시 활성화
            Collider2D[] newCols = newKey.GetComponents<Collider2D>();
            foreach (Collider2D c in newCols)
                c.enabled = true;

            KeyItem newKeyItem = newKey.GetComponent<KeyItem>();
            newKeyItem.keyType = keyType;

            Debug.Log($"Dropped new {keyType} key at {dropPos}");
        }

        Destroy(gameObject);

        owner.heldKey = null;
        owner = null;
    }

    public void TransferTo(PlayerController newOwner)
    {
        if (owner == newOwner) return;

        transform.SetParent(null);
        owner = newOwner;
        newOwner.heldKey = this;
        transform.position = newOwner.keyHoldPosition.position;
    }
}