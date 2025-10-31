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
    public KeyCode leftKey;
    public KeyCode rightKey;
    public KeyCode downKey;
    public KeyCode jumpKey;
    public KeyCode interactKey = KeyCode.E;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool facingRight = true;

    // Key system
    [HideInInspector] public KeyItem heldKey = null;
    public Transform keyHoldPosition;
    private PlayerController otherPlayer;

    // Animation Handler
    private AnimationHandler animationHandler;

    // 레버 근처 감지용
    private LeverObject nearbyLever;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animationHandler = GetComponentInChildren<AnimationHandler>();
    }

    private void Start()
    {
        // 다른 플레이어 탐색
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

        // 레버와 상호작용
        if (nearbyLever != null && Input.GetKeyDown(interactKey))
        {
            nearbyLever.SetPlayerInRange(this);
            
        }

        // 키 관련 기능
        if (heldKey != null)
        {
            heldKey.transform.position = keyHoldPosition.position;

            if (Input.GetKeyDown(downKey))
                DropKey();

            if (Input.GetKeyDown(interactKey))
                TryGiveKey();
        }
    }

    private void Move()
    {
        float moveDir = 0;
        if (Input.GetKey(leftKey)) moveDir = -1;
        if (Input.GetKey(rightKey)) moveDir = 1;

        rb.velocity = new Vector2(moveDir * moveSpeed, rb.velocity.y);

        if (moveDir > 0 && !facingRight)
            Flip(true);
        else if (moveDir < 0 && facingRight)
            Flip(false);

        animationHandler?.Move(rb.velocity);
    }

    private void Flip(bool faceRight)
    {
        facingRight = faceRight;
        transform.rotation = faceRight ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
    }

    private void Jump()
    {
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        if (isGrounded && Input.GetKeyDown(jumpKey))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animationHandler?.Jump(rb.velocity);
        }

        if (!wasGrounded && isGrounded)
            animationHandler?.JumpEnd();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 불 캐릭터 - 물에 닿으면 죽음
        if (playerType == "Fire" && collision.CompareTag("Water"))
            Die();

        // 물 캐릭터 - 불에 닿으면 죽음
        if (playerType == "Water" && collision.CompareTag("Fire"))
            Die();

        // 키 획득
        if (collision.CompareTag("Key_Fire") || collision.CompareTag("Key_Water"))
        {
            KeyItem key = collision.GetComponent<KeyItem>();
            if (key != null && heldKey == null)
                PickUpKey(key);
        }

        // LeverObject 감지
        if (collision.CompareTag("Lever"))
        {
            LeverObject lever = collision.GetComponent<LeverObject>();
            if (lever != null)
            {
                nearbyLever = lever;
                lever.SetPlayerInRange(this);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 레버 범위를 벗어나면 연결 해제
        if (collision.CompareTag("Lever"))
        {
            LeverObject lever = collision.GetComponent<LeverObject>();
            if (lever != null)
            {
                lever.SetPlayerInRange(null);
                if (nearbyLever == lever)
                    nearbyLever = null;
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
        animationHandler?.Dead(rb.velocity);
        gameObject.SetActive(false);
    }
}