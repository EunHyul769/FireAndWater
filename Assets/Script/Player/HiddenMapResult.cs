using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiddenMapResult : MonoBehaviour
{
    [Header("결과 표시 UI")]
    public GameObject resultPanel;      // 결과창 (Canvas 안에)
    public Text resultText;             // 승자 텍스트

    private bool gameEnded = false;     // 이미 끝났는지 여부

    TimeManager timeManager;

    private void Start()
    {
        if (resultPanel != null)
            resultPanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 이미 종료된 게임이면 무시
        if (gameEnded) return;

        timeManager.StopTimer();

        // Fire 또는 Water 플레이어만 반응
        if (collision.CompareTag("Player_Fire") || collision.CompareTag("Player_Water"))
        {
            gameEnded = true;

            // 승자 판정
            string winner = collision.CompareTag("Player_Fire") ? "Fire" : "Water";

            // 결과 출력
            ShowResult(winner);

            // 이동 정지 처리 (둘 다 멈추게)
            StopAllPlayers();
        }
    }

    private void ShowResult(string winner)
    {
        if (resultPanel != null && resultText != null)
        {
            resultPanel.SetActive(true);
            resultText.text = $"{winner} Winner!";
            resultText.color = (winner == "Fire") ? Color.red : Color.cyan;
        }

        Debug.Log($"{winner} Winner!");
    }

    private void StopAllPlayers()
    {
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        foreach (var p in players)
        {
            Rigidbody2D rb = p.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.velocity = Vector2.zero;

            // 움직임 막기
            p.enabled = false;
        }
    }
}
