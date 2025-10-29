using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public string playerType = "Fire"; // "Fire" or "Water"
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public KeyCode leftKey;
    public KeyCode rightKey;
    public KeyCode jumpKey;
    public KeyCode interactKey = KeyCode.E;

    private Rigidbody2D rb;
    private bool isGrounded = false;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
        Jump();
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
        // 불 캐릭터 - 물에 닿으면 죽음
        if (playerType == "Fire" && collision.CompareTag("Water"))
        {
            Die();
        }

        // 물 캐릭터 - 불에 닿으면 죽음
        if (playerType == "Water" && collision.CompareTag("Fire"))
        {
            Die();
        }

        // 버튼
        if (collision.CompareTag("Button"))
        {
            collision.GetComponent<ButtonTrigger>()?.Press();
        }

        // 레버 (근처에서만 활성화 → E키)
        if (collision.CompareTag("Lever"))
        {
            Lever lever = collision.GetComponent<Lever>();
            if (lever != null)
                lever.SetPlayerInRange(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Button"))
        {
            collision.GetComponent<ButtonTrigger>()?.Release();
        }

        if (collision.CompareTag("Lever"))
        {
            collision.GetComponent<Lever>()?.SetPlayerInRange(null);
        }
    }

    private void Die()
    {
        Debug.Log($"{playerType} Player died!");
        gameObject.SetActive(false);
    }
}
