using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance; // 싱글톤
    public TextMeshProUGUI timerText;

    private float elapsedTime = 0f;
    private bool isRunning = false;

    void Awake()
    {
        // 스테이지 전환 시마다 새로 생성되므로 굳이 DontDestroy 필요 없음
        Instance = this;
    }

    void Start()
    {
        StartTimer(); // 스테이지 시작 시 자동 실행
    }

    void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;  //시간 진행따라 알아서 시간 증가
        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        int milliseconds = Mathf.FloorToInt((elapsedTime * 100) % 100);

        timerText.text = $"{minutes:00}:{seconds:00}.{milliseconds:00}";
    }

    public void StartTimer()
    {
        elapsedTime = 0f;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }
}
