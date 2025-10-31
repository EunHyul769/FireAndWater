using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageChoiceUIManager : MonoBehaviour
{
    // 스테이지 1 버튼
    public void OnClickStage1()
    {
        GameManager.Instance.LoadNextStage("Level1");
    }

    // 스테이지 2 버튼
    public void OnClickStage2()
    {
        GameManager.Instance.LoadNextStage("Level2");
    }

    public void OnClickStage3()
    {
        GameManager.Instance.LoadNextStage("Level3");
    }

    // 타이틀로 돌아가기 버튼
    public void OnClickReturnToTitle()
    {
        GameManager.Instance.ReturnToTitle();
    }
}
