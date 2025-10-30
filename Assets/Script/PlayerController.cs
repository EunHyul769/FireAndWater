using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public string playerType = "Fire"; // "Fire" or "Water"
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode downKey = KeyCode.S;
    public KeyCode jumpKey = KeyCode.W;
    public KeyCode interactKey = KeyCode.E;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded = false;

    // Key system
    [HideInInspector] public KeyItem heldKey = null;
    public Transform keyHoldPosition;  // 플레이어 머리 위에 빈 오브젝트 (열쇠가 따라다님)
    private PlayerController otherPlayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // 씬에서 다른 플레이어 찾기
        foreach (PlayerController p in FindObjectsOfType<PlayerController>())
        {
            if (p != this)
                otherPlayer = p;
        }
    }

    private void Update()
    {
        Move();
        Jump();

        // 키 들고 있을 때
        if (heldKey != null)
        {
            heldKey.transform.position = keyHoldPosition.position;

            // 내려놓기
            if (Input.GetKeyDown(downKey))
            {
                DropKey();
            }

            // 건네주기
            if (Input.GetKeyDown(interactKey))
            {
                TryGiveKey();
            }
        }
    }

    private void Move()
    {
        float moveDir = 0;
        if (Input.GetKey(leftKey)) moveDir = -1;
        if (Input.GetKey(rightKey)) moveDir = 1;

        rb.velocity = new Vector2(moveDir * moveSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        if (isGrounded && Input.GetKeyDown(jumpKey))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 불 플레이어 - 물에 닿으면 죽음
        if (playerType == "Fire" && collision.CompareTag("Water"))
            Die();

        // 물 플레이어 - 불에 닿으면 죽음
        if (playerType == "Water" && collision.CompareTag("Fire"))
            Die();

        // 키 획득 (속성과 상관없이)
        if (collision.CompareTag("Key"))
        {
            KeyItem key = collision.GetComponent<KeyItem>();
            if (key != null && heldKey == null)
            {
                PickUpKey(key);
            }
        }
    }

    private void PickUpKey(KeyItem key)
    {
        heldKey = key;
        key.PickUp(this);
        Debug.Log($"{playerType} player picked up a {key.keyType} key!");
    }

    private void DropKey()
    {
        heldKey.Drop();
        heldKey = null;
        Debug.Log($"{playerType} dropped the key.");
    }

    private void TryGiveKey()
    {
        if (otherPlayer == null || heldKey == null) return;

        float distance = Vector2.Distance(transform.position, otherPlayer.transform.position);
        if (distance < 2f && otherPlayer.heldKey == null)
        {
            heldKey.TransferTo(otherPlayer);
            heldKey = null;
            Debug.Log($"{playerType} gave key to {otherPlayer.playerType}!");
        }
    }

    private void Die()
    {
        Debug.Log($"{playerType} Player died!");
        gameObject.SetActive(false);
    }
}
