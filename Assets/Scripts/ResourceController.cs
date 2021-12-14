using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ResourceController : MonoBehaviour
{
    public Button ResourceButton;
    public Image ResourceImage;
    public Text ResourceDescription;
    public Text ResourceUpgradeCost;
    public Text ResourceUnlockCost;

    private ResourceConfig _config;
    private int level = 1;
    public bool IsUnlocked { get; private set; }

    private void Start()
    {
        ResourceButton.onClick.AddListener(() =>
        {
            if (IsUnlocked)
            {
                UpgradeLevel();
            }
            else
            {
                UnlockResource();
            }
        });
    }

    private int _index;

    private int _level

    {
        set

        {
            // Menyimpan value yang di set ke _level pada Progress Data

            UserDataManager.Progress.ResourcesLevels[_index] = value;

            UserDataManager.Save();
        }

        get

        {
            // Mengecek apakah index sudah terdapat pada Progress Data

            if (!UserDataManager.HasResources(_index))

            {
                // Jika tidak maka tampilkan level 1

                return 1;
            }

            // Jika iya maka tampilkan berdasarkan Progress Data

            return UserDataManager.Progress.ResourcesLevels[_index];
        }
    }
    public void SetConfig(int index, ResourceConfig config)

    {
        _index = index;
        _config = config;

        // ToString("0") berfungsi untuk membuang angka di belakang koma
        ResourceDescription.text = $"{ _config.Name } Lv. { _level }\n+{ GetOutput().ToString("0") }";
        ResourceUnlockCost.text = $"Unlock Cost\n{ _config.UnlockCost }";
        ResourceUpgradeCost.text = $"Upgrade Cost\n{ GetUpgradeCost() }";
        SetUnlocked(_config.UnlockCost == 0 || UserDataManager.HasResources(_index));
    }
    public void UnlockResource()
    {
        double unlockCost = GetUnlockCost();
        if (UserDataManager.Progress.Gold < unlockCost)
        {
            return;
        }
        SetUnlocked(true);
        GameManager.Instance.ShowNextResource();
        AchievementController.Instance.UnlockAchievement(AchievementType.UnlockResource, _config.Name);
    }
    public void SetUnlocked(bool unlocked)

    {
        IsUnlocked = unlocked;

        if (unlocked)

        {
            // Jika resources baru di unlock dan belum ada di Progress Data, maka tambahkan data

            if (!UserDataManager.HasResources(_index))

            {
                UserDataManager.Progress.ResourcesLevels.Add(_level);

                UserDataManager.Save();
            }
        }
    }

    public double GetOutput()
    {
        return _config.Output * level;
    }

    public double GetUpgradeCost()
    {
        return _config.UpgradeCost * level;
    }

    public double GetUnlockCost()
    {
        return _config.UnlockCost;
    }

    public void UpgradeLevel()

    {
        double upgradeCost = GetUpgradeCost();
        if (UserDataManager.Progress.Gold < upgradeCost)
        {
            return;
        }
        GameManager.Instance.AddGold(-upgradeCost);
        level++;
        ResourceUpgradeCost.text = $"Upgrade Cost\n{ GetUpgradeCost() }";
        ResourceDescription.text = $"{ _config.Name } Lv. { level }\n+{ GetOutput().ToString("0") }";
    }
}