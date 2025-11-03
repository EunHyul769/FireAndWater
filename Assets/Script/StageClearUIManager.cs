using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class StageClearUIManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject stageClearPanel;
    public TextMeshProUGUI clearTimeText;

    [SerializeField] private GameObject[] starFronts;

    private void Start()
    {
        stageClearPanel.SetActive(false);
    }

    public void ShowStageClearUI(float clearTime)
    {
        int minutes = Mathf.FloorToInt(clearTime / 60);
        int seconds = Mathf.FloorToInt(clearTime % 60);
        int milliseconds = Mathf.FloorToInt((clearTime * 100) % 100);
        clearTimeText.text = $"클리어 타임\n{minutes:00}:{seconds:00}.{milliseconds:00}";
        stageClearPanel.SetActive(true);

        StartCoroutine(StarCheck());
    }

    public void OnClickRetry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnClickNext()
    {
        GameManager.Instance.OnClickNextStage();
    }

    public void OnClickStageSelect()
    {
        SceneManager.LoadScene("StageChoiceScene");
    }

    public void OnClickMainMenu()
    {
        SceneManager.LoadScene("TitleScene");
    }

    private IEnumerator StarCheck()
    {

        int starNum = GameManager.Instance.ScoreCheck();

        while (starNum - 1 >= 0)
        {
            yield return new WaitForSeconds(0.75f);

            if (starNum -1  < 0 || starNum -1 >= starFronts.Length)
            {
            Debug.LogWarning($"Star index {starNum} out of range (starNum={starNum})");
            yield break;
            }

            Debug.Log(starNum);
            starFronts[starNum - 1].SetActive(true);

            starNum--;
            
        }
    }
}
