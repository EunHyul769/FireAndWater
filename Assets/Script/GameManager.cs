using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // scene ready
    [SerializeField] private Scene[] scenes;
    [SerializeField] private Scene currentScene;

    // stage run/stop
    [SerializeField] private bool isRunning = true;

    // stage clear
    private float clearTime;
    [SerializeField] private GoalObject goalFire;
    [SerializeField] private GoalObject goalWater; 
    private static GameManager instance;

    // singleton
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }

    // 게임의 모든 데이터는 GameManager가 소유하도록

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        startStage(99); // 골 테스트용 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && isRunning)
        {
            PauseStage();
        }
        else if (Input.GetKeyDown(KeyCode.C) && !isRunning)
        {
            CountinueStage();
        }
        
    }

    private void StartGame()
    {
        // 이전 게임 정보 불러오기
    }

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

    private void GetStageInfo()
    {
        
    }

    private void PauseStage()
    {
        isRunning = false;
        Time.timeScale = 0f;
        //TimeManager.Instance.StopTimer();
    }

    private void CountinueStage()
    {
        isRunning = true;
        Time.timeScale = 1f;
        //TimeManager.Instance.StartTimer();
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
            ClearStage();
        }
    }

    private void RetryStage()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    private void ExitStage()
    {
        // 현재 씬 종료

        // 메모리 누수 방지용 구독 해제???
        goalFire.OnActivated -= OnGoalOpend;
        goalWater.OnActivated -= OnGoalOpend;
    }
}
