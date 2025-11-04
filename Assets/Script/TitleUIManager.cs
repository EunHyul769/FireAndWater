using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUIManager : MonoBehaviour
{

    public GameObject creditPanel;

    private void Start()
    {
        creditPanel.SetActive(false);
    }

    public void OnClickStart()
    {
        SceneManager.LoadScene("StageChoiceScene"); 
    }

    public void OnClickCredit()
    {
        creditPanel.SetActive(true);
    }

    public void OnClickBackFromCredit()
    {
        creditPanel.SetActive(false);
    }

    public void OnClickHidden()
    {
        if (BGMManager.Instance != null)
            BGMManager.Instance.Stop(1.0f); // BGM 1초간 페이드아웃

        SceneManager.LoadScene("HiddenIntroScene");
    }

    public void OnClickQuit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
