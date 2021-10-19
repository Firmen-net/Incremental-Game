using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementController : MonoBehaviour
{
    private void Update()
    {
        if (popUpShowDurationCounter > 0)
        {
            // Kurangi durasi ketika pop up durasi lebih dari 0
            popUpShowDurationCounter -= Time.unscaledDeltaTime;
            // Lerp adalah fungsi linear interpolation, digunakan untuk mengubah value secara perlahan
            popUpTransform.localScale = Vector3.LerpUnclamped(popUpTransform.localScale, Vector3.one, 0.5f);
        }
        else
        {
            popUpTransform.localScale = Vector2.LerpUnclamped(popUpTransform.localScale, Vector3.right, 0.5f);
        }
    }

    private static AchievementController _instance = null;
    public static AchievementController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AchievementController>();
            }

            return _instance;
        }
    }
    [SerializeField] private Transform popUpTransform;
    [SerializeField] private Text popUpText;
    [SerializeField] private float popUpShowDuration = 3f;
    [SerializeField] private List<AchievementData> achievementList;

    private float popUpShowDurationCounter;

    

    public void UnlockAchievement(AchievementType type, string value)
    {
        // Mencari data achievement
        AchievementData achievement = achievementList.Find(a => a.Type == type && a.Value == value);
        if (achievement != null && !achievement.IsUnlocked)
        {
            achievement.IsUnlocked = true;
            ShowAchivementPopUp(achievement);
        }
    }

    private void ShowAchivementPopUp(AchievementData achievement)
    {
        popUpText.text = achievement.Title;
        popUpShowDurationCounter = popUpShowDuration;
        popUpTransform.localScale = Vector2.right;
    }
}

[System.Serializable]
public class AchievementData
{
    public string Title;
    public AchievementType Type;
    public string Value;
    public bool IsUnlocked;
}

public enum AchievementType
{
    UnlockResource
}