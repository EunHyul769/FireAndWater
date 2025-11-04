using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HiddenMapResult : MonoBehaviour
{
    [Header("결과 표시 UI")]
    public GameObject FinishUI;          // 결과창 (Canvas 안)
    public TextMeshProUGUI WinnerText;   // 승자 텍스트
    public TextMeshProUGUI TimeText;     // 타이머 표시용 텍스트 (추가)

    private bool gameEnded = false;

    private void Start()
    {
        if (FinishUI != null)
            FinishUI.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 이미 종료된 게임이면 무시
        if (gameEnded) return;

        // Fire 또는 Water 플레이어만 반응
        if (collision.CompareTag("Player_Fire") || collision.CompareTag("Player_Water"))
        {
            gameEnded = true;

            // 승자 판정
            string winner = collision.CompareTag("Player_Fire") ? "Fire" : "Water";

            // 타이머 중지
            if (TimeManager.Instance != null)
                TimeManager.Instance.StopTimer();

            // 결과 출력
            ShowResult(winner);

            // 이동 정지 처리 (둘 다 멈추게)
            StopAllPlayers();
        }
    }

    private void ShowResult(string winner)
    {
        if (FinishUI != null)
            FinishUI.SetActive(true);

        // 승자 텍스트
        if (WinnerText != null)
        {
            WinnerText.text = $"{winner} Winner!";
            WinnerText.color = (winner == "Fire") ? Color.red : Color.cyan;
        }

        // 타이머 표시
        if (TimeText != null && TimeManager.Instance != null)
        {
            float elapsedTime = TimeManager.Instance.GetElapsedTime();

            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            int milliseconds = Mathf.FloorToInt((elapsedTime * 100) % 100);

            TimeText.text = $"Time: {minutes:00}:{seconds:00}.{milliseconds:00}";
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