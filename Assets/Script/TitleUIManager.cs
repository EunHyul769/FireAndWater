using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUIManager : MonoBehaviour
{
    public void OnClickStart()
    {
        SceneManager.LoadScene("StageChoiceScene"); 
    }

    public void OnClickCredit()
    {
        SceneManager.LoadScene("CreditScene");
    }

    public void OnClickQuit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
