using System.Collections.Generic;
using UnityEngine;

public class AchievementUIManager : MonoBehaviour
{
    [Header("References")]
    public Transform contentParent;  // Scroll View 안의 Content
    public GameObject achievementItemPrefab;

    [SerializeField] private GameObject targetPanel;

    private List<AchievementItem> spawnedItems = new List<AchievementItem>();

    void Start()
    {
        
    }

    public void PopulateUI()
    {
        // 도전과제 목록
        var achievements = AchievementManager.Instance.achievements;
        // 이미 있는 도전과제 아이템들 삭제
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);
        spawnedItems.Clear();
        // 도전과제 수만큼 viewport의 content 밑에 자식으로 생성, 데이터 적용
        foreach (var ach in achievements)
        {
            GameObject obj = Instantiate(achievementItemPrefab, contentParent);
            AchievementItem item = obj.GetComponent<AchievementItem>();
            item.SetData(ach);
            spawnedItems.Add(item);
        }
    }

    public void RefreshUI()
    {
        var achievements = AchievementManager.Instance.achievements;
        for (int i = 0; i < achievements.Count; i++)
        {
            spawnedItems[i].SetData(achievements[i]);
        }
    }

    public void OpenPanel()
    {
        targetPanel.SetActive(true);
        PopulateUI();
    }

    public void ClosePanel()
    {
        targetPanel.SetActive(false);
    }
}
