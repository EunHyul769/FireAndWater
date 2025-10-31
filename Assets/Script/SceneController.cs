using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void OnStageClear()
    {
        TimeManager.Instance.StopTimer();
        float clearTime = TimeManager.Instance.GetElapsedTime();

        // MainUI에서 StageClearUIManager 찾기
        StageClearUIManager clearUI = FindObjectOfType<StageClearUIManager>();
        if (clearUI != null)
            clearUI.ShowStageClearUI(clearTime);
    }

    public void OnRetryStage()
    {
        GameManager.Instance.RetryStage();
    }

    public void OnNextStage(string nextStageName)
    {
        GameManager.Instance.LoadNextStage(nextStageName);
    }
}
