using System.Diagnostics;
using UnityEngine;

[System.Serializable]
public class Achievement
{
    public string id;
    public string title;
    public string description;
    public int goal;       // 목표값 (예: 100번)
    public int progress;   // 현재 진행값
    public bool isUnlocked;

    // 진행도를 업데이트하는 함수
    public bool AddProgress(int amount)
    {
        if (isUnlocked) return false;

        progress += amount;

        if (progress >= goal)
        {
            progress = goal;
            isUnlocked = true;
            return true; // 새로 해금됨
        }

        return false; // 아직 미달성
    }

    public bool AdjustProgress(int amount)
    {
        if (isUnlocked) return false;

        progress = amount;

        if (progress >= goal)
        {
            progress = goal;
            isUnlocked = true;
            UnityEngine.Debug.Log("도전과제 달성(adjust)");
            return true; // 새로 해금됨
        }

        return false; // 아직 미달성
    }
}

