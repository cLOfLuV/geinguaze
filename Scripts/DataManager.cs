using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class ShopData
{
    public List<SlotData> SlotDatas = new List<SlotData>();
}

[Serializable]
public class SlotData
{
    public string Rcode;
    public string Name;
    public string Description;
    public int Price;
    public bool IsSold;
}

[Serializable]
public class UserInfo
{
    public int Amount;
    public float Speed;
    public int Coin;
    public bool IsAuto;
}

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public UserInfo UserInfo;
    public ShopData ShopDatas;

    public event Action OnSaveData;

    private readonly string USERINFO_PATH = Path.Combine(Application.dataPath, "Resources/Data/UserInfo.json");
    private readonly string SHOPDATA_PATH = Path.Combine(Application.dataPath, "Resources/Data/SlotDatas.json");

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadData()
    {
        LoadUserInfo();
        LoadShopData();
    }

    private void LoadUserInfo()
    {
        if (!File.Exists(USERINFO_PATH))
        {
            UserInfo = new UserInfo { Amount = 1, Speed = 0.01f, Coin = 9 };
        }
        else
        {
            var jsonUserInfoData = File.ReadAllText(USERINFO_PATH);
            UserInfo = JsonUtility.FromJson<UserInfo>(jsonUserInfoData);
        }
    }

    private void LoadShopData()
    {
        if (!File.Exists(SHOPDATA_PATH))
        {
            Debug.LogWarning("아이템 데이터 없음");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        else
        {
            var jsonItemData = File.ReadAllText(SHOPDATA_PATH);
            ShopDatas = JsonUtility.FromJson<ShopData>(jsonItemData);
        }
    }

    public void SaveData()
    {
        OnSaveData?.Invoke();
        Mining.instance.SaveData();
        SaveUserInfo();
        SaveShopData();
    }

    private void SaveUserInfo()
    {
        var userData = JsonUtility.ToJson(UserInfo);
        File.WriteAllText(USERINFO_PATH, userData);
        Debug.Log("User data saved to " + USERINFO_PATH);
    }

    private void SaveShopData()
    {
        var shopData = JsonUtility.ToJson(ShopDatas);
        File.WriteAllText(SHOPDATA_PATH, shopData);
        Debug.Log("Shop data saved to " + SHOPDATA_PATH);
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }
}