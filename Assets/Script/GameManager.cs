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
    
    // scene ready_Juwon
    [SerializeField] private Scene[] scenes;
    [SerializeField] private Scene currentScene;
    // stage run/stop
    [SerializeField] private bool isRunning = true;
    [SerializeField] private GoalObject goalFire;
    [SerializeField] private GoalObject goalWater;


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

        // 씬 로드 이벤트 구독
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"씬 로드됨: {scene.name}");

        // MainUI 씬이 로드되어 있지 않다면 추가 로드
        if (!SceneManager.GetSceneByName("MainUI").isLoaded)
        {
            SceneManager.LoadScene("MainUI", LoadSceneMode.Additive);
            Debug.Log("MainUI 씬을 Additive로 로드함");
        }
    }

    // 스테이지 클리어 시 처리
    public void StageClear()
    {
        //타이머 정지, 클탐 계산
        TimeManager.Instance.StopTimer();
        float clearTime = TimeManager.Instance.GetElapsedTime();
        PlayerPrefs.SetFloat("ClearTime", clearTime);

        //현재 스테이지 이름 기록
        lastStageName = SceneManager.GetActiveScene().name;

        //스테이지클리어ui매니저 찾아서 표시
        StageClearUIManager clearUI = FindObjectOfType<StageClearUIManager>();
        if (clearUI != null)
        {
            clearUI.ShowStageClearUI(clearTime);
        }
        else
        {
            Debug.LogWarning("StageClearUIManager를 찾을 수 없습니다. MainUI 씬이 로드되어 있는지 확인하세요.");
        }

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


    // Juwon
    private void startStage(int i)
    {

        if (i < scenes.Length)
        {
            // 씬 메니저 연결 
            //SceneManager.LoadScene(scenes[i].name);
            // 현재 씬 저장
            //currentScene = scenes[i];
        }

        // 현재 스테이지에 있는 골 확인
        if(GameObject.FindGameObjectWithTag("Goal_Fire") != null)
        {
          goalFire = GameObject.FindGameObjectWithTag("Goal_Fire").GetComponent<GoalObject>();
            goalWater = GameObject.FindGameObjectWithTag("Goal_Water").GetComponent<GoalObject>();

            if (goalFire == null || goalWater == null)
            {
                Debug.LogError("No Goal assigned, plz check goals in scene");
            }
            else
            {
                // 이벤트 구독
                goalFire.OnActivated += OnGoalOpend;
                goalWater.OnActivated += OnGoalOpend;
            }  
        }    
    }

    private void ClearStage()
    {
        Debug.Log($"{currentScene.name} clear!!!");
    }   
    private void OnGoalOpend()
    {
        if(goalWater.isOpen && goalFire.isOpen)
        {
            Debug.Log("Both opened");
            StageClear();
        }
    }
    
    private void ExitStage()
    {
        // 현재 씬 종료

        // 메모리 누수 방지용 구독 해제???
        goalFire.OnActivated -= OnGoalOpend;
        goalWater.OnActivated -= OnGoalOpend;
    }
}
