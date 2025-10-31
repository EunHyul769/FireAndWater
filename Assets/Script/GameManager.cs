using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour

{   
    public static GameManager Instance;
    public string lastStageName;  // 마지막 클리어한 스테이지 이름 저장용

    public SceneController sceneController;
    public TimeManager timeManager;
    public TitleUIManager titleUIManager;
    public StageClearScene stageClearScene;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 넘어가도 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 스테이지 클리어 시 처리
    public void StageClear()
    {
        TimeManager.Instance.StopTimer();
        float clearTime = TimeManager.Instance.GetElapsedTime();
        PlayerPrefs.SetFloat("ClearTime", clearTime);

        lastStageName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("StageClearScene");
    }

    // 다음 스테이지 이동
    public void LoadNextStage(string nextStageName)
    {
        SceneManager.LoadScene(nextStageName);
    }

    // 현재 스테이지 재시작
    public void RetryStage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 타이틀로 복귀
    public void ReturnToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }


    // 게임의 모든 데이터는 GameManager가 소유하도록



}
