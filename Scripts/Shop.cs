using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public List<ItemSlot> Slots = new List<ItemSlot>();
    private ShopData shopDatas;
    private Coroutine saveDataCoroutine;

    private void Awake()
    {
        shopDatas = DataManager.instance.ShopDatas;
    }

    private void OnEnable()
    {
        DataManager.instance.OnSaveData += UpdateShopData;
        foreach (ItemSlot slot in Slots)
        {
            slot.OnBuyItem += UpdateShopData;
        }
    }

    private void OnDisable()
    {
        DataManager.instance.OnSaveData -= UpdateShopData;
        foreach (ItemSlot slot in Slots)
        {
            slot.OnBuyItem -= UpdateShopData;
        }

        if (saveDataCoroutine != null)
        {
            StopCoroutine(saveDataCoroutine);
        }
    }

    private void Start()
    {
        SetSlots();
        saveDataCoroutine = StartCoroutine(SaveDataPeriodically());
    }

    private void SetSlots()
    {
        for (int i = 0; i < Slots.Count; i++)
        {
            foreach (var slotData in shopDatas.SlotDatas)
            {
                if (Slots[i].Rcode == slotData.Rcode)
                {
                    Slots[i].SlotData = slotData;
                    break;
                }
            }
        }
    }

    private IEnumerator SaveDataPeriodically()
    {
        var waitTime = new WaitForSeconds(30.0f);
        while (true)
        {
            yield return waitTime; // 30초마다 저장
            UpdateShopData();
            DataManager.instance.SaveData();
        }
    }

    public void UpdateShopData()
    {
        for (int i = 0; i < Slots.Count; i++)
        {
            shopDatas.SlotDatas[i] = Slots[i].SlotData;
        }
        DataManager.instance.ShopDatas = shopDatas;
    }
}