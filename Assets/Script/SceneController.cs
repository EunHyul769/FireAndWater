using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void OnStageClear()
    {
        GameManager.Instance.StageClear();
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
