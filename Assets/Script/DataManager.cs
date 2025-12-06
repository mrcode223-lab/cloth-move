using System.Collections.Generic;
using UnityEngine;

public static class DataManager
{
    private const string CoinKey = "CoinKey";
    private const string CurrentLevelKey = "CurrentLevelKey";
    private const string ExpkKey = "ExpkKey";
    private const string PurchasedSkinKey = "PurchasedSkinKey";
    private const string SelectedSkinKey = "SelectedSkinKey";

    private static HashSet<int> purchasedSkinIds = new HashSet<int>();

    static DataManager()
    {
        LoadPurchasedSkins();
    }

    #region --- LEVEL & EXP ---
    public static float GetExpRequired(int level)
    {
        float expBase = 100f;
        float growthRate = 1.2f;
        return Mathf.Round(expBase * Mathf.Pow(growthRate, level - 1));
    }
    #endregion

    #region --- COIN ---
    public static int Coin
    {
        get => PlayerPrefs.GetInt(CoinKey, 0);
        set
        {
            PlayerPrefs.SetInt(CoinKey, Mathf.Max(0, value));
            PlayerPrefs.Save();
        }
    }

    public static void AddCoin(int amount)
    {
        Coin = Mathf.Max(0, Coin + amount);
        PlayerPrefs.Save();
    }
    #endregion

    #region --- LEVEL ---
    public static int CurrentLevel
    {
        get => PlayerPrefs.GetInt(CurrentLevelKey, 1);
        set => PlayerPrefs.SetInt(CurrentLevelKey, value);
    }

    public static int CurrenExp
    {
        get => PlayerPrefs.GetInt(ExpkKey, 0);
        set => PlayerPrefs.SetInt(ExpkKey, value);
    }
    #endregion

    #region --- SKIN MANAGEMENT ---
    public static void SetSelectedSkin(SkinType skinType)
    {
        PlayerPrefs.SetInt(SelectedSkinKey, (int)skinType);
        PlayerPrefs.Save();
    }

    public static SkinType GetSelectedSkin()
    {
        if (!PlayerPrefs.HasKey(SelectedSkinKey))
            return SkinType.Skin1;

        return (SkinType)PlayerPrefs.GetInt(SelectedSkinKey);
    }

    /// <summary>
    /// Kiểm tra xem skin đã được mua chưa
    /// </summary>
    public static bool IsSkinOwned(SkinType skinType)
    {
        return purchasedSkinIds.Contains((int)skinType);
    }

    /// <summary>
    /// Mở khóa skin (thường sau khi mua)
    /// </summary>
    public static void AddOwnedSkin(SkinType skinType)
    {
        int id = (int)skinType;
        if (!purchasedSkinIds.Contains(id))
        {
            purchasedSkinIds.Add(id);
            SavePurchasedSkins();
        }
    }

    private static void SavePurchasedSkins()
    {
        string data = string.Join(",", purchasedSkinIds);
        PlayerPrefs.SetString(PurchasedSkinKey, data);
        PlayerPrefs.Save();
    }

    private static void LoadPurchasedSkins()
    {
        purchasedSkinIds.Clear();

        bool firstTimeSetup = false;

        // ✅ Nếu chưa từng lưu PurchasedSkinKey → người chơi mới
        if (!PlayerPrefs.HasKey(PurchasedSkinKey))
        {
            firstTimeSetup = true;
            purchasedSkinIds.Add((int)SkinType.None);
            SavePurchasedSkins();
        }
        else
        {
            string data = PlayerPrefs.GetString(PurchasedSkinKey);
            if (!string.IsNullOrEmpty(data))
            {
                var ids = data.Split(',');
                foreach (var idStr in ids)
                {
                    if (int.TryParse(idStr, out int id))
                        purchasedSkinIds.Add(id);
                }
            }

            // ✅ Nếu dữ liệu cũ nhưng thiếu Skin1 thì thêm lại
            if (!purchasedSkinIds.Contains((int)SkinType.None))
            {
                purchasedSkinIds.Add((int)SkinType.None);
                SavePurchasedSkins();
            }
        }

        // ✅ Nếu chưa có SelectedSkinKey → chọn mặc định Skin1
        if (!PlayerPrefs.HasKey(SelectedSkinKey) || firstTimeSetup)
        {
            PlayerPrefs.SetInt(SelectedSkinKey, (int)SkinType.None);
            PlayerPrefs.Save();
        }
    }

    #endregion
}
