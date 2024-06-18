using System.Collections;
using UnityEngine;
using TMPro;

public class Mining : MonoBehaviour
{
    public static Mining instance;
    public TextMeshProUGUI CoinTxt;

    public int amount;
    public float speed;
    public int coin;
    public bool IsAuto;

    private Coroutine autoClickCoroutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeUserData();
        UpdateCoinText();
        StartAutoClickCoroutine();
    }

    private void OnEnable()
    {
        StartAutoClickCoroutine();
    }

    private void OnDisable()
    {
        StopAutoClickCoroutine();
    }

    private void InitializeUserData()
    {
        var userInfo = DataManager.instance.UserInfo;
        amount = userInfo.Amount;
        speed = userInfo.Speed;
        coin = userInfo.Coin;
        IsAuto = userInfo.IsAuto;
    }

    private void StartAutoClickCoroutine()
    {
        if (autoClickCoroutine == null)
        {
            autoClickCoroutine = StartCoroutine(AutoClick());
        }
    }

    private void StopAutoClickCoroutine()
    {
        if (autoClickCoroutine != null)
        {
            StopCoroutine(autoClickCoroutine);
            autoClickCoroutine = null;
        }
    }

    private IEnumerator AutoClick()
    {
        while (true)
        {
            if (IsAuto)
            {
                yield return new WaitForSeconds(Mathf.Max(0.1f, 1.0f - speed)); // Speed가 1.0 이상일 때 대기 시간이 음수가 되지 않도록 함
                IncreaseCoin();
            }
            else
            {
                yield return null;
            }
        }
    }

    public void IncreaseAmount()
    {
        amount += 1;
    }

    public void IncreaseSpeed()
    {
        speed += 0.01f;
    }

    public void IncreaseCoin()
    {
        coin += amount;
        UpdateCoinText();
    }

    public void UpdateCoinText()
    {
        CoinTxt.text = coin.ToString();
    }

    public void SaveData()
    {
        var userInfo = DataManager.instance.UserInfo;
        userInfo.Amount = amount;
        userInfo.Speed = speed;
        userInfo.Coin = coin;
        userInfo.IsAuto = IsAuto;
    }
}