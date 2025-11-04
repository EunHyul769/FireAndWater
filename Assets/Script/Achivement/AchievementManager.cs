using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;

    private string savePath; 
    public List<Achievement> achievements = new List<Achievement>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        savePath = Path.Combine(Application.persistentDataPath, "Achievements.json");

        LoadAchievements();
    }

    // JSON에서 불러오기
    void LoadAchievements()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            achievements = JsonUtility.FromJson<AchievementList>(json).list;
        }
        else
        {
            // 기본 데이터 로드 (Resources 폴더에서)
            TextAsset jsonFile = Resources.Load<TextAsset>("Achievements");
            achievements = JsonUtility.FromJson<AchievementList>(jsonFile.text).list;

            SaveAchievements();
        }

        Debug.Log($"achievements's count = {achievements.Count}");
    }


    // 저장
    public void SaveAchievements()
    {
        Debug.Log("Save achievements");
        Debug.Log(savePath);
        AchievementList wrapper = new AchievementList { list = achievements };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(savePath, json);
    }

    // 도전과제 해금
    public void UnlockAchievement(string id)
    {
        Achievement achievement = achievements.Find(a => a.id == id);
        if (achievement != null && !achievement.isUnlocked)
        {
            achievement.isUnlocked = true;
            Debug.Log($"도전과제 해금: {achievement.title}");
            SaveAchievements();
        }
    }

    [System.Serializable]
    private class AchievementList
    {
        public List<Achievement> list;
    }

    public void AddProgress(string id, int amount)
    {
        Achievement a = achievements.Find(x => x.id == id);
        if (a == null) return;

        bool unlocked = a.AddProgress(amount);
        if (unlocked)
        {
            Debug.Log($"도전과제 달성: {a.title}");
        }

        SaveAchievements();

        AchievementUIManager ui = FindObjectOfType<AchievementUIManager>();
        if (ui != null)
            ui.RefreshUI();
    }

    public void AdjustProgress(string id, int amount)
    {
        Debug.Log("start adjust");
        Achievement a = achievements.Find(x => x.id == id);
        if (a == null) return;

        bool unlocked = a.AdjustProgress(amount);
        Debug.Log($"amount adjust, {unlocked}");

        if (unlocked)
        {
            Debug.Log($"도전과제 달성: {a.title}");
        }

        SaveAchievements();

        AchievementUIManager ui = FindObjectOfType<AchievementUIManager>();
        if (ui != null)
            ui.RefreshUI();
    }
}
