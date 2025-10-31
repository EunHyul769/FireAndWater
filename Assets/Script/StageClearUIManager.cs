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
    }

    public void OnClickRetry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnClickNextStage(string nextStage)
    {
        SceneManager.LoadScene(nextStage);
    }

    public void OnClickStageSelect()
    {
        SceneManager.LoadScene("StageChoiceScene");
    }

    public void OnClickMainMenu()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
