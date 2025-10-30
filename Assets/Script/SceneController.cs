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
        PlayerPrefs.SetFloat("ClearTime", clearTime);

        // 스테이지 클리어 씬으로 이동
        SceneManager.LoadScene("StageClearScene");
    }

    public void OnRetryStage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // 현재 씬 다시 로드한 후 타이머 자동 리셋
    }

    public void OnNextStage(string nextStageName)
    {
        SceneManager.LoadScene(nextStageName);
        // 다음 스테이지 로드한 후 타이머 자동 리셋
    }
}
